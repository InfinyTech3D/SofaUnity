# SOFA Scene Autotester

Opens Unity scenes one at a time and checks that:

1. **Unity does not crash or hang** while loading and playing the scene. A crash in the SOFA DLL is caught here.
2. **The `SofaContext` is running**: the average SOFA FPS stays above a threshold for the measured period.
3. **The log is clean**: errors logged while the scene runs give a warning, or a failure if you ask for it.

Each scene runs in its **own Unity process** in batchmode. A crash in the native SOFA DLL only stops that scene's test; the runner itself survives and moves on to the next scene. Each run also loads the SOFA DLLs fresh, so you can swap in a new SOFA build between runs.

## Files

| File | Role |
|---|---|
| `run_scene_tests.py` | Python runner. Reads the JSON config, starts Unity once per scene, enforces the timeout, classifies the result and prints a summary. |
| `Lists/` | The SofaUnity scene lists (configs): which scenes to test and with which settings. |
| `Lists/scene_tests.json` | Small default list, used when no list option is given. |
| `Lists/sofaunity_all_scene_tests.json` | Every `.unity` scene under `Assets/SofaUnity`. |

The lists of the other projects live in those projects, so each project keeps its own tests:

| Lists folder | Results folder | Option | Lists |
|---|---|---|---|
| `Assets/SurgiViz4D/Tests/Lists/` | `Assets/SurgiViz4D/Tests/Results/` | `-s` / `--surgiviz` | `public_surgiviz_scene_tests` (the `PublicRelease` group), `surgiviz_full_scene_tests` (operations and samples) |
| `Assets/SofaLnRobotics/Tests/Lists/` | `Assets/SofaLnRobotics/Tests/Results/` | `-l` / `--lnrobotics` | `LNR_scene_tests` (the `LnRobotics` group) |

`Tests/Results*` is in the SurgiViz4D `.gitignore`. SofaLnRobotics is not in git.
| `Editor/SofaSceneAutoTester.cs` | The probe that runs inside Unity. The Python script calls it through `-executeMethod`, and it does the actual check: open the scene, turn on the `SofaContext`, measure SOFA FPS. **Required.** It only acts when Unity is started with `-sofaTestOutput`; normal editor use is not affected. |
| `result/` | Output of the SofaUnity lists. **Ignored by git** (`.gitignore`: `Tests/sceneAutotester/result/`). |
| `result/results~/`, `result/results_all~/` | One folder per list (set by `output_dir`): one `.json` and one `.log` per scene, plus `summary.json`. Unity ignores folders ending in `~`, so it doesn't import these files. |

## Requirements

- Python 3.8 or newer, standard library only.
- The Unity version from `ProjectSettings/ProjectVersion.txt`, installed through Unity Hub. Otherwise, set `unity_editor` in the config.
- **The project must not be open in the Unity Editor.** Unity locks a project to one editor at a time. The runner checks this first and stops with a message if the project is open.

## How to run

All commands are run from this folder.

Run every scene in `Lists/scene_tests.json`:

```bash
python run_scene_tests.py
```

Run only some scenes, using their `name` from the config. `--scene` can be repeated:

```bash
python run_scene_tests.py --scene Example_01_SimpleLiver
python run_scene_tests.py --scene MainSalpingectomy_v2 --scene Multi_US_Haptic_OpenHepaticSurgery
```

Use another list from `Lists/`, for example every SofaUnity scene:

```bash
python run_scene_tests.py --config sofaunity_all_scene_tests.json
```

Use a list of another project:

```bash
python run_scene_tests.py -s public_surgiviz_scene_tests
python run_scene_tests.py -l LNR_scene_tests
```

| Option | Looks for the list in | Writes results to |
|---|---|---|
| `--config <list>` (or no option) | `Tests/sceneAutotester/Lists/` | `Tests/sceneAutotester/result/` |
| `-s <list>`, `--surgiviz <list>` | `Assets/SurgiViz4D/Tests/Lists/` | `Assets/SurgiViz4D/Tests/Results/` |
| `-l <list>`, `--lnrobotics <list>` | `Assets/SofaLnRobotics/Tests/Lists/` | `Assets/SofaLnRobotics/Tests/Results/` |

- Use only one of these options per run.
- The `.json` extension is optional.
- With `--config` only, a path that contains a folder (e.g. `--config C:/somewhere/my_tests.json`) is used as is.
- Any of them can be combined with `--scene`, e.g. `python run_scene_tests.py -s surgiviz_full_scene_tests --scene QuitScene`.

Results are written inside the list's `output_dir`, in the results folder that matches the option (see the table above).

**Exit code:** `0` if every scene is `PASS` or `WARN`, `1` if any scene failed, so the script can be used from a scheduled task or CI.

## What happens for one scene

1. The runner deletes the scene's old result and log, then starts:
   `Unity.exe -batchmode -projectPath <project> -executeMethod SofaScripts.SofaSceneAutoTester.Run -sofaTestScene <path> -sofaTestOutput <json> ...`
2. Inside Unity, the probe opens the scene and enters Play mode.
3. On the first play frame, it finds the `SofaContext`, including one on an inactive GameObject, and turns it on:
   - activates its GameObject and all parents
   - enables the component
   - sets `IsSofaUpdating = true`
4. It waits `warmup` seconds, then samples `SofaContext.SimulationFPS` for `duration` seconds. During the whole Play session it also records every error, exception and assert in the Unity log, grouping identical messages with a count.
5. It writes the result JSON and closes Unity: exit code `0` for PASS, `1` for FAIL.
6. Meanwhile the runner waits up to `timeout` seconds. Afterwards it:
   - combines the probe result with what it saw from outside
   - removes the errors that match `ignore_errors`
   - turns a `PASS` that still has errors into `WARN`, or into `FAIL` if `fail_on_errors` is set
   - writes the final result file, prefixed according to the outcome (see [Result file names](#result-file-names))

## Result status

| Status | Meaning |
|---|---|
| `PASS` | A `SofaContext` was found, the average SOFA FPS was above `min_fps`, and no error was logged (apart from ignored ones). |
| `WARN` | SOFA is running like a `PASS`, but errors were logged, e.g. `Grasping plugin not found`. The scene works but something is missing or wrong. |
| `FAIL` | Unity ran fine, but either no `SofaContext` was found, SOFA is not simulating (average FPS ≤ `min_fps`), or errors were logged with `fail_on_errors` set. |
| `CRASH` | Unity exited without writing a result. Usually a native crash; the log contains `Crash!!!` / `Native Crash Reporting`. |
| `HANG` | No result after `timeout` seconds. The runner killed Unity and every process it started. Usual causes: a deadlock or infinite loop in SOFA, a modal dialog, or a scene that takes longer to load than the timeout. |
| `ERROR` | The scene file in `path` doesn't exist (Unity isn't started), or Unity stopped batchmode before testing, for example because of script compile errors. Check the reason and the log. |

### How log errors are handled

Errors only matter for a scene where SOFA is running; a scene that already failed stays failed.

1. Errors whose message **contains** a text from `ignore_errors` are moved to `ignoredErrors`. They don't count.
2. If errors remain:
   - by default the scene becomes `WARN`
   - with `"fail_on_errors": true` it becomes `FAIL` (file `ERROR_<name>.json`)
3. If no errors remain, the scene is `PASS`.

Typical uses:
- Accept a known error for one scene, while still catching new ones:
  ```json
  {
      "name": "MainSalpingectomy_v2",
      "path": "Assets/SurgiViz4D/Operations/Gynecologic_Surgery/Patients/IT3D_Patient_03/MainSalpingectomy_v2.unity",
      "ignore_errors": ["Garsping plugin not found"]
  }
  ```
- Make a scene that must be clean fail on any error: `"fail_on_errors": true`.
- Ignore an error everywhere: put its text in `defaults.ignore_errors`.

## Result file names

So that problems stand out when you browse the results folder, the result JSON name depends on the outcome:

| File name | When |
|---|---|
| `<name>.json` | `PASS` |
| `WARN_<name>.json` | `WARN`: SOFA is running but errors were logged |
| `ERROR_<name>.json` | `FAIL`, `CRASH`, `HANG` or `ERROR` |
| `EMPTY_<name>.json` | No `SofaContext` was found in the scene. This takes priority over `ERROR_`. |

Before a scene runs, every version of its result file is deleted, so only the latest outcome is left. The log is always `<name>.log`.

## The list file (e.g. `Lists/scene_tests.json`)

```json
{
    "unity_editor": "",
    "project_path": "",
    "output_dir": "results~",
    "defaults": {
        "warmup": 3,
        "duration": 5,
        "min_fps": 0,
        "timeout": 300,
        "fail_on_errors": false,
        "ignore_errors": []
    },
    "scenes": [
        {
            "name": "Example_01_SimpleLiver",
            "path": "Assets/SofaUnity/Scenes/Examples/Example_01_SimpleLiver.unity"
        }
    ]
}
```

### Top-level settings

| Key | Meaning |
|---|---|
| `unity_editor` | Full path to `Unity.exe`. Leave it empty to use `C:\Program Files\Unity\Hub\Editor\<project version>\Editor\Unity.exe`. |
| `project_path` | Path to the Unity project root (the folder that contains `Assets/` and `ProjectSettings/`). Leave it empty to find it automatically by searching up from this folder. |
| `output_dir` | The folder inside the results folder (`result/`, or `Tests/Results/` of SurgiViz4D / SofaLnRobotics) where results and logs are written. Give each list its own folder so runs don't overwrite each other, and end the name with `~` so Unity doesn't import it. |
| `defaults` | Settings applied to every scene unless the scene overrides them (see below). |
| `scenes` | The list of scenes to test. |

### Scene settings

You can put any of these in `defaults` or in a scene entry. A value set in the scene entry wins.

| Key | Unit | Meaning |
|---|---|---|
| `name` | – | **Required, per scene.** A unique short name. Used by `--scene` and for the result and log file names. |
| `path` | – | **Required, per scene.** The scene path relative to the project root, starting with `Assets/` and using forward slashes. |
| `warmup` | seconds | Time after Play starts before measuring. SOFA scene loading and the first FPS updates happen here. Increase it for heavy scenes. |
| `duration` | seconds | How long the SOFA FPS is sampled. |
| `min_fps` | FPS | The scene passes if the average SOFA FPS is **strictly greater** than this. `0` means "SOFA is running at all". Raise it to catch performance regressions. |
| `timeout` | seconds | The longest time allowed for the whole Unity process: startup, import, scene load, warm-up and measurement. After that the scene is `HANG`. Editor startup alone takes about 20–40 s, so keep it well above that. |
| `fail_on_errors` | true/false | `false` (default): errors logged in a running scene give `WARN`. `true`: they give `FAIL`. |
| `ignore_errors` | list of texts | Errors whose message contains one of these texts are not counted. The match is plain text and case-sensitive, not a regex. Unlike the other keys, the `defaults` list and the scene's list are **combined**, not replaced. |

## Adding a new scene

1. Find the scene's path from the project root, e.g. `Assets/SofaUnity/Scenes/Demos/Interaction/Demo_05_LiverCut.unity`. In Unity: right-click the scene → *Copy Path*.
2. Add an entry to `scenes` in a list, in the `Lists/` folder of the project the scene belongs to (or create a new list there):

   ```json
   {
       "name": "Demo_05_LiverCut",
       "path": "Assets/SofaUnity/Scenes/Demos/Interaction/Demo_05_LiverCut.unity"
   }
   ```

3. Optionally, override settings for this scene only, e.g. a heavy scene that needs more time and should keep a minimum speed:

   ```json
   {
       "name": "liverHD-CUDA",
       "path": "Assets/SofaUnity/Scenes/Benchmarks/liverHD-CUDA.unity",
       "warmup": 10,
       "min_fps": 30,
       "timeout": 600
   }
   ```

4. Run it on its own once to check it. Add `--config`, `-s` or `-l` with the list name if it isn't in the default list:

   ```bash
   python run_scene_tests.py --scene Demo_05_LiverCut
   ```

The scene does **not** need to be in the Build Settings, and you don't need to add anything to the scene. The probe finds the `SofaContext` itself.

## Reading the results

The console prints a summary table:

```
Scene                                    Status  SOFA FPS avg  Errors  Reason
------------------------------------------------------------------------------------------
Example_01_SimpleLiver                   PASS           714.8       0  SOFA is running.
MainSalpingectomy_v2                     WARN            34.0       4  SOFA is running but 4 error(s) in log.
Multi_US_Haptic_OpenHepaticSurgery       HANG               -       -  No result after 300s, process killed.
```

`<results folder>/<output_dir>/[WARN_|ERROR_|EMPTY_]<name>.json` holds the full details for one scene:

| Field | Meaning |
|---|---|
| `scene`, `path` | The config name and the scene path. |
| `status`, `reason` | See [Result status](#result-status). |
| `exitCode` | The Unity process exit code; `null` if the runner killed it. |
| `elapsed` | Wall-clock seconds for the whole Unity run. |
| `contextFound` | Whether a `SofaContext` was found in the scene. |
| `contextWasDisabled` | Its GameObject or component was disabled, and the probe turned it on. |
| `contextWasPaused` | `IsSofaUpdating` was false, and the probe set it to true. |
| `fpsMin`, `fpsMax`, `fpsAvg`, `fpsSamples` | SOFA FPS statistics over `duration`. |
| `errorCount`, `errors` | The number of errors, exceptions and asserts that count, and the unique messages, each with its `count`. Up to 100 unique messages are kept; all occurrences are counted. |
| `ignoredErrorCount`, `ignoredErrors` | The same for errors removed by `ignore_errors`. |
| `log` | Path to the full Unity log for this scene. **Look here first for CRASH, HANG and ERROR.** |
| `result` | Path to this result file, including its prefix. |

`<results folder>/<output_dir>/summary.json` is the report of the last run. Each run overwrites it. It contains:
- a timestamp and the list that was run
- the counts `tested`, `passed`, `warned` and `failed`
- the full results of **only the scenes that did not pass**: failures first (`ERROR_`, `EMPTY_`), then warnings (`WARN_`)

Passing scenes are only counted there. Their details stay in their own `<name>.json`, and the console table still shows every scene.

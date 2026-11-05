"""Run SofaUnity scenes one by one in a fresh Unity editor (batchmode) and report
whether each scene crashed, hung, or has a running SofaContext (SOFA FPS > min_fps).

Usage:
    python run_scene_tests.py                      # all scenes of Lists/scene_tests.json
    python run_scene_tests.py --scene Example_01_SimpleLiver
    python run_scene_tests.py --config sofaunity_all_scene_tests.json   # read from Lists/
    python run_scene_tests.py -s public_surgiviz_scene_tests   # Assets/SurgiViz4D/Tests/Lists/

A bare list name is looked up in the Lists/ folder of SofaUnity (--config) or SurgiViz4D (-s);
".json" is optional. A --config path with folders is used as is.
Result JSON files are prefixed EMPTY_ when no SofaContext was found in the scene,
ERROR_ when the scene failed, and WARN_ when SOFA runs but errors were logged.

The Unity project must NOT be open in another editor (Unity locks the project).
Empty "unity_editor" / "project_path" in the config are auto-detected.
"""
import argparse
import json
import os
import subprocess
import sys
import time
from pathlib import Path

# Naming convention: every folder used by the autotester starts with an upper case letter
# (Lists, Results, Editor, and the output_dir of each list, e.g. Results_all~).
SCRIPT_DIR = Path(__file__).resolve().parent
LISTS_DIR = SCRIPT_DIR / "Lists"
# Lists of SurgiViz4D, relative to the Unity project root
SURGIVIZ_LISTS_DIR = Path("Assets/SurgiViz4D/Tests/Lists")
# each list writes into its own output_dir in the result folder of its project (ignored by git)
RESULT_DIR = SCRIPT_DIR / "Results"
SURGIVIZ_RESULT_DIR = Path("Assets/SurgiViz4D/Tests/Results")
EXECUTE_METHOD = "SofaScripts.SofaSceneAutoTester.Run"
CRASH_MARKERS = ("Crash!!!", "Native Crash Reporting")


def find_project_path(start: Path) -> Path:
    for parent in [start, *start.parents]:
        if (parent / "ProjectSettings" / "ProjectVersion.txt").is_file():
            return parent
    sys.exit("Could not find the Unity project (ProjectSettings/ProjectVersion.txt) above " + str(start))


def find_unity_editor(project: Path) -> Path:
    version = None
    for line in (project / "ProjectSettings" / "ProjectVersion.txt").read_text().splitlines():
        if line.startswith("m_EditorVersion:"):
            version = line.split(":", 1)[1].strip()
    editor = Path(r"C:\Program Files\Unity\Hub\Editor") / version / "Editor" / "Unity.exe"
    if not editor.is_file():
        sys.exit(f"Unity {version} not found at {editor}. Set 'unity_editor' in the config.")
    return editor


def project_is_open(project: Path) -> bool:
    lockfile = project / "Temp" / "UnityLockfile"
    if not lockfile.exists():
        return False
    try:  # a running editor keeps the lockfile open exclusively
        with open(lockfile, "a"):
            return False
    except PermissionError:
        return True


def kill_tree(pid: int):
    subprocess.run(["taskkill", "/T", "/F", "/PID", str(pid)], capture_output=True)


def read_log(log_file: Path) -> str:
    try:
        return log_file.read_text(encoding="utf-8", errors="replace")
    except OSError:
        return ""


def resolve_config(arg: str, lists_dir: Path) -> Path:
    path = Path(arg)
    if not path.is_absolute() and path.parent == Path("."):  # bare file name -> lists_dir
        path = lists_dir / path
    if path.suffix != ".json":
        path = path.with_name(path.name + ".json")
    if not path.is_file():
        sys.exit(f"Config file not found: {path}")
    return path.resolve()


RESULT_PREFIXES = ("EMPTY_", "ERROR_", "WARN_")


def result_prefix(result: dict) -> str:
    if result.get("contextFound") is False:
        return "EMPTY_"
    if result["status"] == "WARN":
        return "WARN_"
    if result["status"] != "PASS":
        return "ERROR_"
    return ""


def apply_error_rules(result: dict, ignore_errors: list, fail_on_errors: bool):
    """Remove ignored errors, then turn a PASS with remaining errors into WARN (or FAIL)."""
    kept, ignored = [], []
    for error in result.get("errors", []):
        (ignored if any(text in error["message"] for text in ignore_errors) else kept).append(error)
    result["errors"] = kept
    result["ignoredErrors"] = ignored
    result["ignoredErrorCount"] = sum(e["count"] for e in ignored)
    result["errorCount"] = result.get("errorCount", 0) - result["ignoredErrorCount"]

    if result["status"] == "PASS" and result["errorCount"] > 0:
        if fail_on_errors:
            result.update(status="FAIL", reason=f"SOFA is running but {result['errorCount']} error(s) in log (fail_on_errors).")
        else:
            result.update(status="WARN", reason=f"SOFA is running but {result['errorCount']} error(s) in log.")


def run_scene(scene: dict, defaults: dict, unity: Path, project: Path, out_dir: Path) -> dict:
    cfg = {**defaults, **scene}
    name = cfg["name"]
    result_file = out_dir / f"{name}.json"  # written by the Unity probe, renamed with a prefix below
    log_file = out_dir / f"{name}.log"
    for f in (result_file, log_file, *(out_dir / f"{p}{name}.json" for p in RESULT_PREFIXES)):
        if f.exists():
            f.unlink()

    if not (project / cfg["path"]).is_file():
        print(f"[{name}] scene file not found, skipped.", flush=True)
        result = {"scene": name, "path": cfg["path"], "status": "ERROR",
                  "reason": f"Scene file not found: {cfg['path']}"}
        final_file = out_dir / f"ERROR_{name}.json"
        result["result"] = str(final_file)
        final_file.write_text(json.dumps(result, indent=4), encoding="utf-8")
        return result

    cmd = [
        str(unity), "-batchmode",
        "-projectPath", str(project),
        "-executeMethod", EXECUTE_METHOD,
        "-logFile", str(log_file),
        "-sofaTestScene", cfg["path"],
        "-sofaTestOutput", str(result_file),
        "-sofaTestWarmup", str(cfg["warmup"]),
        "-sofaTestDuration", str(cfg["duration"]),
        "-sofaTestMinFps", str(cfg["min_fps"]),
    ]

    print(f"[{name}] running ...", flush=True)
    start = time.time()
    proc = subprocess.Popen(cmd)
    try:
        exit_code = proc.wait(timeout=cfg["timeout"])
        hung = False
    except subprocess.TimeoutExpired:
        kill_tree(proc.pid)
        exit_code = None
        hung = True
    elapsed = time.time() - start

    result = {"scene": name, "path": cfg["path"], "exitCode": exit_code, "elapsed": round(elapsed, 1), "log": str(log_file)}
    if result_file.is_file():
        probe = json.loads(result_file.read_text(encoding="utf-8"))
        probe.pop("scene", None)  # keep the config name, path is stored above
        result.update(probe)
        # ignore_errors lists of defaults and scene are combined
        ignore_errors = defaults.get("ignore_errors", []) + scene.get("ignore_errors", [])
        apply_error_rules(result, ignore_errors, cfg.get("fail_on_errors", False))
    else:
        log = read_log(log_file)
        if hung:
            result.update(status="HANG", reason=f"No result after {cfg['timeout']}s, process killed.")
        elif any(marker in log for marker in CRASH_MARKERS):
            result.update(status="CRASH", reason="Unity crashed (native crash in log).")
        elif "Scripts have compiler errors" in log or "Aborting batchmode" in log:
            result.update(status="ERROR", reason="Unity aborted batchmode (compile errors?), see log.")
        else:
            result.update(status="CRASH", reason=f"Unity exited (code {exit_code}) without writing a result.")

    if result_file.exists():
        result_file.unlink()
    final_file = out_dir / f"{result_prefix(result)}{name}.json"
    result["result"] = str(final_file)
    final_file.write_text(json.dumps(result, indent=4), encoding="utf-8")
    return result


def main():
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    lists = parser.add_mutually_exclusive_group()
    lists.add_argument("--config", help="list name in SofaUnity Lists/ (default: scene_tests), or a path to a list")
    lists.add_argument("-s", "--surgiviz", metavar="LIST", help=f"list name in {SURGIVIZ_LISTS_DIR.as_posix()}/")
    parser.add_argument("--scene", action="append", help="only run the scene(s) with this name")
    args = parser.parse_args()

    if args.surgiviz:
        config_path = resolve_config(args.surgiviz, find_project_path(SCRIPT_DIR) / SURGIVIZ_LISTS_DIR)
        result_dir = find_project_path(SCRIPT_DIR) / SURGIVIZ_RESULT_DIR
    else:
        config_path = resolve_config(args.config or "scene_tests.json", LISTS_DIR)
        result_dir = RESULT_DIR
    config = json.loads(config_path.read_text(encoding="utf-8"))

    project = Path(config["project_path"]) if config.get("project_path") else find_project_path(SCRIPT_DIR)
    unity = Path(config["unity_editor"]) if config.get("unity_editor") else find_unity_editor(project)
    out_dir = (result_dir / config.get("output_dir", "Results~")).resolve()
    out_dir.mkdir(parents=True, exist_ok=True)

    if project_is_open(project):
        sys.exit(f"The project {project} is open in a Unity editor. Close it and run again.")

    scenes = config["scenes"]
    if args.scene:
        scenes = [s for s in scenes if s["name"] in args.scene]
        if not scenes:
            sys.exit(f"No scene named {args.scene} in {config_path}")

    print(f"Unity:   {unity}\nProject: {project}\nResults: {out_dir}\n")
    results = [run_scene(s, config.get("defaults", {}), unity, project, out_dir) for s in scenes]

    print("\n" + "=" * 90)
    print(f"{'Scene':40} {'Status':7} {'SOFA FPS avg':>12} {'Errors':>7}  Reason")
    print("-" * 90)
    for r in results:
        fps = f"{r['fpsAvg']:.1f}" if "fpsAvg" in r else "-"
        print(f"{r['scene']:40} {r['status']:7} {fps:>12} {r.get('errorCount', '-'):>7}  {r['reason']}")
    print("=" * 90)

    passed = [r for r in results if r["status"] == "PASS"]
    warned = [r for r in results if r["status"] == "WARN"]
    failed = [r for r in results if r["status"] not in ("PASS", "WARN")]
    # only the problems (EMPTY_ / ERROR_ / WARN_) are listed, passing scenes are just counted
    summary = {"time": time.strftime("%Y-%m-%d %H:%M:%S"), "config": str(config_path),
               "tested": len(results), "passed": len(passed), "warned": len(warned), "failed": len(failed),
               "results": failed + warned}
    (out_dir / "summary.json").write_text(json.dumps(summary, indent=4), encoding="utf-8")

    print(f"{len(passed)} passed, {len(warned)} warning(s), {len(failed)} failed (of {len(results)}). "
          f"Summary: {out_dir / 'summary.json'}")
    sys.exit(1 if failed else 0)


if __name__ == "__main__":
    main()

# SOFAUnity-Renderer

## Description
This repository is a **Unity3D package allowing to load and visualize a SOFA simulation scene inside Unity3D**. 
Every SOFA `VisualModel` components from the loaded simulation will be rendered inside the Unity 3D scene, allowing therefor to use all Unity material and other assets on top of it.
SOFA simulation will be performed as soon as Unity is playing.

**<u>Important Note:</u>  This version allows only to change the Gravity and the TimeStep of the SOFA simulation and only the `VisualModel` are mapped into Unity3D. 
A full integration of SOFA components with a two-way communication can be requested here: https://infinytech3d.com/sapapi-unity3d/**

### Compatibility:
- Unity version > 2020.3.x
- SOFA > 22.06
- Only tested on Windows

## Installation guide
1. Install Unity engine version > 2020.3.x
2. (optionnal) Create a new project

### Installation from source:
3. clone the repo inside your unity project: 
```git clone git@github.com:InfinyTech3D/SofaUnity-Renderer.git /myUnityProject/Assets/SofaUnity```
5. Copy the **SOFA dll libraries** inside: ```/myUnityProject/Assets/SofaUnity/Plugins/Native/x64```  (for windows)

### Installation from asset:
3. install the unity asset: In Unity3D Editor, use ```Assets -> Import Package -> Custom Package``` and load the package **SofaUnity-Renderer.unitypackage**


## Usage
As soon as the **SofaUnity-Renderer.unitypackage** is loaded inside your Unity project, you will have access to a top menu panel called `SofaUnity`. 

<p align="center">
	<img src="https://github.com/InfinyTech3D/SofaUnity-Renderer/blob/main/Doc/img/menu_01.jpg" style="width:400px;"/>
</p>


This pannel allows to **load a SOFA scene** in only 2 steps:
1. **Click on SofaContext** to add a `GameObject` into your unity scene graph with a `SofaContext component`. 
This component correspond to the SOFA world 3D frame and provides a simple API to load a SOFA scene, change the Time Stepping and the Gravity. 

<p align="center">
	<img src="https://github.com/InfinyTech3D/SofaUnity-Renderer/blob/main/Doc/img/menu_02.jpg" style="width:400px;"/>
</p>

2. **Click on the button** ```Load SOFA Scene (.scn) file```  to load a SOFA scene. This will create a `GameObject` with a `Unity MeshFilter` for each SOFA ```VisualModel```

<p align="center">
	<img src="https://github.com/InfinyTech3D/SofaUnity-Renderer/blob/main/Doc/img/menu_03.jpg"/>
</p>


## Examples
Three examples corresponding to SOFA Demo folder are provided inside the package:
They are available in the folder [/Scenes/Demos/](https://github.com/InfinyTech3D/SofaUnity-Renderer/tree/main/Scenes/Demos)
- Demo_01_SimpleLiver -> Integration of  [Demos/liver.scn](https://github.com/sofa-framework/sofa/blob/master/examples/Demos/liver.scn)
- Demo_02_Caduceus -> Integration of  [Demos/caduceus.scn](https://github.com/sofa-framework/sofa/blob/master/examples/Demos/caduceus.scn)
- Demo_03_Tissue -> Integration of  [Demos/TriangleSurfaceCutting.scn](https://github.com/sofa-framework/sofa/blob/master/examples/Demos/TriangleSurfaceCutting.scn)

Here are a some results of the integration:
<p align="center">
	<img src="https://github.com/InfinyTech3D/SofaUnity-Renderer/blob/main/Doc/img/interface_01.jpg" style="width:1000px;"/>
</p>
<p align="center">
	<img src="https://github.com/InfinyTech3D/SofaUnity-Renderer/blob/main/Doc/img/interface_02.jpg" style="width:1000px;"/>
</p>

## License
In discussion



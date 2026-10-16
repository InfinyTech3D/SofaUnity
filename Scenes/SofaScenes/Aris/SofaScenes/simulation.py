from os import name
from tkinter import W
import Sofa
import SofaImGui
import Sofa.Gui

# General Parameters
transparency = 0.7

skin_params = {
    "0102_EpidermisPapillary" : {
        "FEM" : {
            "youngModulus": 9e6,
            "poissonRatio": 0.3,
            "method" : "large"
        },
        "Mass" : {
            "massDensity": 1e3
        },
        "NeedleInsertion" : {
            "punctureForceThreshold": 2e5,
            "tipDistThreshold": 1e-1,
            "punctureFrictionCoeff": 0.05,
            "insertionFrictionCoeff": 5e-5,
            "distanceFilter": 0.2
        },
        "color": [0.8, 0.7, 0.6, transparency],
    },
    "03_ReticularDermis" : {
        "FEM" : {
            "youngModulus": 2e7,
            "poissonRatio": 0.3,
            "method" : "large"
        },
        "Mass" : {
            "massDensity": 1e3
        },
        "NeedleInsertion" : {
            "punctureForceThreshold": 3e5,
            "tipDistThreshold": 1e-1,
            "punctureFrictionCoeff": 0.02,
            "insertionFrictionCoeff": 8e-5,
            "distanceFilter": 0.2
        },
        "color": [0.64, 0.5, 0.5, transparency],
        "Attachments": ["0102_EpidermisPapillary"]
    },
    "04_SubcutaneousFat" : {
        "FEM" : {
            "youngModulus": 5e6,
            "poissonRatio": 0.3,
            "method" : "large"
        },
        "Mass" : {
            "massDensity": 9e2
        },
        "NeedleInsertion" : {
            "punctureForceThreshold": 4e4,
            "tipDistThreshold": 1e-1,
            "punctureFrictionCoeff": 0.01,
            "insertionFrictionCoeff": 0,#6e-2,
            "distanceFilter": 0.1
        },
        "color": [0.7, 0.7, 0.1, transparency],
        "Attachments": ["03_ReticularDermis"]
    }
}

needle_params = {
    "nEdges": 50,
    "length": 1.7,
    "mass": 2e-11,
    "radiusVisual": 1e-2,
    "FEM": {
        "youngModulus": 5e20,
        "poissonRatio": 0.3,
        "radius": 3e-4
    }
}

def main():
    # Call the SOFA function to create the root node
    root_node = Sofa.Core.Node("root")
    
    # Call the createScene function, as runSofa does
    createScene(root_node)
    
    # Once defined, initialization of the scene graph
    Sofa.Simulation.initRoot(root_node)
    
    # Launch the GUI (imgui is now by default, to use Qt please refer to the example "basic-useQtGui.py")
    Sofa.Gui.GUIManager.Init("myscene", "imgui")
    Sofa.Gui.GUIManager.createGUI(root_node, __file__)
    Sofa.Gui.GUIManager.SetDimension(1080, 800)
    
    # Initialization of the scene will be done here
    Sofa.Gui.GUIManager.MainLoop(root_node)
    Sofa.Gui.GUIManager.closeGUI()

def createScene(root_node):
    root_node.gravity = [0.0, 0.0, 0.0]
    root_node.dt = 5e-3

    root_node.addObject("RequiredPlugin", pluginName=[
          "Sofa.Component.AnimationLoop"
        , "Sofa.Component.Constraint.Lagrangian.Solver"
        , "Sofa.Component.Constraint.Lagrangian.Correction"
        , "Sofa.Component.Engine.Select"
        , "Sofa.Component.Haptics"
        , "Sofa.Component.IO.Mesh"
        , "Sofa.Component.LinearSolver.Direct"
        , "Sofa.Component.LinearSolver.Iterative"
        , "Sofa.Component.LinearSolver.Preconditioner"
        , "Sofa.Component.LinearSystem"
        , "Sofa.Component.Mapping.Linear"
        , "Sofa.Component.Mapping.NonLinear"
        , "Sofa.Component.Mass"
        , "Sofa.Component.MechanicalLoad"
        , "Sofa.Component.ODESolver.Backward"
        , "Sofa.Component.Playback"
        , "Sofa.Component.StateContainer"
        , "Sofa.Component.SolidMechanics.FEM.Elastic"
        , "Sofa.Component.SolidMechanics.Spring"
        , "Sofa.Component.Topology.Container.Dynamic"
        , "Sofa.Component.Topology.Mapping"
        , "Sofa.Component.Visual"
        , "Sofa.GL.Component.Rendering3D"
        , "CollisionAlgorithm"
        , "ConstraintGeometry"
        , "NeedleInsertion"
        , "Geomagic"
    ])

    root_node.addObject("VisualStyle", displayFlags="showVisualModels hideForceFields")

    root_node.addObject("FreeMotionAnimationLoop")
    
    root_node.addObject("BlockGaussSeidelConstraintSolver", name="ConstraintSolver", tolerance=1e-6, maxIt=1000
                        , useSVDForRegularization=False)#, regularizationTerm=1e-7)#, regularizationTerm=1e-4)
    root_node.addObject("CollisionLoop")

    skin_node = {}
    skin_node["Parent"] = root_node.addChild("Skin")
    skin_node["Parent"].addObject("EulerImplicitSolver", name="odeSolver", rayleighMass=0.01, rayleighStiffness=0.01)

    # 1. PCG 
    skin_node["Parent"].addObject("PreconditionedMatrixFreeSystem", name="pcgLinearSystem", template="GraphScattered", assemblingRate=1)
    skin_node["Parent"].addObject("PCGLinearSolver", name="linearSolver", template="GraphScattered"
                        , tolerance=1e-4
                        , iterations=100, printLog=False
                        , linearSystem="@pcgLinearSystem"
                        #, preconditioner="@preconditioner"
                        #, use_precond=False
    )
    
    skin_node["Parent"].addObject("MatrixLinearSystem", name="ldlLinearSystem", template="CompressedRowSparseMatrixMat3x3d")
    skin_node["Parent"].addObject("AsyncSparseLDLSolver", name="preconditioner", template="CompressedRowSparseMatrixMat3x3d", linearSystem="@ldlLinearSystem")

    ## 2. CG
    #skin_node["Parent"].addObject("MatrixLinearSystem", name="cgLinearSystem", template="CompressedRowSparseMatrixMat3x3d")
    #skin_node["Parent"].addObject("CGLinearSolver", name="linearSolver", template="CompressedRowSparseMatrixMat3x3d"
    #                    , tolerance=1e-6
    #                    , threshold=1e-6
    #                    , iterations=200
    #                    , linearSystem="@cgLinearSystem"
    #)

    ## 3. SparseLDL
    #skin_node["Parent"].addObject("MatrixLinearSystem", name="ldlLinearSystem", template="CompressedRowSparseMatrixMat3x3d")
    #skin_node["Parent"].addObject("SparseLDLSolver", name="linearSolver", template="CompressedRowSparseMatrixMat3x3d", linearSystem="@ldlLinearSystem")

    for patch_name in skin_params:
        skin_node[patch_name] = add_patch(skin_node["Parent"], patch_name)

    #skin_node["01_Epidermis"].addObject("VisualStyle", displayFlags="showWireframe")

    add_needle(root_node)

    return root_node

def add_patch(parent_node, patch_name):
    patch_node = parent_node.addChild(patch_name)

    patch_node.addObject("MeshGmshLoader", name="mesh_loader", filename="mesh/"+patch_name+"_coarse.msh")
    patch_node.addObject("TetrahedronSetTopologyContainer", name="tet_container", position="@mesh_loader.position", tetrahedra="@mesh_loader.tetrahedra")
    patch_node.addObject("MechanicalObject", name="ms"
                         #, template="CudaVec3f"
                         , template="Vec3d"
                         , position="@tet_container.position")
    patch_node.addObject("DiagonalMass", name="mass", **skin_params[patch_name]["Mass"])
    patch_node.addObject("FastTetrahedralCorotationalForceField", name="FEM", **skin_params[patch_name]["FEM"])
    #patch_node.addObject("TetrahedronFEMForceField", name="FEM", **skin_params[patch_name]["FEM"])
    patch_node.addObject("TetrahedronGeometry", name="tet_geom", mstate="@ms", topology="@tet_container")

    collision_node = patch_node.addChild("Collision")
    #collision_node.addObject("MeshSTLLoader", name="obj_loader", filename="mesh/"+patch_name+"_coarse_collision.stl")
    #collision_node.addObject("TriangleSetTopologyContainer", name="tri_container", position="@obj_loader.position", triangles="@obj_loader.triangles")
    #collision_node.addObject("MechanicalObject", name="ms", position="@tri_container.position")
    #collision_node.addObject("BarycentricMapping", name="mapping", input="@../ms", output="@ms")
    collision_node.addObject("TriangleSetTopologyContainer", name="tri_container")#, position="@obj_loader.position", triangles="@obj_loader.triangles")
    collision_node.addObject("TriangleSetTopologyModifier", name="tri_modifier")
    collision_node.addObject("TriangleSetGeometryAlgorithms", name="tri_geom_algo")
    collision_node.addObject("Tetra2TriangleTopologicalMapping", name="tet2tri_mapping", input="@../tet_container", output="@tri_container")
    collision_node.addObject("MechanicalObject", name="ms", template="Vec3d", position="@tet_container.position")
    collision_node.addObject("IdentityMapping", name="mapping", input="@../ms", output="@ms")

    collision_node.addObject("TriangleGeometry", name="tri_geom", mstate="@ms", topology="@tri_container", draw=True)
    collision_node.addObject("PhongTriangleNormalHandler", name="tri_normals", geometry="@tri_geom")
    #collision_node.addObject("AABBBroadPhase", name="AABBTriangles", thread=1, nbox=[6, 6, 2])

    visual_node = patch_node.addChild("VisualModel")
    visual_node.addObject("MeshOBJLoader", name="obj_loader", filename="mesh/"+patch_name+"_visual.obj")
    visual_node.addObject("OglModel", name="ogl_model", position="@obj_loader.position", triangles="@obj_loader.triangles", color=skin_params[patch_name]["color"])
    visual_node.addObject("BarycentricMapping", name="mapping", input="@../ms", output="@ogl_model")

    #patch_node.addObject("LinearSolverConstraintCorrection", name="ConstraintCorrection", linearSolver="@../linearSolver", ODESolver="@../odeSolver")
    patch_node.addObject("PrecomputedConstraintCorrection", name="ConstraintCorrection"
                         , fileDir="compliance/", printLog=True)
    patch_node.addObject("BoxROI", name="FixedROI", drawROI=True, drawPoints=True, drawSize=5
        , box=[
              [-0.40, 0.2, -0.47, 0.80, 1.7, -0.4]
            , [-0.40, 0.2, -1.60, 0.80, 1.7, -1.53]
            , [ 0.72, 0.2, -1.60, 0.80, 1.7, -0.4]
            , [-0.40, 0.2, -1.60,-0.30, 1.7, -0.4]
    ])
    patch_node.addObject("RestShapeSpringsForceField", name="FixingSprings", points="@FixedROI.indices", stiffness=1e5, angularStiffness=1e5)

    if "Attachments" in skin_params[patch_name]:
        for parent_patch in skin_params[patch_name]["Attachments"]:
            if parent_patch in skin_params:
                parent_path = str("@" + parent_node.getPathName() + "/" + parent_patch)
                parent_msObj = str(parent_path + "/ms")
                parent_msIds = str(parent_path + "/ms.indices")

                patch_path = str("@" + patch_node.getPathName())
                patch_msObj = str(patch_path + "/ms")
                patch_msIds = str(patch_path + "/ms.indices")

                roi_name = "AttachROI_" + patch_name + "_to_" + parent_patch
                patch_node.addObject("NearestPointROI", name=roi_name, template="Vec3d"
                                     , object1 = parent_msObj
                                     , inputIndices1 = parent_msIds
                                     , object2 = patch_msObj
                                     , inputIndices2 = patch_msIds
                                     , radius = 1e-2
                )

                spring_name = str("AttachmentSprings_" + patch_name + "_to_" + parent_patch)
                patch_node.addObject("SpringForceField", name=spring_name
                                     , object1 = parent_msObj
                                     , springsIndices1 = str("@" + roi_name + ".indices1")
                                     , object2 = patch_msObj
                                     , springsIndices2 = str("@" + roi_name + ".indices2")
                                     , stiffness = 1e5
                                     , damping = 0
                                     , elongationOnly = 0
                                     , enabled = 1
                                     , lengths = str("@" + roi_name + ".distances")
                                     , drawMode = 2, showArrowSize = 3e-3
                )

    return patch_node

def add_needle(parent_node):
    # Needle Node Hierarchy
    needle_toolkit_node = parent_node.addChild("NeedleToolkit")

    tool_controller_node = needle_toolkit_node.addChild("ToolController")
    tool_controller_node.addObject("GeomagicDriver", name="GeomagicDevice",
            deviceName="Default Device", scale=1e-1, drawDeviceFrame=False, drawDevice=False,
            manualStart=False, positionBase=[-0.2, 1.5, -0.5], orientationBase=[0, 1, 0, 0]
    )
    tool_controller_node.addObject("MechanicalObject", name="ms", template="Rigid3d", showObject=False,
                                   #position="@readState.position"
                                   position="@GeomagicDevice.positionDevice"
    )
    #tool_controller_node.addObject("WriteState", name="readState", filename="ReadState_Manual.txt", period=5e-3)
    #tool_controller_node.addObject("ReadState", name="readState", filename="ReadState_Manual.txt")


    needle_node = needle_toolkit_node.addChild("Needle")
    needle_node.addObject("EulerImplicitSolver", name="odeSolver")
    needle_node.addObject("EigenSparseLU", name="linearSolver", template="CompressedRowSparseMatrixMat3x3d")
    needle_node.addObject("EdgeSetTopologyContainer", name="edge_container"
                          , position=[[0, 0, 1. - i * needle_params["length"]/needle_params["nEdges"]] for i in range(needle_params["nEdges"]+1)]
                          , edges=[[i, i+1] for i in range(needle_params["nEdges"])]
    )
    needle_node.addObject("MechanicalObject", name="ms", template="Rigid3d", showObject=False, showObjectScale=0.1)
    needle_node.addObject("UniformMass", name="mass", totalMass=needle_params["mass"], showAxisSizeFactor=1e-2)
    needle_node.addObject("BeamFEMForceField", name="FEM", **needle_params["FEM"])
    needle_node.addObject("LinearSolverConstraintCorrection", name="ConstraintCorrection", linearSolver="@linearSolver", ODESolver="@odeSolver")
    needle_node.addObject("RestShapeSpringsForceField", external_rest_shape="@../ToolController/ms", stiffness=3e7, angularStiffness=1e11, points=needle_params["nEdges"], external_points=0)

    needle_base_node = needle_node.addChild("Base")
    needle_base_node.addObject("PointSetTopologyContainer", name="point_container", position="@../ms.position")
    needle_base_node.addObject("MechanicalObject", name="ms", template="Rigid3d")
    needle_base_node.addObject("SubsetMapping", name="mapping", indices=needle_params["nEdges"])

    needle_shaft_node = needle_node.addChild("Shaft")
    needle_shaft_node.addObject("EdgeSetTopologyContainer", name="edge_container", position="@../edge_container.position", edges="@../edge_container.edges")
    needle_shaft_node.addObject("MechanicalObject", name="ms", template="Vec3d")
    needle_shaft_node.addObject("EdgeGeometry", name="shaft_geom", topology="@edge_container", color=[0.7, 0.7, 0.7, 1])
    needle_shaft_node.addObject("EdgeNormalHandler", name="edge_handler", geometry="@shaft_geom")
    needle_shaft_node.addObject("IdentityMapping", name="mapping") #localCoord=False)

    needle_tip_node = needle_node.addChild("Tip")
    needle_tip_node.addObject("PointSetTopologyContainer", name="point_container", position=[0, 0, 1 - needle_params["length"]])
    needle_tip_node.addObject("MechanicalObject", name="ms", template="Vec3d")
    needle_tip_node.addObject("PointGeometry", name="tip_geom", mstate="@ms")
    needle_tip_node.addObject("RigidMapping", name="mapping", globalToLocalCoords=True, index=needle_params["nEdges"])

    needle_visual_node = needle_node.addChild("Visual")
    needle_visual_node.addObject("MechanicalObject", name="ms", template="Vec3d")
    needle_visual_node.addObject("QuadSetTopologyContainer", name="quad_container")
    needle_visual_node.addObject("QuadSetTopologyModifier", name="quad_modifier")
    needle_visual_node.addObject("QuadSetGeometryAlgorithms", name="quad_geom")
    needle_visual_node.addObject("Edge2QuadTopologicalMapping", 
                                 nbPointsOnEachCircle=6, radius=needle_params["radiusVisual"],
                                 input="@../edge_container", output="@quad_container", flipNormals=True
    )
    needle_visual_node.addObject("TubularMapping", name="mapping", nbPointsOnEachCircle=6, radius=needle_params["radiusVisual"],
                                 input="@../ms", output="@ms", peak=-1)
    needle_ogl_node = needle_visual_node.addChild("OglNode")
    needle_ogl_node.addObject("OglModel", name="ogl_model", position="@../quad_container.position", color=[1, 1, 1])
    needle_ogl_node.addObject("IdentityMapping")

    needle_ff_node = needle_toolkit_node.addChild("ForceFeedback")
    needle_ff_node.addObject("MechanicalObject", name="ms", template="Rigid3d", position="@../Needle/Base/ms.position")
    needle_ff_node.addObject("LCPForceFeedback", name="lcp_ff", activate=True, forceCoef=3e-4)
    needle_ff_collision_node = needle_ff_node.addChild("Collision")
    needle_ff_collision_node.addObject("EdgeSetTopologyContainer", name="edge_container", position="@../../Needle/Shaft/edge_container.position", edges="@../../Needle/Shaft/edge_container.edges")
    needle_ff_collision_node.addObject("MechanicalObject", name="ms", constraint="@../../Needle/Shaft/ms.constraint")
    needle_ff_collision_node.addObject("RigidMapping", name="mapping")
    needle_ff_tip_node = needle_ff_node.addChild("Tip")
    needle_ff_tip_node.addObject("PointSetTopologyContainer", name="point_container", 
                                 position="@../../Needle/Tip/point_container.position")
    needle_ff_tip_node.addObject("MechanicalObject", name="ms", constraint="@../../Needle/Tip/ms.constraint")
    needle_ff_tip_node.addObject("RigidMapping", name="mapping")

    for patch_name in skin_params:
        needle_algo = needle_toolkit_node.addChild(patch_name+"_NeedleInsertionAlgorithm")
        needle_algo.addObject("VisualStyle", displayFlags="showCollisionModels showInteractionForceFields")
        needle_algo.addObject("NeedleInsertionAlgorithm", name="algorithm"
                                , tipGeom="@NeedleToolkit/Needle/Tip/tip_geom"
                                , shaftGeom="@NeedleToolkit/Needle/Shaft/shaft_geom"
                                , surfGeom="@/Skin/"+patch_name+"/Collision/tri_geom"
                                , volGeom="@/Skin/"+patch_name+"/tet_geom"
                                , punctureForceThreshold=skin_params[patch_name]["NeedleInsertion"]["punctureForceThreshold"]
                                , tipDistThreshold=skin_params[patch_name]["NeedleInsertion"]["tipDistThreshold"]
                                , enableShaftCollision=False
                                , drawcollision=False
        )
        needle_algo.addObject("DistanceFilter", algo="@algorithm", distance=skin_params[patch_name]["NeedleInsertion"]["distanceFilter"])
        needle_algo.addObject("SecondDirection", name="punctureDirection", handler="@/Skin/"+patch_name+"/Collision/tri_normals")
        needle_algo.addObject("ConstraintUnilateral", input="@algorithm.collisionOutput", directions="@punctureDirection", draw_scale=0.01
                              , mu=skin_params[patch_name]["NeedleInsertion"]["punctureFrictionCoeff"])
        needle_algo.addObject("FirstDirection", name="insertionDirection", handler="@NeedleToolkit/Needle/Shaft/edge_handler")
        needle_algo.addObject("ConstraintInsertion", input="@algorithm.insertionOutput", directions="@insertionDirection", draw_scale=0.01
                              , frictionCoeff=skin_params[patch_name]["NeedleInsertion"]["insertionFrictionCoeff"])

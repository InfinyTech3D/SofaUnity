# Required import for python
import Sofa


def main():
    import SofaRuntime
    import Sofa.Gui
    SofaRuntime.importPlugin("SofaOpenglVisual")
    SofaRuntime.importPlugin("SofaImplicitOdeSolver")

    root = Sofa.Core.Node("root")
    createScene(root)
    Sofa.Simulation.init(root)


def createScene(root):
    root.gravity=[0, -9.81, 0]
    root.dt=0.02

    root.addObject('DefaultVisualManagerLoop')
    root.addObject('DefaultAnimationLoop')

    root.addObject('VisualStyle', displayFlags="showCollisionModels")
    root.addObject('RequiredPlugin', pluginName="SofaImplicitOdeSolver SofaLoader SofaOpenglVisual SofaBoundaryCondition SofaGeneralLoader SofaGeneralSimpleFem")
    root.addObject('DefaultPipeline', name="CollisionPipeline")
    root.addObject('BruteForceBroadPhase', name="BroadPhase")
    root.addObject('BVHNarrowPhase', name="NarrowPhase")
    root.addObject('DefaultContactManager', name="CollisionResponse", response="default")
    root.addObject('DiscreteIntersection')

    root.addObject('MeshObjLoader', name="LiverSurface", filename="mesh/liver-smooth.obj")

    liver = root.addChild('Liver')
    liver.addObject('EulerImplicitSolver', name="cg_odesolver", rayleighStiffness="0.1", rayleighMass="0.1")
    liver.addObject('CGLinearSolver', name="linear_solver", iterations="25", tolerance="1e-09", threshold="1e-09")
    liver.addObject('MeshGmshLoader', name="meshLoader", filename="mesh/liver.msh")
    liver.addObject('TetrahedronSetTopologyContainer', name="topo", src="@meshLoader")
    liver.addObject('MechanicalObject', name="dofs", src="@meshLoader")
    liver.addObject('TetrahedronSetGeometryAlgorithms', template="Vec3d", name="GeomAlgo")
    liver.addObject('DiagonalMass', name="Mass", massDensity="1.0")
    liver.addObject('TetrahedralCorotationalFEMForceField', template="Vec3d", name="FEM", method="large", poissonRatio="0.3", youngModulus="3000", computeGlobalMatrix="0")
    liver.addObject('FixedConstraint', name="FixedConstraint", indices="3 39 64")

    visu = liver.addChild('Visu')
    visu.addObject('OglModel', name="VisualModel", src="@../../LiverSurface")
    visu.addObject('BarycentricMapping', name="VisualMapping", input="@../dofs", output="@VisualModel")

    surf = liver.addChild('Surf')
    surf.addObject('SphereLoader', name="sphereLoader", filename="mesh/liver.sph")
    surf.addObject('MechanicalObject', name="spheres", position="@sphereLoader.position")
    surf.addObject('SphereCollisionModel', name="CollisionModel", listRadius="@sphereLoader.listRadius")
    surf.addObject('BarycentricMapping', name="CollisionMapping", input="@../dofs", output="@spheres")

    return root


# Function used only if this script is called from a python environment
if __name__ == '__main__':
    main()

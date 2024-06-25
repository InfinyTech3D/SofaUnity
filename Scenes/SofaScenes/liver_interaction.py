# Required import for python
import Sofa


# Choose in your script to activate or not the GUI
USE_GUI = False


class MOController(Sofa.Core.Controller):
    def __init__(self, state, pressure):
         ## These are needed (and the normal way to override from a python class)
         #Sofa.Core.Controller.__init__(self, *args, **kwargs)
         super().__init__()
         #self.mechanical_object = kwargs.get("mechanical_object")
         self.state = state
         self.pressure = pressure
         
         self.inited = False
         self.center = [0, 0, 0]
         self.indices = []
         self.counter = 0

    def onAnimateBeginEvent(self, event):
        print("onAnimateBeginEvent")

        # Access the position of the particle 
        particles_mecha = self.state.position.value

        if (self.inited == False):
            for i in range(len(particles_mecha)): 
                self.center += particles_mecha[i]

            self.center /= particles_mecha.size
            print('self.center: ' + str(self.center))


            #self.pressure.pressure = self.center
            a=[0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
            #np.resize(a,(2,3))
            self.pressure.indices = a
            self.forces = [self.center, self.center, self.center, self.center, self.center, self.center, self.center, self.center, self.center, self.center]

            for i in range(len(self.pressure.indices)): 
                print('self.center: ' + str(self.center - particles_mecha[self.pressure.indices[i]]))
                self.forces[i] = (self.center - particles_mecha[self.pressure.indices[i]])

            self.pressure.forces = self.forces            
            self.inited = True
             
        print('====================================')
        print('State of the particle: ' + str(self.counter))
        print('====================================')
        
        
        if (self.counter == 200):
            self.counter = 0
        
        for i in range(len(self.pressure.indices)): 
            self.forces[i] = (self.center - particles_mecha[self.pressure.indices[i]]) * self.counter
         
        self.pressure.forces = self.forces
         
        self.counter += 1


def main():
    import SofaRuntime
    import Sofa.Gui

    root = Sofa.Core.Node("root")
    createScene(root)
    Sofa.Simulation.init(root)

    if not USE_GUI:
        for iteration in range(10):
            Sofa.Simulation.animate(root, root.dt.value)
    else:
        Sofa.Gui.GUIManager.Init("myscene", "qglviewer")
        Sofa.Gui.GUIManager.createGUI(root, __file__)
        Sofa.Gui.GUIManager.SetDimension(1080, 1080)
        Sofa.Gui.GUIManager.MainLoop(root)
        Sofa.Gui.GUIManager.closeGUI()


def createScene(root):
    root.gravity=[0, -9.81, 0]
    root.dt=0.02

    root.addObject("RequiredPlugin", pluginName=[    'Sofa.Component.Collision.Detection.Algorithm',
    'Sofa.Component.Collision.Detection.Intersection',
    'Sofa.Component.Collision.Geometry',
    'Sofa.Component.Collision.Response.Contact',
    'Sofa.Component.Constraint.Projective',
    'Sofa.Component.IO.Mesh',
    'Sofa.Component.LinearSolver.Iterative',
    'Sofa.Component.Mapping.Linear',
    'Sofa.Component.Mass',
    'Sofa.Component.ODESolver.Backward',
    'Sofa.Component.SolidMechanics.FEM.Elastic',
    'Sofa.Component.StateContainer',
    'Sofa.Component.Topology.Container.Dynamic',
    'Sofa.Component.Visual',
    'Sofa.GL.Component.Rendering3D'
    ])

    root.addObject('DefaultAnimationLoop')

    root.addObject('VisualStyle', displayFlags="hideCollisionModels")
    root.addObject('CollisionPipeline', name="CollisionPipeline")
    root.addObject('BruteForceBroadPhase', name="BroadPhase")
    root.addObject('BVHNarrowPhase', name="NarrowPhase")
    root.addObject('DefaultContactManager', name="CollisionResponse", response="PenalityContactForceField")
    root.addObject('DiscreteIntersection')

    root.addObject('MeshOBJLoader', name="LiverSurface", filename="mesh/liver-smooth.obj")

    liver = root.addChild('Liver')
    liver.addObject('EulerImplicitSolver', name="cg_odesolver", rayleighStiffness="0.1", rayleighMass="0.1")
    liver.addObject('CGLinearSolver', name="linear_solver", iterations="25", tolerance="1e-09", threshold="1e-09")
    liver.addObject('MeshGmshLoader', name="meshLoader", filename="mesh/liver.msh")
    liver.addObject('TetrahedronSetTopologyContainer', name="topo", src="@meshLoader")
    MO = liver.addObject('MechanicalObject', name="dofs", src="@meshLoader")
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
    
    ff = surf.addObject('ConstantForceField', forces=[100, 0, 0])
    surf.addObject(MOController(state=MO, pressure = ff))

    return root


# Function used only if this script is called from a python environment
if __name__ == '__main__':
    main()

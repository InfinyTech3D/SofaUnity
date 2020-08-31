import Sofa

import SofaPython.Tools

def createScene(rootNode):
	# rootNode
	rootNode.findData('dt').value = 0.02
	rootNode.findData('gravity').value=[0.0, -9.81, 0.0]

	rootNode.createObject('DefaultPipeline', name='CollisionPipeline', verbose=0)
	rootNode.createObject('BruteForceDetection', name='N2')
	rootNode.createObject('DiscreteIntersection', name='Intersection')
	rootNode.createObject('DefaultContactManager', name='Response', response='distanceLMConstraint')
   
	# Liver node
	livNode = rootNode.createChild('Liver')

	livNode.createObject('EulerImplicitSolver',name='cg_odesolver',printLog='false',rayleighStiffness='0.1', rayleighMass='0.1')
	livNode.createObject('CGLinearSolver',name='linear solver',iterations='15',tolerance='1.0e-5',threshold='1.0e-5')

	livNode.createObject('MeshGmshLoader', name='meshLoader', filename='mesh/liver.msh')	
	livNode.createObject('MechanicalObject', name='dofs', src='@meshLoader')
	
	livNode.createObject('TetrahedronSetTopologyContainer', name='topo' ,src='@meshLoader')
	livNode.createObject('TetrahedronSetGeometryAlgorithms', name='GeomAlgo', template='Vec3d')
	
	livNode.createObject('DiagonalMass', name='computed using mass density', massDensity='1')
	livNode.createObject('TetrahedralCorotationalFEMForceField', name='FEM', template='Vec3d', method='large', poissonRatio='0.3', youngModulus='3000', computeGlobalMatrix='0')
	livNode.createObject('FixedConstraint', name='FixedConstraint', indices='3 39 64')
	
	# visu node
	visuNode = livNode.createChild('Visu')
	visuNode.findData('tags').value = 'Visual'
	meshLoader = SofaPython.Tools.meshLoader(visuNode, 'mesh/liver-smooth.obj')
	visuNode.createObject('OglModel', name='visual', template='Vec3d', src='@'+meshLoader.name)
	visuNode.createObject('BarycentricMapping', input="@../dofs",output='@visual')
	
	# collision node
	collNode = livNode.createChild('Surf')
	collNode.createObject('SphereLoader', name='sphereMesh', filename='mesh/liver.sph')
	collNode.createObject('MechanicalObject', name='spheres', position='@sphereMesh.position')
	collNode.createObject('SphereCollisionModel', name='CollisionModel', listRadius='@sphereMesh.listRadius')
	collNode.createObject('BarycentricMapping',name='sphere mapping', input='@../dofs',output='@spheres')
   
	return rootNode

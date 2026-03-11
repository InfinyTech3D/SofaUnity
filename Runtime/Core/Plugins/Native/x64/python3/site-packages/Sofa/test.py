import Sofa
import jedi
import sys

if sys.argv[1] == '1':
        print("RUN 1:", jedi.Script("import Sofa; Sofa.").completions())
elif sys.argv[1] == '2':
        print("RUN 2:", jedi.Script("import Sofa; Sofa.Simulation.Node.").completions())
elif sys.argv[1] == '3':
        print("RUN 3:", jedi.Script("import Sofa; c = Sofa.Simulation.Node('damien'); c.").completions())
elif sys.argv[1] == '4':
        print("RUN 4:", jedi.Script("""import Sofa 
                              c = Sofa.Simulation.Node('damien') # type: Sofa.Simulation.Node
                              c.""").completions())
elif sys.argv[1] == '5':
        print("RUN 5:", jedi.Script("""import Sofa 
                              c = Sofa.Simulation.Node('damien') # type: Sofa.Simulation.Node
                              c2 = c.addChild('Damien') 
                              c2.""").completions())
                              
   

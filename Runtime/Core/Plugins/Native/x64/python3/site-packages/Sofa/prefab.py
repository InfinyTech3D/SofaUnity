"""
Prefabs are python templates for a SOFA scene
"""

import Sofa.Core
import inspect
import os


class Prefab(Sofa.Core.RawPrefab):

    def __init__(self, *args, **kwargs):
        """
        Prefabs are python templates for a SOFA scene

        They allow to simplify the design of a simulation by using pre-structured and reusable python script.
        Inherit from this class to create your own Prefab. What makes Prefab special is that they
        have a set of special data named prefabParameters. When any of prefabParameter is changed the prefab
        is completely recreated by calling the onParameterChanged method so the scene graph is always kept synchronized
        with the parameter's content.

        To specify the prefabParameters, it is possible to provide in the class a list of dictionaries containing the 3 required fields ("name", "type", "help")
        and one optional field ("default").

        The same syntax can be used to also add prefab's data.

        Example:
          .. code-block:: python

            import Sofa.Core

            class Foo(Sofa.Core.Prefab):
                prefabParameters = [{ 'name': 'n', 'type': 'int', 'help': 'number of repetition, 'default': 1},
                                    {'name': 'message', 'type': 'string', 'help': 'message to display', 'default': ''}]

                myAttribute = 0

                def __init__(self, *a, *k):
                    Sofa.Core.Prefab.__init__(self, *a, **k)

                def init(self):
                    myAttribute += 1
                    for i in range(0, self.n.value):
                        print(self.message.value)

            n = Sofa.Core.Node()
            n.addChild(Foo(name="aFooPrefab", n=42, message="hello universe!"))

        Prefab has protected the following additional keywords:
            - "name" = the name of the prefab instance
            - "parent" and "parents" = can't be used together, they set the context of the prefab, thus allowing paths resolution for Prefab parameters whose arguments are passed as link paths (strings). parents (with an '-s') sets multi-node contexts
        """

        Sofa.Core.RawPrefab.__init__(self, *args, **kwargs)
        frame = inspect.currentframe().f_back
        frameinfo = inspect.getframeinfo(frame)
        definedloc = (os.path.abspath(frameinfo.filename), frameinfo.lineno)

        self.setDefinitionSourceFileName(definedloc[0])
        self.setDefinitionSourceFilePos(definedloc[1])
        self.setSourceTracking(definedloc[0])

        frame = frame.f_back
        if frame is not None:
            frameinfo = inspect.getframeinfo(frame)
            definedloc = (os.path.abspath(frameinfo.filename), frameinfo.lineno)
            self.setInstanciationSourceFileName(definedloc[0])
            self.setInstanciationSourceFilePos(definedloc[1])

        if "name" not in kwargs:
            self.setName(str(self.__class__.__name__))
        self.addData("prefabname", value=type(self).__name__, type="string", group="Infos", help="Name of the prefab")

        # A prefab should be added to its parent explicitely by calling parent.addChild() with this prefab.
        # However if for some reason you need to pass a context to your prefab, use the "parents" keyword argument to pass its context
        if "parent" in kwargs and "parents" not in kwargs:
            if isinstance(kwargs['parent'], Sofa.Core.Node):
                kwargs['parent'].addChild(self)
            else:
                Sofa.Helper.msg_error(self, "'parent' is a protected keyword on Sofa.Core.Prefab and should only be used to provide the Prefab's context node - if needed for its instantation")
        elif "parents" in kwargs and "parent" not in kwargs:
            if isinstance(kwargs["parents"], list):
                for parent in kwargs['parents']:
                    if isinstance(parent, Sofa.Core.Node):
                        parent.addChild(self)
                    else:
                        Sofa.Helper.msg_error(self, "'parents' is an optional protected keyword on Sofa.Core.Prefab and must contain the list of parents of the prefab - if needed for its instantiation")
            else:
                Sofa.Helper.msg_error(self, "'parents' is a protected keyword on Sofa.Core.Prefab and should contain the list of parents for the prefab - if needed for its instantiation.")
        else:
            if "parent" in kwargs and "parents" in kwargs:
                Sofa.Helper.msg_error(self, "Cannot use both 'parent' and 'parents' keywords on a prefab. Use 'parent' to set the context of your prefab, 'parents' in the case of a multi-parent prefab")
        
        # Prefab parameters are defined in a list of dictionaries named "prefabParameters".
        # The dictionaries has 3 required fields (name, type, help) and an additional optional field "default"
        docstring = ""

        if hasattr(self, "prefabParameters"):
            docstring += "Prefab's parameters:"
            for p in self.prefabParameters:
                self.addPrefabParameter(name=p['name'],
                                        type=p['type'],
                                        help=p['help'],
                                        default=kwargs.get(p['name'], p.get('default', None)))
                docstring += "\n:param " + p['name'] + ": " + p['help'] + ", defaults to " + str(p.get('default', '')) + '\n:type ' + p['name'] + ": " + p['type'] + "\n\n"

        if hasattr(self, "prefabData"):
            docstring += "Prefab's data:"
            for p in self.prefabData:
                self.addData(name=p['name'], type=p['type'], help=p['help'],
                             value=kwargs.get(p['name'], p.get('default', None)),
                             default=p['default'],
                             group=p.get('group', 'Property'))
                docstring += "\n:param " + p['name'] + ": " + p['help'] + ", defaults to " + str(p.get('default', '')) + '\n:type ' + p['name'] + ": " + p['type'] + "\n\n"

        if hasattr(self, "properties"):
            Sofa.Helper.msg_deprecated(self, "'properties' has been replaced with 'prefabParameters'. Please update your code.")
            docstring += "Prefab's (properties):"
            for p in self.properties:
                self.addPrefabParameter(name=p['name'], type=p['type'], help=p['help'], default=kwargs.get(p['name'], p.get('default', None)))
                docstring += "\n:param " + p['name'] + ": " + p['help'] + ", defaults to " + str(p.get('default', '')) + '\n:type ' + p['name'] + ": " + p['type'] + "\n\n"

        self.addData("docstring", value=('' if self.__doc__ is None else self.__doc__) + docstring, type="string", group="Infos", help="Documentation of the prefab")

        # calls the init() method in the prefab, then do node init & traversal.
        self.init()

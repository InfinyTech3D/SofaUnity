"""
Constant library for an easier keyboard interaction.


Example:
The following scene creates a Controller that will print a custom message in the command line when a key is pressed,
using constants from Sofa.constants.Key

    .. code-block:: python

        import Sofa.Core
        from Sofa.constants import *

        functionDict = {
                # Arrows
                Key.leftarrow : lambda:print("You pressed the left arrow"),
                Key.rightarrow : lambda:print("You pressed the right arrow"),
                Key.uparrow : lambda:print("You pressed the up arrow"),
                Key.downarrow : lambda:print("You pressed the down arrow"),
                # Special caracters
                Key.space: lambda:print("You pressed the space"),
                Key.plus: lambda:print("You pressed the plus"),
                Key.minus: lambda:print("You pressed the minux"),
                # Letters
                Key.I: lambda:print("You pressed the letter I"),
                # KeyPad
                Key.KP_0: lambda:print("You pressed the number 0 on the keypad"),
                Key.KP_1: lambda:print("You pressed the number 1 on the keypad"),
                Key.KP_2: lambda:print("You pressed the number 2 on the keypad"),
                Key.KP_3: lambda:print("You pressed the number 3 on the keypad"),
                Key.KP_4: lambda:print("You pressed the number 4 on the keypad"),
                Key.KP_5: lambda:print("You pressed the number 5 on the keypad"),
                Key.KP_6: lambda:print("You pressed the number 6 on the keypad"),
                Key.KP_7: lambda:print("You pressed the number 7 on the keypad"),
                Key.KP_8: lambda:print("You pressed the number 8 on the keypad"),
                Key.KP_9: lambda:print("You pressed the number 9 on the keypad")
                        }


        class KeyPressedController(Sofa.Core.Controller):
            #This controller monitors key movements.
            #    Press ctrl and a key to test it!

            def __init__(self, *args, **kwargs):
                Sofa.Core.Controller.__init__(self, *args, **kwargs)
                self.listening = True
                self.name = "keyPressedController"

            def onKeypressedEvent(self, event):
                if event['key'] in functionDict:
                    functionDict[event['key']]()
                else:
                    print("You pressed the key : " + event['key'])

            def onKeyreleasedEvent(self, event):
                print("You released a key!")

            def createScene(rootNode):
                rootNode.addObject(KeyPressedController(name = "MyController"))
                return rootNode

"""

__all__=["KeyCode", "Key"]

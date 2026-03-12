"""Control of the application runtime

   Example:
        .. code-block:: python

            import SofaRuntime
            SofaRuntime.importPlugin("Sofa.Component.LinearSolver")
"""

from SofaRuntime.SofaRuntime import *

import Sofa

import sys
import os
import inspect
import traceback
import importlib

###############################################################################
###################### MODULES INPORTS & UNLOAD FEATURES ######################
###############################################################################


try:
    import numpy
except ModuleNotFoundError as error:
    Sofa.msg_error("SofaRuntime",str(error))
    Sofa.msg_error("SofaRuntime", 'numpy is mandatory for SofaPython3')
    sys.exit(1)

# Keep a list of the modules always imported in the Sofa-PythonEnvironment
try:
    __SofaPythonEnvironment_importedModules__
except:
    __SofaPythonEnvironment_importedModules__ = sys.modules.copy()

    # some modules could be added here manually and can be modified procedurally
    # e.g. plugin's modules defined from c++
    __SofaPythonEnvironment_modulesExcludedFromReload = []

def unloadModules():
    """ Call this function to unload python modules and to force their reload

        (useful to take into account their eventual modifications since
        their last import).
    """
    global __SofaPythonEnvironment_importedModules__
    toremove = [name for name in sys.modules if not name in __SofaPythonEnvironment_importedModules__ and not name in __SofaPythonEnvironment_modulesExcludedFromReload ]
    for name in toremove:
        del(sys.modules[name]) # unload it

################################################################
###################### CALLSTACK FEATURES ######################
################################################################

def formatStackForSofa(o):
    """ Format the stack trace provided as parameter

        The parameter is converted into a string like that

        .. code-block:: text

            in filename.py:10:functioname()
                -> the line of code.
            in filename2.py:101:functioname1()
                -> the line of code.
            in filename3.py:103:functioname2()
                -> the line of code.

    """
    ss='Python Stack: \n'
    for entry in o:
        ss+= ' in ' + str(entry[1]) + ':' + str(entry[2]) + ':'+ entry[3] + '()  \n'
        ss+= '  -> '+ entry[4][0] + '  \n'
        return ss


def getStackForSofa():
    """Returns the current stack with a "informal" formatting """
    ## we exclude the first level in the stack because it is the getStackForSofa() function itself.
    ss=inspect.stack()[1:]
    return formatStackForSofa(ss)


def getPythonCallingPointAsString():
    """Returns the last entry with an "informal" formatting """

    ## we exclude the first level in the stack because it is the getStackForSofa() function itself.
    ss=inspect.stack()[-1:]
    return formatStackForSofa(ss)


def getPythonCallingPoint():
    """Returns the tupe with closest filename and line """
    ## we exclude the first level in the stack because it is the getStackForSofa() function itself.
    ss=inspect.stack()[1]
    tmp=(os.path.abspath(ss[1]), ss[2])
    return tmp


#############################################################################
###################### EXCEPTION HANDLING (NECESSARY?) ######################
#############################################################################

def getSofaFormattedStringFromException(e):
    """Function handling exception using `sofaFormatHandler()` (python stack)"""
    exc_type, exc_value, exc_tb = sys.exc_info()
    return sofaFormatHandler(exc_type, exc_value, exc_tb)

def sofaFormatHandler(type, value, tb):
    """This exception handler forwards python exceptions & traceback

       Example of formatting:

            .. code-block:: text

                Python Stack (most recent are at the end)
                File file1.py line 4  ...
                File file1.py line 10 ...
                File file1.py line 40 ...
                File file1.py line 23 ...

    """
    global oldexcepthook

    s="\nPython Stack (most recent are at the end): \n"
    for line in traceback.format_tb(tb):
        s += line

    return repr(value)+" "+s


def sendMessageFromException(e):
    """Function handling exception using `sofaExceptHandler()` (SOFA format)"""
    exc_type, exc_value, exc_tb = sys.exc_info()
    sofaExceptHandler(exc_type, exc_value, exc_tb)


def sofaExceptHandler(type, value, tb):
    """This exception handler converts python exceptions & traceback into classical SOFA error messages

       Message:

       .. code-block:: text

            Python Stack (most recent are at the end)
            File file1.py line 4  ...
            File file1.py line 10 ...
            File file1.py line 40 ...
            File file1.py line 23 ...

    """
    global oldexcepthook

    h = type.__name__

    if str(value) != '':
        h += ': ' + str(value)

    s = "Traceback (most recent call last):\n"
    s += ''.join(traceback.format_tb(tb))
    
    Sofa.msg_error("SofaRuntime", h + '\n' + s)

sys.excepthook=sofaExceptHandler

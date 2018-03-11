using System;
using System.Collections.Generic;
using System.Text;

namespace LayoutLib
{
    class ArgumentOutOfRangeException : System.ArgumentOutOfRangeException
    {
        public ArgumentOutOfRangeException(int param, string paramName, int min, int max)
            :
            base($"Invalid argument: {paramName} = ({param}). Value must be within {min} and {max}.")
        {
        }
    }
}

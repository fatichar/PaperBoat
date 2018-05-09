namespace Layout15
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

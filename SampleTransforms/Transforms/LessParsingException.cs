using System;

namespace SampleTransforms.Transforms
{
    public class LessParsingException : Exception
    {
        public LessParsingException(string message):base(message)
        {
        }
    }
}
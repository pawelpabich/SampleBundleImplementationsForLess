using dotless.Core.Loggers;

namespace SampleTransforms.Transforms.DotLess
{
    public class ExceptionLogger : Logger
    {
        public ExceptionLogger() : base(LogLevel.Error) { }
        protected override void Log(string message)
        {
            throw new LessParsingException(message);
        }
    }
}
using System;

namespace Harness
{
    public class HarnessBaseException : Exception
    {
        public HarnessBaseException(string message) : base(message)
        {
        }

        public HarnessBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
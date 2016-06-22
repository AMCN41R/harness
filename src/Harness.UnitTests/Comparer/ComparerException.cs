using System;

namespace Harness.UnitTests.Comparer
{
    public class ComparerException : Exception
    {
        public ComparerException()
        {
        }

        public ComparerException(string message) : base(message)
        {
        }

        public ComparerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

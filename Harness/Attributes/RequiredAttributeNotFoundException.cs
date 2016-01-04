using System;

namespace Harness.Attributes
{
    public class RequiredAttributeNotFoundException : Exception
    {
        public RequiredAttributeNotFoundException()
        {
        }

        public RequiredAttributeNotFoundException(string message)
            : base(message)
        {
        }

        public RequiredAttributeNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }

}

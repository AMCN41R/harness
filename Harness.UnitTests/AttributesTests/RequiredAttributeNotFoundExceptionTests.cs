using System;
using Xunit;
using Harness.Attributes;

namespace Harness.UnitTests.AttributesTests
{
    public class RequiredAttributeNotFoundExceptionTests
    {
        [Fact]
        public void New_Message_BaseMessageIsSet()
        {
            var ex = Assert.Throws<RequiredAttributeNotFoundException>(
                () => ExceptionTestClass.ThrowWithMessage());

            Assert.Equal("TestMessage", ex.Message);

        }

        [Fact]
        public void New_MessageAndInnerException_BaseParametersAreSet()
        {
            var ex = Assert.Throws<RequiredAttributeNotFoundException>(
                () => ExceptionTestClass.ThrowWithMessageAndInnerException());

            Assert.Equal("TestMessage", ex.Message);
            Assert.Equal("InnerMessage", ex.InnerException.Message);

        }

        private static class ExceptionTestClass
        {
            public static void ThrowWithMessage()
            {
                throw new RequiredAttributeNotFoundException(
                    "TestMessage");
            }

            public static void ThrowWithMessageAndInnerException()
            {
                throw new RequiredAttributeNotFoundException(
                    "TestMessage",
                    new Exception("InnerMessage"));
            }

        }
    }
}

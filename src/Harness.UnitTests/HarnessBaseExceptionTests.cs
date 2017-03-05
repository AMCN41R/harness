using System;
using Xunit;

namespace Harness.UnitTests
{
    public class HarnessBaseExceptionTests
    {
        [Fact]
        public void New_Message_BaseMessageIsSet()
        {
            var ex = Assert.Throws<HarnessBaseException>(
                () => ExceptionTestClass.ThrowWithMessage());

            Assert.Equal("TestMessage", ex.Message);

        }

        [Fact]
        public void New_MessageAndInnerException_BaseParametersAreSet()
        {
            var ex = Assert.Throws<HarnessBaseException>(
                () => ExceptionTestClass.ThrowWithMessageAndInnerException());

            Assert.Equal("TestMessage", ex.Message);
            Assert.Equal("InnerMessage", ex.InnerException?.Message);

        }

        private static class ExceptionTestClass
        {
            public static void ThrowWithMessage()
            {
                throw new HarnessBaseException(
                    "TestMessage");
            }

            public static void ThrowWithMessageAndInnerException()
            {
                throw new HarnessBaseException(
                    "TestMessage",
                    new Exception("InnerMessage"));
            }

        }
    }
}

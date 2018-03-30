using System;
using Harness.Settings;
using Xunit;

namespace Harness.Tests.Unit.SettingsTests
{
    public class SettingsLoaderExceptionTests
    {
        [Fact]
        public void New_Message_BaseMessageIsSet()
        {
            var ex = Assert.Throws<SettingsLoaderException>(
                () => ExceptionTestClass.ThrowWithMessage());

            Assert.Equal("TestMessage", ex.Message);

        }

        [Fact]
        public void New_MessageAndInnerException_BaseParametersAreSet()
        {
            var ex = Assert.Throws<SettingsLoaderException>(
                () => ExceptionTestClass.ThrowWithMessageAndInnerException());

            Assert.Equal("TestMessage", ex.Message);
            Assert.Equal("InnerMessage", ex.InnerException?.Message);

        }

        private static class ExceptionTestClass
        {
            public static void ThrowWithMessage()
            {
                throw new SettingsLoaderException(
                    "TestMessage");
            }

            public static void ThrowWithMessageAndInnerException()
            {
                throw new SettingsLoaderException(
                    "TestMessage",
                    new Exception("InnerMessage"));
            }

        }
    }
}

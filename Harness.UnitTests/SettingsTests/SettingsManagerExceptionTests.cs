using System;
using Harness.Settings;
using Xunit;

namespace Harness.UnitTests.SettingsTests
{
    class SettingsManagerExceptionTests
    {
        [Fact]
        public void New_Message_BaseMessageIsSet()
        {
            var ex = Assert.Throws<SettingsManagerException>(
                () => ExceptionTestClass.ThrowWithMessage());

            Assert.Equal("TestMessage", ex.Message);

        }

        [Fact]
        public void New_MessageAndInnerExceptio_BaseParametersAreSet()
        {
            var ex = Assert.Throws<SettingsManagerException>(
                () => ExceptionTestClass.ThrowWithMessageAndInnerException());

            Assert.Equal("TestMessage", ex.Message);
            Assert.Equal("InnerMessage", ex.InnerException.Message);

        }

        private static class ExceptionTestClass
        {
            public static void ThrowWithMessage()
            {
                throw new SettingsManagerException(
                    "TestMessage");
            }

            public static void ThrowWithMessageAndInnerException()
            {
                throw new SettingsManagerException(
                    "TestMessage",
                    new Exception("InnerMessage"));
            }

        }
    }
}

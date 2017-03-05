using System;

namespace Harness.Settings
{
    public class SettingsLoaderException : Exception
    {
        public SettingsLoaderException(string message) : base(message)
        {
        }

        public SettingsLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
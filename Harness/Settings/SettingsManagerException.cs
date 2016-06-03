using System;

namespace Harness.Settings
{
    public class SettingsManagerException : Exception
    {
        public SettingsManagerException(string message) : base(message)
        {
        }

        public SettingsManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
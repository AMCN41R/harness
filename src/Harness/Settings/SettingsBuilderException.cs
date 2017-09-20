using System;

namespace Harness.Settings
{
    /// <summary>
    /// Represents errors thrown by the <see cref="SettingsBuilder"/> class and its <see cref="SettingsBuilderException">extensions</see>.
    /// </summary>
    public class SettingsBuilderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsBuilderException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SettingsBuilderException(string message) : base(message)
        {
        }
    }
}
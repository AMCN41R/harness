using System;

namespace Harness.Settings
{
    /// <summary>
    /// Represents exceptions thrown by the <see cref="ISettingsLoader"/>.
    /// </summary>
    public class SettingsLoaderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsLoaderException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SettingsLoaderException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsLoaderException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public SettingsLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
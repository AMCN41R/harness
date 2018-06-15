namespace Harness.CLI
{
    using System;

    /// <summary>
    /// The Options Exception.
    /// </summary>
    internal class OptionsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsException"/>
        /// class with a specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public OptionsException(string message)
            : base(message)
        {
        }
    }
}

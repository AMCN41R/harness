namespace Harness
{
    internal class ValidationResult
    {
        /// <summary>
        /// Gets or sets a value indicating if the operation is valid or not.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string Message { get; set; }
    }
}
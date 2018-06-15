namespace Harness.CLI.Input
{
    internal static class CommonValidators
    {
        public static class String
        {
            public static InputValidationResult IsNotNullEmptyOrWhitespace(string value)
                => string.IsNullOrWhiteSpace(value)
                    ? new InputValidationResult("Value cannot be null, empty or whitespace.")
                    : new InputValidationResult();
        }
    }
}

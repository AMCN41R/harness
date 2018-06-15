namespace Harness.CLI.Input
{
    internal class InputValidationResult
    {
        public InputValidationResult()
        {
            this.IsValid = true;
            this.Error = null;
        }

        public InputValidationResult(string error)
        {
            this.IsValid = false;
            this.Error = error;
        }

        public bool IsValid { get; }

        public string Error { get; }
    }
}

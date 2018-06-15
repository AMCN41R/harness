namespace Harness.CLI.Input
{
    using System;

    internal class UserInputStep
    {
        public UserInputStep(string question, Action<string> success)
            : this(question, null, success)
        {
        }

        public UserInputStep(
            string question,
            Func<string, InputValidationResult> validate,
            Action<string> success)
        {
            this.Question =
                question.Trim().EndsWith(":")
                    ? $"{question.Trim()} "
                    : $"{question.Trim()}: ";

            this.Validate = validate;
            this.Success = success;
        }

        private string Question { get; }

        private Func<string, InputValidationResult> Validate { get; }

        private Action<string> Success { get; }

        public void Execute()
        {
            while (true)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.Write(this.Question);

                Console.ForegroundColor = color;

                var input = Console.ReadLine();

                if (this.Validate == null)
                {
                    this.Success(input);
                    return;
                }

                var validationResult = this.Validate(input);

                if (validationResult.IsValid)
                {
                    this.Success(input);
                    return;
                }

                Console.WriteLine(validationResult.Error);
                Console.WriteLine();
            }
        }
    }
}

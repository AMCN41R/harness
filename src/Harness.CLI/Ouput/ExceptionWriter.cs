namespace Harness.CLI.Ouput
{
    using System;

    /// <summary>
    /// Implementation of <see cref="IConsoleWriter{T}"/> that writes an
    /// exception to the console.
    /// </summary>
    internal class ExceptionWriter : IConsoleWriter<Exception>
    {
        private static string INDENT => "  ";

        /// <summary>
        /// Writes an exception to the console in a nice format, outputting the
        /// exception type, message and stack trace.
        /// </summary>
        /// <param name="item">The exception that will be written to the console.</param>
        public void Write(Exception item)
        {
            this.WriteTitle();

            this.WriteDetail("Type", item.GetType().Name);
            this.WriteDetail("Message", item.Message);

            this.WriteStackTrace(item);

            Console.ResetColor();
            Console.WriteLine();
        }

        private void WriteTitle()
        {
            var initialBackgroundColor = Console.BackgroundColor;

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  An exception was thrown  ");
            Console.BackgroundColor = initialBackgroundColor;
            Console.WriteLine();
        }

        private void WriteDetail(string key, string value)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{INDENT}{key}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{value}");
            Console.WriteLine();
        }

        private void WriteStackTrace(Exception item)
        {
            var trace = new System.Diagnostics.StackTrace(item, true);
            var frames = trace.GetFrames();

            this.WriteDetail("Stack Trace", string.Empty);
            for (var i = 0; i < trace.FrameCount; i++)
            {
                this.WriteDetail($"{INDENT}[{i}]", $"{frames[i]}".Trim());
            }
        }
    }
}
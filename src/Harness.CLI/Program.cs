namespace Harness.CLI
{
    using System;
    using System.Diagnostics;

    using CommandLine;

    using Harness.CLI.Options;
    using Harness.CLI.Ouput;

    /// <summary>
    /// The application.
    /// </summary>
    public class Program
    {
        private static ExceptionWriter ExceptionWriter { get; } = new ExceptionWriter();

        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            var exitCode = 0;

            var parsed =
                Parser
                    .Default
                    .ParseArguments<RunOptions, CreateOptions>(args);

            try
            {
                exitCode =
                    parsed.MapResult(
                            (RunOptions opts) => (opts as IProcess).Process(),
                            (CreateOptions opts) => (opts as IProcess).Process(),
                            error => 1
                        );
            }
            catch (OptionsException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(parsed, null, null).ToString());
                exitCode = 1;
            }
            catch (Exception ex)
            {
                ExceptionWriter.Write(ex);
                exitCode = 1;
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine($"Exit Code: {exitCode}");
                Console.ReadKey();
            }

            Environment.Exit(exitCode);
        }
    }
}

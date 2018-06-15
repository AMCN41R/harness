namespace Harness.CLI.Options
{
    using System;
    using System.IO;

    using CommandLine;

    /// <summary>
    /// The command line options for the 'Run' action.
    /// </summary>
    [Verb("run", HelpText = "Run harness against a config.")]
    internal class RunOptions : IProcess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunOptions"/> class.
        /// </summary>
        /// <param name="configPath">The path to the Harness config file.</param>
        public RunOptions(string configPath)
        {
            this.ConfigPath = configPath;
        }

        /// <summary>
        /// Gets the path to the Harness config file.
        /// </summary>
        [Value(0, Required = true, HelpText = "Path to the Harness config file.")]
        public string ConfigPath { get; }

        /// <inheritdoc />
        public int Process()
        {
            var path = this.ConfigPath;

            if (path.IsNullOrWhitespace() || Path.GetExtension(path) != ".json")
            {
                throw new OptionsException("Argument must be a valid string with a '.json' extension.");
            }

            if (!File.Exists(path))
            {
                throw new OptionsException("File not found.");
            }

            Console.WriteLine("Starting Harness...");
            var manager = new HarnessManager();

            Console.WriteLine("Building...");
            manager.UsingSettings(path).Build();

            Console.WriteLine("DONE");

            return 0;
        }
    }
}

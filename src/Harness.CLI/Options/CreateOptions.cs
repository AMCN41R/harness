namespace Harness.CLI.Options
{
    using System;
    using System.Collections.Generic;

    using CommandLine;

    using Harness.CLI.Input;

    using Newtonsoft.Json;

    /// <summary>
    /// The command line options for the 'Create' action.
    /// </summary>
    [Verb("create")]
    internal class CreateOptions : IProcess
    {
        public int Process()
        {
            var filename = "harness.json";

            string databaseName = null;
            string connectionString = null;
            bool dropFirst;

            var steps = new List<UserInputStep>
            {
                new UserInputStep(
                    "Enter file name (harness):",
                    str =>
                    {
                        var name = str.Trim();

                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            filename = name.EndsWith(".json")
                                ? name
                                : $"{name}.json";
                        }
                    }
                ),
                new UserInputStep(
                    "Enter database name:",
                    CommonValidators.String.IsNotNullEmptyOrWhitespace,
                    str => databaseName = str
                ),
                new UserInputStep(
                    "Enter connection string (mongodb://localhost:27017):",
                    SetOrDefault(str => connectionString = str, "mongodb://localhost:27017")
                ),
                new UserInputStep(
                    "Drop database first? (yes):",
                    str => dropFirst = IsOk(str)
                ),
            };

            this.WriteInfo();

            steps.ForEach(x => x.Execute());

            var obj = new
            {
                connectionString,
                databaseName,
            };

            var path = $"{Environment.CurrentDirectory}\\{filename}";

            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            this.WriteJson(path, json);

            return 0;
        }

        private void WriteInfo()
        {
            Console.WriteLine("This utility will walk you through creating a Harness settings file.");
            Console.WriteLine();

            Console.WriteLine("See `harness help` for information on these fields, or go to");
            Console.WriteLine("`https://github.com/AMCN41R/harness` to find out more.");
            Console.WriteLine();

            Console.WriteLine("Press ^C at any time to quit.");
            Console.WriteLine();
        }

        private void WriteJson(string path, string json)
        {
            Console.WriteLine($"Write to {path}:");
            Console.WriteLine(json);
            Console.WriteLine();

            Console.Write("All good? (yes)");

            var input = Console.ReadLine().Trim().ToLower();

            if (input == string.Empty || Ok.Contains(input))
            {
                System.IO.File.WriteAllText(path, json);
                Console.WriteLine();
            }

            Console.WriteLine("-- Aborted --");
            Console.WriteLine();
        }

        private static List<string> Ok { get; }
            = new List<string> { "y", "yes", "true", "ok" };

        private static Action<string> SetOrDefault(Action<string> setter, string defaultValue)
            => str => setter(str.IsNullOrWhitespace() ? defaultValue : str);

        private static bool IsOk(string str)
            => str.IsNullOrWhitespace() || Ok.Contains(str.Trim().ToLower());
    }
}

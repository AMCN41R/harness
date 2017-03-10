using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Harness.Settings
{
    /// <summary>
    /// An implementation of <see cref="IDataProvider"/> that loads data from
    /// the a json file. The json file should be an array of json objects.
    /// </summary>
    internal class FromJsonFileDataProvider : IDataProvider
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        public FromJsonFileDataProvider(string filepath) : this(filepath, new FileSystem())
        {
        }

        /// <summary>
        /// Internal constructor for unit testing.
        /// </summary>
        internal FromJsonFileDataProvider(string filepath, IFileSystem fileSystem)
        {
            this.Filepath = filepath;
            this.FileSystem = fileSystem;
        }

        /// <summary>
        /// Gets the filepath.
        /// </summary>
        private string Filepath { get; }

        /// <summary>
        /// Gets the <see cref="IFileSystem"/> implementation.
        /// </summary>
        private IFileSystem FileSystem { get; }

        /// <summary>
        /// Gets data from the given filepath. The file must have a .json extension.
        /// </summary>
        /// <returns>
        /// The data from the given file as an <see cref="IEnumerable{Object}"/>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the filepath cannot be validated.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the filepath does dot have a .json extension.</exception>
        public IEnumerable<object> GetData()
        {
            var result = this.FileSystem.ValidateFile(this.Filepath);

            if (!result?.IsValid ?? false)
            {
                throw new ArgumentException(
                    $"Error validating file {this.Filepath}. {result?.Message}",
                    nameof(this.Filepath)
                );
            }

            if (this.FileSystem.GetFileExtension(this.Filepath) != ".json")
            {
                throw new ArgumentOutOfRangeException(
                    nameof(this.Filepath),
                    "Invalid file type. File must be a .json file.");
            }

            var fileText = this.FileSystem.ReadAllText(this.Filepath);

            var itemArray =
                JsonConvert.DeserializeObject(fileText) as JArray;

            return itemArray?.Select(x => BsonDocument.Parse(x.ToString()));
        }
    }
}
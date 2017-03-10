using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harness
{
    public interface IFileSystem
    {
        /// <summary>
        /// Returns the extension of the specified path string.
        /// </summary>
        /// <param name="path">The path string from which to get the extension.</param>
        /// <returns>
        /// The extension of the specified path (including the period "."), or null, or <see cref="string.Empty"/>. 
        /// If path is null, returns null. 
        /// If path does not have extension information, returns <see cref="string.Empty"/>.
        /// </returns>
        string GetFileExtension(string path);

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        string ReadAllText(string path);

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The file to check.</param>
        /// <returns>True if the file exists, else false.</returns>
        bool FileExists(string path);
    }

    public class FileSystem : IFileSystem
    {
        /// <inheritdoc />
        public string GetFileExtension(string path)
        {
            Guard.AgainstNullEmptyOrWhitespace(path, nameof(path));

            return Path.GetExtension(path);
        }

        /// <inheritdoc />
        public string ReadAllText(string path)
        {
            Guard.AgainstNullEmptyOrWhitespace(path, nameof(path));

            return File.ReadAllText(path);
        }

        /// <inheritdoc />
        public bool FileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            return File.Exists(path);
        }
    }
}

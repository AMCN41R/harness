using System.IO;

namespace Harness
{
    /// <inheritdoc />
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

using System;
using System.IO.Abstractions;
using System.Linq;

namespace Harness
{
    internal static class Extensions
    {
        /// <summary>
        /// Gets the custom attribute <typeparamref name="T"/> from the given 
        /// class type.
        /// </summary>
        /// <typeparam name="T">The custom attribute.</typeparam>
        /// <param name="type">
        /// The class from which to retrive attribute <typeparamref name="T"/>
        /// </param>
        /// <returns>
        /// The first instance of the custom attribute. If none are
        /// found, returns null.
        /// </returns>
        public static T GetAttribute<T>(this Type type) where T : class
            => type.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;

        /// <summary>
        /// Validates a filepath using the given file system implementation.
        /// </summary>
        /// <param name="filepath">The filepath to validate.</param>
        /// <param name="fileSystem">
        /// The <see cref="IFileSystem"/> implementation
        /// to use to check that the file exists.
        /// </param>
        /// <returns>
        /// False if the file does not exist on the given file system 
        /// or if the filepath is empty, null or whitespace, otherwise returns 
        /// true.
        /// </returns>
        public static ValidationResult ValidateFile(this IFileSystem fileSystem, string filepath)
        {
            var isValid = true;
            var message = "OK";

            if (string.IsNullOrWhiteSpace(filepath))
            {
                isValid = false;
                message = "Filepath cannot be null, empty or whitespace.";
            }

            if (!fileSystem.File.Exists(filepath))
            {
                isValid = false;
                message = "File cannot be found or does not exist.";
            }

            return new ValidationResult
            {
                IsValid = isValid,
                Message = message
            };
        }
    }
}

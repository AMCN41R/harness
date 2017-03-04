using System;

namespace Harness
{
    internal static class Guard
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given 
        /// <paramref name="arg"/> if null, empty or whitespace.
        /// </summary>
        /// <param name="arg">The argument to check.</param>
        /// <param name="paramName">The name of the argument that is being checked.</param>
        public static void AgainstNullEmptyOrWhitespace(string arg, string paramName)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given 
        /// <paramref name="arg"/> is null.
        /// </summary>
        /// <param name="arg">The argument to check.</param>
        /// <param name="paramName">The name of the argument that is being checked.</param>
        public static void AgainstNullArgument<T>(T arg, string paramName) where T : class
        {
            if (arg == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}

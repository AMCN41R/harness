using System;

namespace Harness
{
    public static class Guard
    {
        public static void AgainstNullEmptyOrWhitespace(string paramName)
        {
            if (string.IsNullOrWhiteSpace(paramName))
            {
                throw new ArgumentNullException(nameof(paramName));
            }
        }

        public static void AgainstNullArgument<T>(string paramName, T arg) where T : class
        {
            if (arg == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}

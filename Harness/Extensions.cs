using System;
using System.Linq;

namespace Harness
{
    internal static class Extensions
    {
        public static T GetAttribute<T>(this Type type) where T : class
        {
            return type.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
        }
    }
}

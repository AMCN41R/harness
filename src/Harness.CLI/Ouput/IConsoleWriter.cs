namespace Harness.CLI.Ouput
{
    /// <summary>
    /// Provides an interface to allow different implementations that write
    /// items of type <typeparamref name="T"/> to the console.
    /// </summary>
    /// <typeparam name="T">The type of item to be written.</typeparam>
    internal interface IConsoleWriter<in T>
    {
        /// <summary>
        /// Writes the given item to the console.
        /// </summary>
        /// <param name="item">The item to be written.</param>
        void Write(T item);
    }
}

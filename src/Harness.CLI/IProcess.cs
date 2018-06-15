namespace Harness.CLI
{
    /// <summary>
    /// The IProcess interface.
    /// </summary>
    internal interface IProcess
    {
        /// <summary>
        /// Runs some process and returns an exit code.
        /// </summary>
        /// <returns>The process exit code.</returns>
        int Process();
    }
}

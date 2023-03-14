namespace Infra.CrossCutting.Log.Interfaces
{
    /// <summary>
    /// Logger Adapter
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    public interface ILoggerAdapter<T>
    {
        /// <summary>
        /// Writes the log information
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="args">object[]</param>
        void LogInformation(string message, params object[] args);
        /// <summary>
        /// Writes the log error information
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="message">string</param>
        /// <param name="args">object[]</param>
        void LogError(Exception ex, string message, params object[] args);
    }
}

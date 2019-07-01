using System;
using WideWorldImporters.Logger.Interfaces;

namespace WideWorldImporters.Logger.Implementation
{

    /// <summary>
    /// Application loggers
    /// </summary>
    public class AppLoggers : IWWILogger
    {

        private readonly ConsoleLogger _consoleLogger;
        private readonly NLogFileLogger _fileLogger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consoleLogger">Console Logger</param>
        /// <param name="fileLogger">File Logger</param>
        public AppLoggers(ConsoleLogger consoleLogger, NLogFileLogger fileLogger)
        {
            this._consoleLogger = consoleLogger;
            this._fileLogger = fileLogger;
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {
            _consoleLogger.Log(message);
            _fileLogger.Log(message);
        }

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void Log(Exception exception)
        {
            LogException(exception);
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">Debug Information to log</param>
        public void LogDebug(string message)
        {
            _consoleLogger.LogDebug(message);
            _fileLogger.LogDebug(message);
        }

        /// <summary>
        /// Logs a informational message
        /// </summary>
        /// <param name="message">Debug information to log</param>
        public void LogInfo(string message)
        {
            _consoleLogger.LogInfo(message);
            _fileLogger.LogInfo(message);
        }

        /// <summary>
        /// Logs a warning
        /// </summary>
        /// <param name="message">Warning message to log</param>
        public void LogWarn(string message)
        {
            _consoleLogger.Log(message);
            _fileLogger.Log(message);
        }

        /// <summary>
        /// Logs a error message
        /// </summary>
        /// <param name="message">Error to log</param>
        public void LogError(string message)
        {
            _consoleLogger.LogError(message);
            _fileLogger.LogError(message);
        }

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void LogException(Exception exception)
        {
            _consoleLogger.Log(exception.ToString());
            _fileLogger.Log(exception.ToString());
        }

    }
}

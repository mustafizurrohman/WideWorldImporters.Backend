using NLog;
using System;
using WideWorldImporters.Logger.Interfaces;

namespace WideWorldImporters.Logger.Implementation
{

    /// <summary>
    /// File Logger using NLog
    /// </summary>
    public class NLogFileLogger : IWWILogger
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">Debug Information to log</param>
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// Logs a error message
        /// </summary>
        /// <param name="message">Error to log</param>
        public void LogError(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="message">Exception to log</param>
        public void LogException(Exception exception)
        {
            logger.Error(exception);
        }

        /// <summary>
        /// Logs a informational message
        /// </summary>
        /// <param name="message">Informational message to log</param>
        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// Logs a warning
        /// </summary>
        /// <param name="message">Warning message to log</param>
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}

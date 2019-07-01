using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Logger.Interfaces;

namespace WideWorldImporters.Logger.Implementation
{

    /// <summary>
    /// Console Logging ( Can also be done using NLog)
    /// This is for demostration
    /// </summary>
    public class ConsoleLogger : IWWILogger
    {

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {
            Console.WriteLine(message + Environment.NewLine);
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
            Console.WriteLine(message + Environment.NewLine);
        }

        /// <summary>
        /// Logs a error message
        /// </summary>
        /// <param name="message">Error to log</param>
        public void LogError(string message)
        {
            Console.WriteLine(message + Environment.NewLine);
        }

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void LogException(Exception exception)
        {
            Console.WriteLine(exception.ToString() + Environment.NewLine);
        }

        /// <summary>
        /// Logs a informational message
        /// </summary>
        /// <param name="message">Debug information to log</param>
        public void LogInfo(string message)
        {
            Console.WriteLine(message + Environment.NewLine);
        }

        /// <summary>
        /// Logs a warning
        /// </summary>
        /// <param name="message">Warning message to log</param>
        public void LogWarn(string message)
        {
            Console.WriteLine(message + Environment.NewLine);
        }
    }
}

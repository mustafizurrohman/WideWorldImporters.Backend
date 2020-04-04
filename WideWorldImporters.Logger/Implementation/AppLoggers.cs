using System;
using System.Collections.Generic;
using System.Linq;
using WideWorldImporters.Logger.Interfaces;

namespace WideWorldImporters.Logger.Implementation
{

    /// <summary>
    /// Application loggers
    /// </summary>
    public class AppLoggers : IWWILogger
    {

        #region -- Private Attributes --

        /// <summary>
        /// The List of Loggers
        /// </summary>
        private readonly List<IWWILogger> _loggers = null;

        /// <summary>
        /// The console logger
        /// </summary>
        private readonly ConsoleLogger consoleLogger;

        /// <summary>
        /// The log file logger
        /// </summary>
        private readonly NLogFileLogger fileLogger;

        #endregion

        #region -- Constructor -- 

        /// <summary>
        /// Constructor
        /// </summary>
        public AppLoggers()
        {
            if (_loggers == null)
            {
                // Reflection is expensive (takes approx 400ms) but this is called ONLY ONCE because AppLoggers is Singleton
                // As long as all new loggers use IWWILogger as interface, this will work without any modification.
                _loggers = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(asm => asm.GetTypes())
                    .Where(typ => typeof(IWWILogger).IsAssignableFrom(typ) && !typ.IsInterface && !typ.IsAbstract)
                    .Where(typ => typ != typeof(AppLoggers))
                    .Select(typ => Activator.CreateInstance(typ) as IWWILogger)
                    .ToList();


                consoleLogger = _loggers.Where(logger => logger.GetType() == typeof(ConsoleLogger))
                    .Select(logger => (ConsoleLogger)logger)
                    .FirstOrDefault();

                fileLogger = _loggers.Where(logger => logger.GetType() == typeof(NLogFileLogger))
                    .Select(logger => (NLogFileLogger)logger)
                    .FirstOrDefault();
            }
        }

        #endregion

        #region -- Public Methods for All Loggers --

        /// <summary>
        /// Logs a message for all Loggers in Application
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {
            _loggers.ForEach(currentLogger =>
            {
                currentLogger.Log(message);
            });
        }

        /// <summary>
        /// Logs an exception for all Loggers in Application
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void Log(Exception exception)
        {
            _loggers.ForEach(currentLogger =>
            {
                currentLogger.LogException(exception);
            });
        }

        /// <summary>
        /// Logs a debug message for all Loggers in Application
        /// </summary>
        /// <param name="message">Debug Information to log</param>
        public void LogDebug(string message)
        {
            _loggers.ForEach(currentLogger =>
            {
                currentLogger.LogDebug(message);
            });
        }

        /// <summary>
        /// Logs a informational message for all Loggers in Application
        /// </summary>
        /// <param name="message">Debug information to log</param>
        public void LogInfo(string message)
        {
            _loggers.ForEach(currentLogger =>
            {
                currentLogger.LogInfo(message);
            });
        }

        /// <summary>
        /// Logs a warning for all Loggers in Application
        /// </summary>
        /// <param name="message">Warning message to log</param>
        public void LogWarn(string message)
        {
            _loggers.ForEach(currentLogger =>
            {
                currentLogger.LogWarn(message);
            });
        }

        /// <summary>
        /// Logs a error message for all Loggers in Application
        /// </summary>
        /// <param name="message">Error to log</param>
        public void LogError(string message)
        {
            _loggers.ForEach(currentLogger =>
            {
                currentLogger.LogError(message);
            });
        }

        /// <summary>
        /// Logs an exception for all Loggers in Application
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void LogException(Exception exception)
        {
            _loggers.ForEach(currentLogger =>
            {
                currentLogger.LogError(exception.ToString());
            });
        }

        #endregion

        #region -- Public Methods for Console Logger --

        /// <summary>
        /// Logs a message using console logger
        /// </summary>
        /// <param name="message">Message to log</param>
        public void LogToConsole(string message)
        {
            consoleLogger.Log(message);
        }

        /// <summary>
        /// Logs an exception using console logger
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void LogToConsole(Exception exception)
        {
            consoleLogger.LogException(exception);
        }

        /// <summary>
        /// Logs a debug message using console logger
        /// </summary>
        /// <param name="message">Debug Information to log</param>
        public void LogDebugToConsole(string message)
        {
            consoleLogger.LogDebug(message);
        }

        /// <summary>
        /// Logs a informational message using console logger
        /// </summary>
        /// <param name="message">Debug information to log</param>
        public void LogInfoToConsole(string message)
        {
            consoleLogger.LogInfo(message);
        }

        /// <summary>
        /// Logs a warning using console logger
        /// </summary>
        /// <param name="message">Warning message to log</param>
        public void LogWarnToConsole(string message)
        {
            consoleLogger.LogWarn(message);
        }

        /// <summary>
        /// Logs a error message using console logger
        /// </summary>
        /// <param name="message">Error to log</param>
        public void LogErrorToConsole(string message)
        {
            consoleLogger.LogError(message);
        }

        /// <summary>
        /// Logs an exception using console logger
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void LogExceptionToConsole(Exception exception)
        {
            consoleLogger.LogError(exception.ToString());
        }

        #endregion

        #region -- Public Methods for NLog File Logger --

        /// <summary>
        /// Logs a message using NLog File Logger
        /// </summary>
        /// <param name="message">Message to log</param>
        public void LogToFile(string message)
        {
            fileLogger.Log(message);
        }

        /// <summary>
        /// Logs an exception using NLog File Logger
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void LogToFile(Exception exception)
        {
            fileLogger.LogException(exception);
        }

        /// <summary>
        /// Logs a debug message using NLog File Logger
        /// </summary>
        /// <param name="message">Debug Information to log</param>
        public void LogDebugToFile(string message)
        {
            fileLogger.LogDebug(message);
        }

        /// <summary>
        /// Logs a informational message using NLog File Logger
        /// </summary>
        /// <param name="message">Debug information to log</param>
        public void LogInfoToFile(string message)
        {
            fileLogger.LogInfo(message);
        }

        /// <summary>
        /// Logs a warning using NLog File Logger
        /// </summary>
        /// <param name="message">Warning message to log</param>
        public void LogWarnToFile(string message)
        {
            fileLogger.LogWarn(message);
        }

        /// <summary>
        /// Logs a error message using NLog File Logger
        /// </summary>
        /// <param name="message">Error to log</param>
        public void LogErrorToFile(string message)
        {
            fileLogger.LogError(message);
        }

        /// <summary>
        /// Logs an exception using NLog File Logger
        /// </summary>
        /// <param name="exception">Exception to log</param>
        public void LogExceptionToFile(Exception exception)
        {
            fileLogger.LogError(exception.ToString());
        }

        #endregion

    }
}

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

        private readonly ConsoleLogger _consoleLogger;
        private readonly NLogFileLogger _fileLogger;

        private readonly List<Type> _loggers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consoleLogger">Console Logger</param>
        /// <param name="fileLogger">File Logger</param>
        public AppLoggers(ConsoleLogger consoleLogger, NLogFileLogger fileLogger)
        {
            _consoleLogger = consoleLogger;
            _fileLogger = fileLogger;

            if (_loggers == null)
            {
                // Reflection is expensive but this is called only once because AppLoggers is Singleton
                _loggers = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(asm => asm.GetTypes())
                    .Where(typ => typeof(IWWILogger).IsAssignableFrom(typ) && !typ.IsInterface && !typ.IsAbstract)
                    .Where(typ => typ != typeof(AppLoggers))
                    // .Select(typ => (IWWILogger)typ as IWWILogger)
                    .ToList();
            }
        }

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {

            //_loggers.ForEach(log =>
            //{
            //    var logger = log as IWWILogger;

            //    logger.Log(message);

            //});

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
            _consoleLogger.LogError(exception.ToString());
            _fileLogger.LogError(exception.ToString());
        }

    }
}

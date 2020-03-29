using System;
using System.Collections.Generic;
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
        private readonly List<IWWILogger> _loggers;

        #endregion

        #region -- Constructor -- 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consoleLogger">Console Logger</param>
        /// <param name="fileLogger">File Logger</param>
        public AppLoggers(ConsoleLogger consoleLogger, NLogFileLogger fileLogger)
        {
            // We are still using DI even when not used!
            // TODO: Optimize this

            if (_loggers == null)
            {
                _loggers = new List<IWWILogger>
                {
                    consoleLogger,
                    fileLogger
                };

                // TODO: Can we use reflection here?
                /*
                // Reflection is expensive but this is called only once because AppLoggers is Singleton
                _loggers = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(asm => asm.GetTypes())
                    .Where(typ => typeof(IWWILogger).IsAssignableFrom(typ) && !typ.IsInterface && !typ.IsAbstract)
                    .Where(typ => typ != typeof(AppLoggers))
                    // .Select(typ => (IWWILogger)typ as IWWILogger)
                    .ToList();
                */
            }
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Logs a message
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
        /// Logs an exception
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
        /// Logs a debug message
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
        /// Logs a informational message
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
        /// Logs a warning
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
        /// Logs a error message
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
        /// Logs an exception
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

    }
}

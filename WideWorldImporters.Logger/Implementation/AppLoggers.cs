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

        #endregion

        #region -- Constructor -- 

        /// <summary>
        /// Constructor
        /// </summary>
        public AppLoggers()
        {
            if (_loggers == null)
            {

                // Reflection is expensive (takes approx 400ms) but this is called only once because AppLoggers is Singleton
                // As long as all new loggers use IWWILogger as interface, this will work without any modification.
                _loggers = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(asm => asm.GetTypes())
                    .Where(typ => typeof(IWWILogger).IsAssignableFrom(typ) && !typ.IsInterface && !typ.IsAbstract)
                    .Where(typ => typ != typeof(AppLoggers))
                    .Select(typ => Activator.CreateInstance(typ) as IWWILogger)
                    .ToList();
            }
        }

        #endregion

        #region -- Public Methods --

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

    }
}

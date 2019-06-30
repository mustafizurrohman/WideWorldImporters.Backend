using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Logger.Interfaces;

namespace WideWorldImporters.Logger.Implementation
{
    public class AppLoggers : IWWILogger
    {

        private readonly ConsoleLogger _consoleLogger;
        private readonly NLogFileLogger _fileLogger;

        public AppLoggers(ConsoleLogger consoleLogger, NLogFileLogger fileLogger)
        {
            this._consoleLogger = consoleLogger;
            this._fileLogger = fileLogger;
        }

        public void Log(string message)
        {
            _consoleLogger.Log(message);
            _fileLogger.Log(message);
        }

        public void Log(Exception exception)
        {
            LogException(exception);
        }

        public void LogDebug(string message)
        {
            _consoleLogger.LogDebug(message);
            _fileLogger.LogDebug(message);
        }

        public void LogError(string message)
        {
            _consoleLogger.LogError(message);
            _fileLogger.LogError(message);
        }

        public void LogException(Exception exception)
        {
            _consoleLogger.Log(exception.ToString());
            _fileLogger.Log(exception.ToString());
        }

        public void LogInfo(string message)
        {
            _consoleLogger.LogInfo(message);
            _fileLogger.LogInfo(message);
        }

        public void LogWarn(string message)
        {
            _consoleLogger.Log(message);
            _fileLogger.Log(message);
        }
    }
}

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
            throw new NotImplementedException();
        }

        public void LogDebug(string message)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message)
        {
            throw new NotImplementedException();
        }

        public void LogException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void LogInfo(string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarn(string message)
        {
            throw new NotImplementedException();
        }
    }
}

using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Logger.Interfaces;

namespace WideWorldImporters.Logger.Implementation
{
    public class WWILogger : IWWILogger
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogException(Exception exception)
        {
            logger.Error(exception);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}

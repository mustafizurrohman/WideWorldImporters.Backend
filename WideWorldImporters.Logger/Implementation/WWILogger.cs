using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Logger.Interfaces;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Logger.Implementation
{

    // [ServiceLifeTime(Lifetime.Transient)]
    public class WWILogger : IWWILogger
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public void Log(string message)
        {
            logger.Info(message);
        }

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

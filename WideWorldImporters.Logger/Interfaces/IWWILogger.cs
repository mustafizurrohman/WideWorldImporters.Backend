using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Logger.Interfaces
{
    public interface IWWILogger
    {
        void Log(string message);
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
        void LogException(Exception exception);
    }
}

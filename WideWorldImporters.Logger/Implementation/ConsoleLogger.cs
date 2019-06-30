using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Logger.Interfaces;

namespace WideWorldImporters.Logger.Implementation
{
    public class ConsoleLogger : IWWILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message + Environment.NewLine);
        }

        public void Log(Exception exception)
        {
            LogException(exception);
        }

        public void LogDebug(string message)
        {
            Console.WriteLine(message + Environment.NewLine);
        }

        public void LogError(string message)
        {
            Console.WriteLine(message + Environment.NewLine);
        }

        public void LogException(Exception exception)
        {
            Console.WriteLine(exception.ToString() + Environment.NewLine);
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(message + Environment.NewLine);
        }

        public void LogWarn(string message)
        {
            Console.WriteLine(message + Environment.NewLine);
        }
    }
}

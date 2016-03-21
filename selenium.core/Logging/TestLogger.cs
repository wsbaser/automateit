/**
 * Created by VolkovA on 27.02.14.
 */

using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using NLog.Config;
using OpenQA.Selenium;
using selenium.core.Exceptions;

namespace selenium.core.Logging
{
    public class TestLogger : ITestLogger
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        private static readonly Logger Log;

        static TestLogger()
        {
            const string CONFIG_FILE_NAME = "nlog.config";
            string buildCheckoutDir = Environment.GetEnvironmentVariable("BuildCheckoutDir");
            string baseDirectory = string.IsNullOrEmpty(buildCheckoutDir)
                ? AppContext.BaseDirectory
                : buildCheckoutDir;
            string configFilePath = Path.Combine(baseDirectory, CONFIG_FILE_NAME);
            LogManager.Configuration = new XmlLoggingConfiguration(configFilePath);
            Log = LogManager.GetLogger("TestLogger");
        }

        #region TestLogger Members

        public void Action(String msg, params object[] args)
        {
            msg = string.Format(msg, args);
            Log.Info(msg);
            Console.WriteLine(msg);
        }

        public void Info(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            Log.Info(msg);
            Console.WriteLine(msg);
        }

        public void FatalError(String msg, Exception e)
        {
            Log.Info(msg, e);
            Console.WriteLine(msg);
        }

        public void WriteValue(string key, object value)
        {
            if (!_values.ContainsKey(key))
                _values.Add(key, value);
            else
                _values[key] = value;
        }

        public T GetValue<T>(string key)
        {
            if (!_values.ContainsKey(key))
                throw Throw.TestException(string.Format("Value with key '{0}' was not logged", key));
            return (T) _values[key];
        }

        public void Selector(By by)
        {
            Log.Info("By: {0}", by);
            Console.WriteLine("By: {0}", by);
        }

        public void Exception(Exception exception)
        {
            Log.Info("Exception: {0}", exception.Message);
            Console.WriteLine("Exception: {0}", exception.Message);
        }

        #endregion
    }
}
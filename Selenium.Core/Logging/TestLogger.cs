/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Logging
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using NLog;
    using NLog.Config;

    using OpenQA.Selenium;

    using Selenium.Core.Exceptions;

    public class TestLogger : ITestLogger
    {
        private static readonly Logger Log;

        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        static TestLogger()
        {
            const string CONFIG_FILE_NAME = "nlog.config";
            var buildCheckoutDir = Environment.GetEnvironmentVariable("BuildCheckoutDir");
            var baseDirectory = string.IsNullOrEmpty(buildCheckoutDir) ? AppContext.BaseDirectory : buildCheckoutDir;
            var configFilePath = Path.Combine(baseDirectory, CONFIG_FILE_NAME);
            LogManager.Configuration = new XmlLoggingConfiguration(configFilePath);
            Log = LogManager.GetLogger("TestLogger");
        }

        #region TestLogger Members

        public void Action(string msg, params object[] args)
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

        public void FatalError(string msg, Exception e)
        {
            this.Info(msg, e);
            Console.WriteLine(msg);
        }

        public void WriteValue(string key, object value)
        {
            if (!this._values.ContainsKey(key))
            {
                this._values.Add(key, value);
            }
            else
            {
                this._values[key] = value;
            }
        }

        public T GetValue<T>(string key)
        {
            if (!this._values.ContainsKey(key))
            {
                throw Throw.TestException(string.Format("Value with key '{0}' was not logged", key));
            }
            return (T)this._values[key];
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
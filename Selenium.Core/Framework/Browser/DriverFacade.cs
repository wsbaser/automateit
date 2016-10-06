/**
* Created by VolkovA on 27.02.14.
*/

namespace Selenium.Core.Framework.Browser
{
    using System;

    using OpenQA.Selenium;

    using Selenium.Core.Framework.Service;
    using Selenium.Core.Logging;

    public abstract class DriverFacade
    {
        public DriverFacade(Browser browser)
        {
            this.Browser = browser;
        }

        protected Browser Browser { get; }

        protected Web Web
        {
            get
            {
                return this.Browser.Web;
            }
        }

        protected ITestLogger Log
        {
            get
            {
                return this.Browser.Log;
            }
        }

        protected IWebDriver Driver
        {
            get
            {
                return this.Browser.Driver;
            }
        }

        /// <summary>
        ///     Повторять действие если возникло StaleReferenceException
        /// </summary>
        /// <param name="func">Действие</param>
        public T RepeatAfterStale<T>(Func<T> func)
        {
            const int TRY_COUNT = 3;
            var result = default(T);
            for (var i = 0; i < TRY_COUNT; i++)
            {
                try
                {
                    result = func.Invoke();
                    break;
                }
                catch (StaleElementReferenceException e)
                {
                    this.Log.Exception(e);
                    if (i == TRY_COUNT - 1)
                    {
                        throw;
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///     Повторять действие если возникло StaleReferenceException
        /// </summary>
        /// <param name="action">действие</param>
        public void RepeatAfterStale(Action action)
        {
            this.RepeatAfterStale(
                () =>
                    {
                        action.Invoke();
                        return true;
                    });
        }
    }
}
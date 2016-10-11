/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Framework.Page
{
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using OpenQA.Selenium;

    using Selenium.Core.Framework.Browser;
    using Selenium.Core.Framework.Service;
    using Selenium.Core.Logging;

    public abstract class PageBase : IPage
    {
        public Browser Browser { get; private set; }

        public ITestLogger Log { get; private set; }

        public BrowserAction Action
        {
            get
            {
                return this.Browser.Action;
            }
        }

        public BrowserAlert Alert
        {
            get
            {
                return this.Browser.Alert;
            }
        }

        public BrowserFind Find
        {
            get
            {
                return this.Browser.Find;
            }
        }

        public BrowserGet Get
        {
            get
            {
                return this.Browser.Get;
            }
        }

        public BrowserGo Go
        {
            get
            {
                return this.Browser.Go;
            }
        }

        public BrowserIs Is
        {
            get
            {
                return this.Browser.Is;
            }
        }

        public BrowserState State
        {
            get
            {
                return this.Browser.State;
            }
        }

        public BrowserWait Wait
        {
            get
            {
                return this.Browser.Wait;
            }
        }

        public BrowserJs Js
        {
            get
            {
                return this.Browser.Js;
            }
        }

        public BrowserWindow Window
        {
            get
            {
                return this.Browser.Window;
            }
        }

        BrowserCookies IPageObject.Cookies
        {
            get
            {
                return this.Browser.Cookies;
            }
        }

        #region IPage Members

        /// <summary>
        ///     Активизировать страницу
        /// </summary>
        /// <remarks>
        ///     Если страница активна, значит через нее можно работать с браузером
        /// </remarks>
        public void Activate(Browser browser, ITestLogger log)
        {
            this.Browser = browser;
            this.Log = log;
            this.Alerts = new List<IHtmlAlert>();
            this.ProgressBars = new List<IProgressBar>();
            WebPageBuilder.InitPage(this);
        }

        public List<IProgressBar> ProgressBars { get; private set; }

        public List<IHtmlAlert> Alerts { get; private set; }

        public BaseUrlInfo BaseUrlInfo { get; set; }

        public List<Cookie> Cookies { get; set; }

        public StringDictionary Params { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public void RegisterComponent(IComponent component)
        {
            if (component is IHtmlAlert)
            {
                this.Alerts.Add(component as IHtmlAlert);
            }
            else if (component is IProgressBar)
            {
                this.ProgressBars.Add(component as IProgressBar);
            }
        }

        public T RegisterComponent<T>(string componentName, params object[] args) where T : IComponent
        {
            var component = this.CreateComponent<T>(args);
            this.RegisterComponent(component);
            component.ComponentName = componentName;
            return component;
        }

        public T CreateComponent<T>(params object[] args) where T : IComponent
        {
            return (T)WebPageBuilder.CreateComponent<T>(this, args);
        }

        #endregion
    }
}
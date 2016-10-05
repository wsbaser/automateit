namespace Selenium.Widget.v3.Tests.@Base
{
    using Selenium.Core.Exceptions;
    using Selenium.Core.Framework.Browser;
    using Selenium.Core.Framework.Page;
    using Selenium.Core.Logging;

    public abstract class TestBase
    {
        protected abstract Browser Browser { get; }

        protected abstract ITestLogger Log { get; }

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

        private BrowserCookies Cookies
        {
            get
            {
                return this.Browser.Cookies;
            }
        }

        public T GoTo<T>(bool update = true, bool waitForAjax = true, bool ajaxInevitable = false) where T : IPage
        {
            if (!this.Browser.State.PageIs<T>() || update)
            {
                this.Browser.Go.ToPage<T>();
            }
            if (!this.Browser.State.PageIs<T>())
            {
                throw Throw.FrameworkException("Не перешли на страницу '{0}'", typeof(T).Name);
            }
            if (waitForAjax)
            {
                this.Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
            }
            return this.Browser.State.PageAs<T>();
        }
    }
}
using selenium.core.Exceptions;
using selenium.core.Framework.Browser;
using selenium.core.Framework.Page;
using selenium.core.Logging;

namespace selenium.widgets_ui_3._0.tests.@base
{
    public abstract class TestBase
    {
        protected abstract Browser Browser { get; }
        protected abstract TestLogger Log { get; }

        public T GoTo<T>(bool update = true, bool waitForAjax = true, bool ajaxInevitable = false) where T : IPage
        {
            if (!Browser.State.PageIs<T>() || update)
                Browser.Go.ToPage<T>();
            if (!Browser.State.PageIs<T>())
                throw Throw.FrameworkException("Не перешли на страницу '{0}'", typeof(T).Name);
            if (waitForAjax)
                Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
            return Browser.State.PageAs<T>();
        }

        public BrowserAction Action
        {
            get { return Browser.Action; }
        }

        public BrowserAlert Alert
        {
            get { return Browser.Alert; }
        }

        public BrowserFind Find
        {
            get { return Browser.Find; }
        }

        public BrowserGet Get
        {
            get { return Browser.Get; }
        }

        public BrowserGo Go
        {
            get { return Browser.Go; }
        }

        public BrowserIs Is
        {
            get { return Browser.Is; }
        }

        public BrowserState State
        {
            get { return Browser.State; }
        }

        public BrowserWait Wait
        {
            get { return Browser.Wait; }
        }

        public BrowserJs Js
        {
            get { return Browser.Js; }
        }

        public BrowserWindow Window
        {
            get { return Browser.Window; }
        }

        private BrowserCookies Cookies
        {
            get { return Browser.Cookies; }
        }
    }
}
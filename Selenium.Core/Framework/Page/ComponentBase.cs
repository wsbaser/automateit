namespace Selenium.Core.Framework.Page
{
    using System;

    using NUnit.Framework;

    using Selenium.Core.Exceptions;
    using Selenium.Core.Framework.Browser;
    using Selenium.Core.Logging;

    public abstract class ComponentBase : IComponent
    {
        private string _componentName;

        protected ComponentBase(IPage parent)
        {
            this.ParentPage = parent;
        }

        public void WaitForVisible()
        {
            this.Wait.Until(this.IsVisible);
        }

        public void AssertVisible()
        {
            Assert.IsTrue(this.IsVisible(), "'{0}' не отображается", this.ComponentName);
        }

        public void AssertNotVisible()
        {
            Assert.IsFalse(this.IsVisible(), "'{0}' отображается", this.ComponentName);
        }

        /// <summary>
        ///     Открыть компонент
        /// </summary>
        /// <param name="action">
        ///     Действие которое нужно выполнить если компонент не отображается
        /// </param>
        public virtual ComponentBase Open(Action action)
        {
            if (!this.IsVisible())
            {
                action.Invoke();
            }
            if (!this.IsVisible())
            {
                throw Throw.FrameworkException("Компонент '{0}' не открылся", this.ComponentName);
            }
            return this;
        }

        public T Open<T>(Action action) where T : ComponentBase
        {
            return (T)this.Open(action);
        }

        #region IComponent Members

        public virtual string ComponentName
        {
            get
            {
                this._componentName = this._componentName ?? (this._componentName = this.GetType().Name);
                return this._componentName;
            }
            set
            {
                this._componentName = value;
            }
        }

        public IPage ParentPage { get; }

        public abstract bool IsVisible();

        public Browser Browser
        {
            get
            {
                return this.ParentPage.Browser;
            }
        }

        public ITestLogger Log
        {
            get
            {
                return this.ParentPage.Log;
            }
        }

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

        #endregion
    }
}
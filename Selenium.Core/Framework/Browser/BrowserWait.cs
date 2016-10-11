/**
* Created by VolkovA on 27.02.14.
*/ // Методы для ожидания изменения состояния браузера

namespace Selenium.Core.Framework.Browser
{
    using System;
    using System.Linq;
    using System.Threading;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    using Selenium.Core.SCSS;

    public class BrowserWait : DriverFacade
    {
        public BrowserWait(Browser browser)
            : base(browser)
        {
        }

        public void Until(Func<bool> condition, int seconds = 3)
        {
            var wait = new WebDriverWait(
                new SystemClock(),
                this.Driver,
                TimeSpan.FromSeconds(seconds),
                TimeSpan.FromMilliseconds(100));
            wait.Until(driver => condition.Invoke());
        }

        /// <summary>
        ///     Подождать пока элемент отображается на странице
        /// </summary>
        /// <param name="by">Селектор видимого элемента</param>
        /// <param name="timeout">Максимальный период ожидания</param>
        public void WhileElementVisible(string scssSelector, int timeout = BrowserTimeouts.AJAX)
        {
            this.WhileElementVisible(Scss.GetBy(scssSelector), timeout);
        }

        public void WhileElementVisible(By by, int timeout = BrowserTimeouts.AJAX)
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(driver => !this.Browser.Is.Visible(by));
        }

        /// <summary>
        ///     Подождать пока не скроются все зарегистрированные на страницы прогрессы
        /// </summary>
        public void WhilePageInProgress()
        {
            if (this.Browser.State.Page == null)
            {
                return;
            }
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(BrowserTimeouts.PAGE_LOAD));
            wait.Until(driver => this.Browser.State.Page.ProgressBars.All(p => !p.IsVisible()));
        }

        public void ForPageProgress()
        {
            if (this.Browser.State.Page == null)
            {
                return;
            }
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(BrowserTimeouts.JS));
            wait.Until(driver => this.Browser.State.Page.ProgressBars.All(p => p.IsVisible()));
        }

        /// <summary>
        ///     Подождать пока не завершатся Ajax запросы
        /// </summary>
        /// <param name="timeout">максимальное время ожидания пока отработают все ajax запросы</param>
        /// <param name="ajaxInevitable">
        ///     true - ajax запрос 100% должен выполниться
        ///     если этого не произошло, ожидаем 3 секунды и проверяем еще раз
        /// </param>
        public void WhileAjax(int timeout = BrowserTimeouts.AJAX, bool ajaxInevitable = false)
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(timeout));
            var waited = false;
            wait.Until(
                driver =>
                    {
                        var ajaxActive = this.Browser.Is.AjaxActive();
                        if (ajaxActive)
                        {
                            waited = true;
                            return false;
                        }
                        return true;
                    });
            if (!waited && ajaxInevitable)
            {
                this.Browser.Wait.ForAjax(3000);
                wait.Until(driver => !this.Browser.Is.AjaxActive());
            }
        }

        /// <summary>
        ///     Подождать пока не начнется выполнения ajax запросов
        /// </summary>
        public void ForAjax(int miliseconds = 1000)
        {
            const int POLLING_INTERVAL = 200;
            var count = (int)Math.Ceiling(miliseconds / (decimal)POLLING_INTERVAL);
            for (var i = 0; i < count; i++)
            {
                if (this.Browser.Is.AjaxActive())
                {
                    return;
                }
                Thread.Sleep(POLLING_INTERVAL);
            }
        }
    }
}
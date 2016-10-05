/**
 * Created by VolkovA on 26.02.14.
 */

namespace Selenium.Core.Framework.Browser
{
    using System;

    using OpenQA.Selenium;

    using Selenium.Core.Framework.Service;
    using Selenium.Core.Logging;

    public class Browser
    {
        private readonly DriverManager _driverManager;

        public readonly BrowserAction Action;

        public readonly BrowserAlert Alert;

        public readonly BrowserFind Find;

        public readonly BrowserGet Get;

        public readonly BrowserGo Go;

        public readonly BrowserIs Is;

        public readonly BrowserJs Js;

        public readonly BrowserState State;

        public readonly BrowserWait Wait;

        public BrowserCookies Cookies;

        public BrowserOptions Options = new BrowserOptions();

        public BrowserWindow Window;

        public Browser(Web web, ITestLogger log, DriverManager driverManager)
        {
            this.Web = web;
            this.Log = log;
            this._driverManager = driverManager;
            this._driverManager.InitDriver();
            this.Driver = this._driverManager.GetDriver();
            this.Find = new BrowserFind(this);
            this.Get = new BrowserGet(this);
            this.Is = new BrowserIs(this);
            this.Alert = new BrowserAlert(this);
            this.State = new BrowserState(this);
            this.Action = new BrowserAction(this);
            this.Window = new BrowserWindow(this);
            this.Go = new BrowserGo(this);
            this.Wait = new BrowserWait(this);
            this.Js = new BrowserJs(this);
            this.Cookies = new BrowserCookies(this);
        }

        public ITestLogger Log { get; private set; }

        public Web Web { get; private set; }

        public IWebDriver Driver { get; }

        // Уничтожить драйвер(закрывает все открытые окна браузер)
        public void Destroy()
        {
            this._driverManager.DestroyDriver();
        }

        // Пересоздать драйвер
        public void Recreate()
        {
        }

        public void DisableTimeout()
        {
            this.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        public void EnableTimeout()
        {
            this.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(BrowserTimeouts.FIND));
        }

        public void WithOptions(Action action, bool findSingle = BrowserOptions.FINDSINGLE_DEFAULT)
        {
            var memento = (BrowserOptions)this.Options.Clone();
            this.Options.FindSingle = findSingle;
            action.Invoke();
            this.Options = memento;
        }
    }

    public class BrowserOptions
    {
        /// <summary>
        ///     Если при поиске элемента по селектору найдено несколько кидать исключение
        /// </summary>
        public const bool FINDSINGLE_DEFAULT = true;

        /// <summary>
        ///     Ожидать завершения Ajax запросов перед выполнением клика
        /// </summary>
        public const bool WAIT_WHILE_AJAX_BEFORE_CLICK = true;

        public bool FindSingle = FINDSINGLE_DEFAULT;

        public bool WaitWhileAjaxBeforeClick = WAIT_WHILE_AJAX_BEFORE_CLICK;

        public object Clone()
        {
            var options = new BrowserOptions
                              {
                                  FindSingle = this.FindSingle,
                                  WaitWhileAjaxBeforeClick = this.WaitWhileAjaxBeforeClick
                              };
            return options;
        }
    }

    public static class BrowserTimeouts
    {
        /// <summary>
        ///     Иногда после выполнения некоторого действия страница подвисает и просто ничего не происходит.
        ///     Данное значение определяет максимальное время ожидания пока отрабатывает Java Script
        /// </summary>
        public const int JS = 10;

        /// <summary>
        ///     Таймаут ожидания при поиске элемента по умолчанию
        /// </summary>
        public const int FIND = 10;

        /// <summary>
        ///     Таймаут ожидания пока отображается прогресс
        /// </summary>
        public const int AJAX = 30;

        /// <summary>
        ///     Таймаут ожидания пока подгружаются компоненты страницы
        /// </summary>
        public const int PAGE_LOAD = 30;
    }
}
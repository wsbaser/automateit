/**
* Created by VolkovA on 27.02.14.
*/ // Текущее состояние браузера

namespace Selenium.Core.Framework.Browser
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;
    using Selenium.Core.Framework.Service;

    public class BrowserState : DriverFacade
    {
        /// <summary>
        ///     Идентификатор текущего окна
        /// </summary>
        public string CurrentWindowHandle;

        // Объект для работы с html имитацией алерта, отображаемой в активной странице браузера
        public IAlert HtmlAlert;

        // Объект для работы со страницей в активном окне браузера
        public IPage Page;

        // Объект для работы с системным алертом, отображаемым в активной странице браузера
        public IAlert SystemAlert;

        public BrowserState(Browser browser)
            : base(browser)
        {
        }

        public IAlert GetActiveAlert()
        {
            return this.SystemAlert ?? this.HtmlAlert;
        }

        /// <summary>
        ///     Приведение текущего html алерта к указанному типу
        /// </summary>
        public T HtmlAlertAs<T>()
        {
            return (T)this.HtmlAlert;
        }

        /// <summary>
        ///     Приведение текущего html алерта к указанному типу
        /// </summary>
        public bool HtmlAlertIs<T>()
        {
            if (this.HtmlAlert == null)
            {
                return false;
            }
            return this.HtmlAlert is T;
        }

        // Приведение текущей страницы к нужному типу
        public T PageAs<T>()
        {
            return (T)this.Page;
        }

        // Проверка соответствия класса текущей страницы указанному типу
        public bool PageIs<T>() where T : IPage
        {
            if (this.Page == null)
            {
                return false;
            }
            return this.Page is T;
        }

        // Определение текущего состояния браузера
        public void Actualize()
        {
            this.ActualizeSystemAlert();
            if (this.SystemAlert != null)
            {
                return;
            }
            this.ActualizeWindow();
            this.ActualizePage(
                new RequestData(
                    this.Driver.Url,
                    new List<Cookie>(this.Driver.Manage().Cookies.AllCookies.AsEnumerable())));
            this.ActualizeHtmlAlert();
        }

        /// <summary>
        ///     Актуализация текущего окна
        /// </summary>
        private void ActualizeWindow()
        {
            if (this.Driver.WindowHandles.Last() != this.CurrentWindowHandle)
            {
                this.Driver.SwitchTo().Window(this.Driver.WindowHandles.Last());
                this.CurrentWindowHandle = this.Driver.CurrentWindowHandle;
            }
        }

        /// <summary>
        ///     Актуализация Html алерта
        /// </summary>
        public void ActualizeHtmlAlert()
        {
            this.HtmlAlert = null;
            if (this.Page == null)
            {
                return;
            }
            this.HtmlAlert = this.Page.Alerts.FirstOrDefault(a => a.IsVisible());
        }

        /// <summary>
        ///     Актуализация системного алерта
        /// </summary>
        private void ActualizeSystemAlert()
        {
            this.SystemAlert = this.Browser.Alert.GetSystemAlert();
        }

        /// <summary>
        ///     Определение класса для работы с текущей активной страницей браузера
        /// </summary>
        private void ActualizePage(RequestData requestData)
        {
            this.Page = null;
            if (requestData.Url.IsFile)
            {
                this.Page = this.Web.GetEmailPage(requestData.Url);
            }
            else
            {
                var result = this.Web.MatchService(requestData);
                if (result != null)
                {
                    this.Page = result.getService().GetPage(requestData, result.getBaseUrlInfo());
                }
            }
            if (this.Page != null)
            {
                this.Page.Activate(this.Browser, this.Log);
            }
        }
    }
}
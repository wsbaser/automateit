/**
* Created by VolkovA on 27.02.14.
*/ // Методы для навигации по страницам

namespace Selenium.Core.Framework.Browser
{
    using System;
    using System.IO;

    using Selenium.Core.Framework.Page;
    using Selenium.Core.Framework.Service;

    public class BrowserGo : DriverFacade
    {
        public BrowserGo(Browser browser)
            : base(browser)
        {
        }

        // Определение Url, соответствующее классу страницы и переход на него
        public T ToPage<T>() where T : IPage
        {
            var pageInstance = (T)Activator.CreateInstance(typeof(T));
            this.ToPage(pageInstance);
            return this.Browser.State.PageAs<T>();
        }

        // Определение Url, соответствующее классу страницы и переход на него
        public void ToPage(IPage page)
        {
            var requestData = this.Web.GetRequestData(page);
            this.ToUrl(requestData);
        }

        public void ToUrl(string url)
        {
            this.ToUrl(new RequestData(url));
        }

        // Переход на указанный Url в текущем окне браузера
        public void ToUrl(RequestData requestData)
        {
            this.Log.Action("Navigating to url: {0}", requestData.Url);
            this.Driver.Navigate().GoToUrl(requestData.Url);
            this.Browser.State.Actualize();
            this.Browser.Wait.WhileAjax();
        }

        /// <summary>
        ///     Сохранить исходный код страницы на диске и открыть страницу в браузере
        /// </summary>
        /// <typeparam name="T">Класс страницы</typeparam>
        /// <param name="html">Исходный код страницы</param>
        public T ToHtml<T>(string html) where T : IPage
        {
            // Сохраниить на диск
            var type = typeof(T);
            var fileName = type.Name + ".html";
            var pagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "SavedPages");
            if (!Directory.Exists(pagesFolder))
            {
                Directory.CreateDirectory(pagesFolder);
            }
            var filePath = Path.Combine(pagesFolder, fileName);
            File.WriteAllText(filePath, html);

            // Открыть в браузере
            this.ToUrl("file://" + filePath);

            // Создать соответствующий класс страницы
            var page = (T)Activator.CreateInstance(type);
            page.Activate(this.Browser, this.Log);
            return page;
        }

        /// <summary>
        ///     Найти письмо с указанным заголовком на указанном почтовом ящике.
        ///     Открыть текст письма в браузере
        /// </summary>
        /// <summary>
        ///     Вернуться на предыдущую страницу
        /// </summary>
        public void Back()
        {
            this.Driver.Navigate().Back();
            this.Log.Action("Go.Back(). Result Url: {0}", this.Driver.Url);
            this.Browser.State.Actualize();
        }
    }
}
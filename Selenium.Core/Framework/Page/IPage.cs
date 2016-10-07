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

    public interface IPage : IPageObject
    {
        /// <summary>
        ///     Куки, имеющие влияние на содержимое страницы
        /// </summary>
        new List<Cookie> Cookies { get; set; }

        /// <summary>
        ///     Данные, содержащиеся в Url
        /// </summary>
        Dictionary<string, string> Data { get; set; }

        /// <summary>
        ///     Параметры, содержащиеся в Url
        /// </summary>
        StringDictionary Params { get; set; }

        /// <summary>
        ///     Информация о домене, поддомене и абсолютном пути
        /// </summary>
        BaseUrlInfo BaseUrlInfo { get; set; }

        List<IHtmlAlert> Alerts { get; }

        /// <summary>
        ///     Список зарегистрированных прогрессов
        /// </summary>
        List<IProgressBar> ProgressBars { get; }

        /// <summary>
        ///     Зарегистрировать компонент
        /// </summary>
        void RegisterComponent(IComponent component);

        /// <summary>
        ///     Зарегистрировать компонент
        /// </summary>
        T RegisterComponent<T>(string componentName, params object[] args) where T : IComponent;

        /// <summary>
        ///     Создать компонент
        /// </summary>
        T CreateComponent<T>(params object[] args) where T : IComponent;

        /// <summary>
        ///     Активизировать страницу
        /// </summary>
        void Activate(Browser browser, ITestLogger log);
    }
}
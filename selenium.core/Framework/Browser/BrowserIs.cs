/**
* Created by VolkovA on 27.02.14.
*/ // Методы для проверки состояния страниц

namespace Selenium.Core.Framework.Browser
{
    using System.Linq;

    using OpenQA.Selenium;

    using Selenium.Core.SCSS;

    public class BrowserIs : DriverFacade
    {
        public BrowserIs(Browser browser)
            : base(browser)
        {
        }

        /// <summary>
        ///     Проверяет что элемент отображается на странице
        /// </summary>
        public bool Visible(string scssSelector)
        {
            return this.Visible(ScssBuilder.CreateBy(scssSelector));
        }

        public bool Visible(By by)
        {
            return this.RepeatAfterStale(
                () =>
                    {
                        var elements = this.Browser.Find.Elements(by);
                        return elements.Count != 0 && elements.Any(e => e.Displayed);
                    });
        }

        public bool Visible(ISearchContext context, By by)
        {
            return this.RepeatAfterStale(
                () =>
                    {
                        var element = this.Browser.Find.ElementFastS(context, by);
                        return element != null && element.Displayed;
                    });
        }

        /// <summary>
        ///     Проверяет имеется ли у элемента указанный класс
        /// </summary>
        public bool HasClass(string scssSelector, string className)
        {
            return this.HasClass(ScssBuilder.CreateBy(scssSelector), className);
        }

        public bool HasClass(By by, string className)
        {
            var element = this.Browser.Find.ElementFastS(by);
            if (element == null)
            {
                return false;
            }
            return this.HasClass(element, className);
        }

        public bool HasClass(IWebElement element, string className)
        {
            return element.GetAttribute("class").Split(' ').Select(c => c.Trim()).Contains(className);
        }

        /// <summary>
        ///     Существует ли элемент на странице
        /// </summary>
        public bool Exists(string scssSelector)
        {
            return this.Exists(ScssBuilder.CreateBy(scssSelector));
        }

        public bool Exists(By by)
        {
            return this.Browser.Find.ElementFastS(by, false) != null;
        }

        /// <summary>
        ///     true - выполняется хотя бы один ajax запрос
        /// </summary>
        public bool AjaxActive()
        {
            return !this.Browser.Js.Excecute<bool>(@"
                        var isJqueryComplete = typeof(jQuery) != 'function' || jQuery.active == 0;
                        var isPrototypeComplete = typeof(Ajax) != 'function' || Ajax.activeRequestCount == 0;
                        var isDojoComplete = typeof(dojo) != 'function' || dojo.io.XMLHTTPTransport.inFlight.length == 0;
                        return isJqueryComplete && isPrototypeComplete && isDojoComplete;");
        }

        /// <summary>
        ///     Отмечен ли чекбокс
        /// </summary>
        public bool Checked(string scssSelector)
        {
            return this.Checked(ScssBuilder.CreateBy(scssSelector));
        }

        public bool Checked(By by)
        {
            return this.RepeatAfterStale(() => this.Checked(this.Browser.Find.Element(by, false)));
        }

        public bool Checked(IWebElement element)
        {
            return element.GetAttribute("checked") == "true";
        }
    }
}
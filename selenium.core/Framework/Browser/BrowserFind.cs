/**
* Created by VolkovA on 27.02.14.
*/ // Методы для поиска элементов на странице

namespace Selenium.Core.Framework.Browser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenQA.Selenium;

    using Selenium.Core.Exceptions;
    using Selenium.Core.SCSS;

    public class BrowserFind : DriverFacade
    {
        public BrowserFind(Browser browser)
            : base(browser)
        {
        }

        /// <summary>
        ///     Поиск элемента. Если не найден - кинуть исключение
        /// </summary>
        public IWebElement Element(string scssSelector)
        {
            return this.Element(ScssBuilder.CreateBy(scssSelector));
        }

        public IWebElement Element(By by, bool displayed = true)
        {
            return this.Element(this.Driver, by, displayed);
        }

        public IWebElement Element(ISearchContext context, By by, bool displayed = true)
        {
            var start = DateTime.Now;
            var elements = context.FindElements(by).ToList();
            if (elements.Count == 0)
            {
                this.Log.Selector(by);
                throw new NoSuchElementException(
                    string.Format("Search time: {0}", (DateTime.Now - start).TotalMilliseconds));
            }
            if (displayed)
            {
                elements = elements.Where(e => e.Displayed).ToList();
                if (elements.Count == 0)
                {
                    this.Log.Selector(by);
                    throw new NoVisibleElementsException();
                }
            }
            if (elements.Count > 1)
            {
                Throw.TestException("Found more then 1 element by selector '{0}'", by);
            }
            return this.Browser.Options.FindSingle ? elements.SingleOrDefault() : elements.First();
        }

        /// <summary>
        ///     Попытка поиска элемента. Если не найден - не кидать исключение
        /// </summary>
        public IWebElement ElementFastS(string scssSelector, bool displayed = true)
        {
            return this.ElementFastS(ScssBuilder.CreateBy(scssSelector), displayed);
        }

        public IWebElement ElementFastS(By by, bool displayed = true)
        {
            return this.ElementFastS(this.Driver, by, displayed);
        }

        public IWebElement ElementFastS(ISearchContext context, By by, bool displayed = true)
        {
            try
            {
                this.Browser.DisableTimeout();
                return this.Element(context, by, displayed);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (NoVisibleElementsException)
            {
                return null;
            }
            finally
            {
                this.Browser.EnableTimeout();
            }
        }

        /// <summary>
        ///     Найти элемент без ожидания
        /// </summary>
        public IWebElement ElementFast(string scssSelector)
        {
            return this.ElementFast(ScssBuilder.CreateBy(scssSelector));
        }

        public IWebElement ElementFast(By by)
        {
            try
            {
                this.Browser.DisableTimeout();
                this.Log.Selector(by);
                return this.Driver.FindElement(by);
            }
            finally
            {
                this.Browser.EnableTimeout();
            }
        }

        /// <summary>
        ///     Найти элементы по указанному селектору без ожидания. Не падать если ничего не найдено
        /// </summary>
        public List<IWebElement> Elements(string scssSelector)
        {
            return this.Elements(ScssBuilder.CreateBy(scssSelector));
        }

        public List<IWebElement> Elements(By by)
        {
            try
            {
                this.Browser.DisableTimeout();
                return new List<IWebElement>(this.Driver.FindElements(by));
            }
            catch (NoSuchElementException)
            {
                return new List<IWebElement>();
            }
            finally
            {
                this.Browser.EnableTimeout();
            }
        }

        /// <summary>
        ///     Найти элементы по указанному селектору без ожидания. Не падать если ничего не найдено
        ///     Вернуть только видимые элементы
        /// </summary>
        public List<IWebElement> VisibleElements(string scssSelector)
        {
            return this.VisibleElements(ScssBuilder.CreateBy(scssSelector));
        }

        public List<IWebElement> VisibleElements(By by)
        {
            return this.RepeatAfterStale(() => this.Elements(by).Where(e => e.Displayed).ToList());
        }
    }
}
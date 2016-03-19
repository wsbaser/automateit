/**
* Created by VolkovA on 27.02.14.
*/ // Методы для поиска элементов на странице

using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using selenium.core.Exceptions;
using selenium.core.SCSS;

namespace selenium.core.Framework.Browser {
    public class BrowserFind : DriverFacade {
        public BrowserFind(Browser browser)
            : base(browser) {
        }

        /// <summary>
        /// Поиск элемента. Если не найден - кинуть исключение
        /// </summary>
        public IWebElement Element(string scssSelector) {
            return Element(ScssBuilder.CreateBy(scssSelector));
        }

        public IWebElement Element(By by, bool displayed = true) {
            return Element(Driver, by, displayed);
        }

        public IWebElement Element(ISearchContext context, By by, bool displayed = true) {
            DateTime start = DateTime.Now;
            List<IWebElement> elements = context.FindElements(by).ToList();
            if (elements.Count == 0) {
                Log.Selector(by);
                throw new NoSuchElementException(string.Format("Search time: {0}",(DateTime.Now-start).TotalMilliseconds));
            }
            if (displayed) {
                elements = elements.Where(e => e.Displayed).ToList();
                if (elements.Count == 0) {
                    Log.Selector(by);
                    throw new NoVisibleElementsException();
                }
            }
            if (elements.Count > 1)
                Throw.TestException("Found more then 1 element by selector '{0}'", by);
            return Browser.Options.FindSingle ? elements.SingleOrDefault() : elements.First();
        }

        /// <summary>
        /// Попытка поиска элемента. Если не найден - не кидать исключение
        /// </summary>
        public IWebElement ElementFastS(string scssSelector, bool displayed = true) {
            return ElementFastS(ScssBuilder.CreateBy(scssSelector), displayed);
        }

        public IWebElement ElementFastS(By by, bool displayed = true) {
            return ElementFastS(Driver, by, displayed);
        }

        public IWebElement ElementFastS(ISearchContext context, By by, bool displayed = true) {
            try {
                Browser.DisableTimeout();
                return Element(context, by, displayed);
            }
            catch (NoSuchElementException) {
                return null;
            }
            catch (NoVisibleElementsException) {
                return null;
            }
            finally {
                Browser.EnableTimeout();
            }
        }

        /// <summary>
        /// Найти элемент без ожидания
        /// </summary>
        public IWebElement ElementFast(string scssSelector) {
            return ElementFast(ScssBuilder.CreateBy(scssSelector));
        }

        public IWebElement ElementFast(By by) {
            try {
                Browser.DisableTimeout();
                Log.Selector(by);
                return Driver.FindElement(by);
            }
            finally {
                Browser.EnableTimeout();
            }
        }

        /// <summary>
        /// Найти элементы по указанному селектору без ожидания. Не падать если ничего не найдено
        /// </summary>
        public List<IWebElement> Elements(string scssSelector) {
            return Elements(ScssBuilder.CreateBy(scssSelector));
        }

        public List<IWebElement> Elements(By by) {
            try {
                Browser.DisableTimeout();
                return new List<IWebElement>(Driver.FindElements(by));
            }
            catch (NoSuchElementException) {
                return new List<IWebElement>();
            }
            finally {
                Browser.EnableTimeout();
            }
        }

        /// <summary>
        /// Найти элементы по указанному селектору без ожидания. Не падать если ничего не найдено
        /// Вернуть только видимые элементы
        /// </summary>
        public List<IWebElement> VisibleElements(string scssSelector) {
            return VisibleElements(ScssBuilder.CreateBy(scssSelector));
        }

        public List<IWebElement> VisibleElements(By by) {
            return RepeatAfterStale(() => Elements(by).Where(e => e.Displayed).ToList());
        }
    }
}
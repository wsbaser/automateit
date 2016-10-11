/**
* Created by VolkovA on 27.02.14.
*/ // Методы для получения данных со страницы

namespace Selenium.Core.Framework.Browser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Core.Extensions;

    using OpenQA.Selenium;

    using Selenium.Core.SCSS;

    public class BrowserGet : DriverFacade
    {
        public BrowserGet(Browser browser)
            : base(browser)
        {
        }

        /// <summary>
        ///     Получить исходный код страницы
        /// </summary>
        public string PageSource
        {
            get
            {
                return this.Browser.Driver.PageSource;
            }
        }

        // Получить содержимое элемента
        public string TextS(string scssSelector)
        {
            return this.TextS(ScssBuilder.CreateBy(scssSelector));
        }

        public string TextS(By by)
        {
            return this.RepeatAfterStale(
                () =>
                    {
                        var element = this.Browser.Find.ElementFastS(by);
                        if (element == null)
                        {
                            return null;
                        }
                        return element.Text;
                    });
        }

        // Получить содержимое элемента
        public string Text(string scssSelector, bool displayed = false)
        {
            return this.Text(ScssBuilder.CreateBy(scssSelector));
        }

        public string Text(By by, bool displayed = false)
        {
            return this.Text(this.Driver, by, displayed);
        }

        /// <summary>
        ///     Получить содержимое элемента
        /// </summary>
        public string Text(ISearchContext context, By by, bool displayed = false)
        {
            if (displayed && !this.Browser.Is.Visible(context, by))
            {
                return null;
            }
            return this.RepeatAfterStale(() => this.Browser.Find.Element(context, @by, displayed).Text);
        }

        /// <summary>
        ///     Найти элементы по указанному селектору и получить их тексты
        /// </summary>
        public List<string> Texts(string scssSelector, bool displayed = false)
        {
            return this.Texts(ScssBuilder.CreateBy(scssSelector), displayed);
        }

        public List<string> Texts(By by, bool displayed = false)
        {
            return this.RepeatAfterStale(
                () =>
                    {
                        var elements = this.Browser.Find.Elements(by);
                        if (displayed)
                        {
                            elements = elements.Where(e => e.Displayed).ToList();
                        }
                        return elements.Select(e => e.Text).ToList();
                    });
        }

        /// <summary>
        ///     Получить атрибут src тега img
        /// </summary>
        public string ImgFileName(string scssSelector)
        {
            return this.ImgFileName(ScssBuilder.CreateBy(scssSelector));
        }

        public string ImgFileName(By by)
        {
            return this.ImgSrc(by).Split('/').Last();
        }

        /// <summary>
        ///     Получить атрибут src тега img
        /// </summary>
        public string ImgSrc(string scssSelector)
        {
            return this.ImgSrc(ScssBuilder.CreateBy(scssSelector));
        }

        public string ImgSrc(By by)
        {
            return this.Attr(by, "src");
        }

        /// <summary>
        ///     Получить список атрибутов src тегов img
        /// </summary>
        public List<string> ImgSrcs(string scssSelector)
        {
            return this.ImgSrcs(ScssBuilder.CreateBy(scssSelector));
        }

        public List<string> ImgSrcs(By by)
        {
            return this.Attrs(by, "src");
        }

        /// <summary>
        ///     Получить аттрибут элемента
        /// </summary>
        /// <param name="scssSelector">селектор элемента</param>
        /// <param name="name">имя аттрибута</param>
        /// <param name="displayed">искать только видимые элементы</param>
        public string Attr(string scssSelector, string name, bool displayed = true)
        {
            return this.Attr(ScssBuilder.CreateBy(scssSelector), name, displayed);
        }

        public string Attr(By by, string name, bool displayed = true)
        {
            return this.RepeatAfterStale(() => this.Attr(this.Browser.Find.Element(@by, displayed), name));
        }

        public T Attr<T>(string scssSelector, string name, bool displayed = true)
        {
            return this.Attr<T>(ScssBuilder.CreateBy(scssSelector), name, displayed);
        }

        public T Attr<T>(By by, string name, bool displayed = true)
        {
            return this.Cast<T>(this.Attr(by, name, displayed));
        }

        public string Attr(IWebElement element, string name)
        {
            return element.GetAttribute(name);
        }

        /// <summary>
        ///     Получить аттрибуты элементов
        /// </summary>
        /// <param name="scssSelector">селектор элемента</param>
        /// <param name="name">имя аттрибута</param>
        public List<string> Attrs(string scssSelector, string name)
        {
            return this.Attrs(ScssBuilder.CreateBy(scssSelector), name);
        }

        public List<string> Attrs(By by, string name)
        {
            return this.RepeatAfterStale(() => this.Browser.Find.Elements(@by).Select(e => this.Attr(e, name)).ToList());
        }

        /// <summary>
        ///     Получить атрибут href тега a
        /// </summary>
        public string Href(string scssSelector)
        {
            return this.Href(ScssBuilder.CreateBy(scssSelector));
        }

        public string Href(By by)
        {
            return this.Attr(by, "href");
        }

        /// <summary>
        ///     Получить список атрибутов href тегов a
        /// </summary>
        public List<string> Hrefs(string scssSelector)
        {
            return this.Hrefs(ScssBuilder.CreateBy(scssSelector));
        }

        public List<string> Hrefs(By by)
        {
            return this.Attrs(by, "href");
        }

        /// <summary>
        ///     Получить список атрибутов указанного типа
        /// </summary>
        public List<T> Attrs<T>(string scssSelector, string name)
        {
            return this.Attrs<T>(ScssBuilder.CreateBy(scssSelector), name);
        }

        public List<T> Attrs<T>(By by, string name)
        {
            return
                this.RepeatAfterStale(
                    () => this.Browser.Find.Elements(by).Select(e => this.Attr(e, name)).Select(this.Cast<T>).ToList());
        }

        private T Cast<T>(string value)
        {
            var type = typeof(T);
            if (type == typeof(short) || type == typeof(int) || type == typeof(long))
            {
                return (T)Convert.ChangeType(value.FindInt(), typeof(T));
            }
            if (type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong))
            {
                return (T)Convert.ChangeType(value.FindUInt(), typeof(T));
            }
            if (type == typeof(decimal) || type == typeof(float))
            {
                return (T)Convert.ChangeType(value.FindNumber(), typeof(T));
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        ///     Получить содержимое поля ввода
        /// </summary>
        public string InputValue(string scssSelector, bool displayed = true)
        {
            return this.InputValue(ScssBuilder.CreateBy(scssSelector), displayed);
        }

        public string InputValue(By by, bool displayed = true)
        {
            return this.Attr(by, "value", displayed);
        }

        public string InputValue(IWebElement element, bool displayed = true)
        {
            return this.Attr(element, "value");
        }

        /// <summary>
        ///     Получить целое значение из элемента найденного по селектору
        /// </summary>
        public int Int(string scssSelector, bool displayed = false)
        {
            return this.Int(ScssBuilder.CreateBy(scssSelector), displayed);
        }

        public int Int(By by, bool displayed = false)
        {
            return this.Int(this.Driver, by, displayed);
        }

        public int Int(ISearchContext context, By by, bool displayed = false)
        {
            return this.Text(context, by, displayed).AsInt();
        }

        /// <summary>
        ///     Получить целые значения из элементов, найденных по селектору
        /// </summary>
        public List<int> Ints(string scssSelector)
        {
            return this.Ints(ScssBuilder.CreateBy(scssSelector));
        }

        public List<int> Ints(By by)
        {
            return this.Texts(by).Select(s => s.AsInt()).ToList();
        }

        /// <summary>
        ///     Получить css параметр тега
        /// </summary>
        public T CssValue<T>(string scssSelector, ECssProperty property)
        {
            return this.CssValue<T>(ScssBuilder.CreateBy(scssSelector), property);
        }

        public T CssValue<T>(By by, ECssProperty property)
        {
            return this.RepeatAfterStale(() => this.CssValue<T>(this.Browser.Find.Element(by), property));
        }

        public T CssValue<T>(IWebElement element, ECssProperty property)
        {
            var value = element.GetCssValue(property.StringValue());
            return this.Cast<T>(value);
        }

        /// <summary>
        ///     Получить количество элементов, найденных по указанному селектору
        /// </summary>
        public int Count(string scssSelector)
        {
            return this.Count(ScssBuilder.CreateBy(scssSelector));
        }

        public int Count(By by)
        {
            return this.RepeatAfterStale(() => this.Browser.Find.Elements(by).Count);
        }

        /// <summary>
        ///     Сделать скриншот указанной области экрана
        /// </summary>
        /// <summary>
        ///     Получить прямоугольник, заданный Css свойствами элемента left, top, height, width
        /// </summary>
        /// <summary>
        ///     Получить координаты элемента
        /// </summary>
        /// <summary>
        ///     Получить значение указанного типа из найденного по селектору элементу
        /// </summary>
        public T Value<T>(string scssSelector)
        {
            return this.Value<T>(ScssBuilder.CreateBy(scssSelector));
        }

        public T Value<T>(By by)
        {
            return this.RepeatAfterStale(() => this.Value<T>(this.Browser.Find.Element(by)));
        }

        public T Value<T>(IWebElement element)
        {
            return this.Cast<T>(element.Text);
        }

        /// <summary>
        ///     Получить список текстов по селектору и объединить их в строку
        /// </summary>
        public string TextsAsString(string scssSelector, string delimiter = " ")
        {
            return this.TextsAsString(ScssBuilder.CreateBy(scssSelector), delimiter);
        }

        public string TextsAsString(By by, string delimiter = " ")
        {
            return this.Texts(by).AsString(delimiter);
        }

        /// <summary>
        ///     Получить атрибут href и получить из него абсолютный Url(без домена)
        /// </summary>
        public string AbsoluteHref(By by)
        {
            return this.Href(by).CutBaseUrl();
        }
    }
}
/**
* Created by VolkovA on 27.02.14.
*/ // Методы для выполнения действий со страницей

namespace Selenium.Core.Framework.Browser
{
    using System;
    using System.Threading;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    using Selenium.Core.SCSS;

    public class BrowserAction : DriverFacade
    {
        private readonly BrowserFind _find;

        public BrowserAction(Browser browser)
            : base(browser)
        {
            this._find = browser.Find;
        }

        /// <summary>
        ///     Выбрать опцию в html теге Select
        /// </summary>
        public void Select(string scssSelector, string value)
        {
            this.Select(ScssBuilder.CreateBy(scssSelector), value);
        }

        public void Select(By by, string value)
        {
            this.RepeatAfterStale(
                () =>
                    {
                        var select = this._find.Element(by);
                        var dropDown = new SelectElement(select);
                        dropDown.SelectByValue(value);
                    });
        }

        /// <summary>
        ///     Ввести значение в поле ввода
        /// </summary>
        public void TypeIn(string scssSelector, object value, bool clear = true)
        {
            this.TypeIn(ScssBuilder.CreateBy(scssSelector), value, clear);
        }

        public void TypeIn(By by, object value, bool clear = true)
        {
            this.RepeatAfterStale(
                () =>
                    {
                        var element = this._find.Element(by);
                        if (clear)
                        {
                            this.Clear(element);
                        }
                        foreach (var c in value.ToString())
                        {
                            element.SendKeys(c.ToString());
                        }
                    });
        }

        /// <summary>
        ///     Клик по элементу
        /// </summary>
        public void Click(string scssSelector, int sleepTimeout = 0)
        {
            this.Click(ScssBuilder.CreateBy(scssSelector), sleepTimeout);
        }

        public void Click(By by, int sleepTimeout = 0)
        {
            this.Click(this.Driver, by, sleepTimeout);
        }

        public void Click(ISearchContext context, By by, int sleepTimeout = 0)
        {
            this.RepeatAfterStale(
                () =>
                    {
                        try
                        {
                            if (this.Browser.Options.WaitWhileAjaxBeforeClick)
                            {
                                this.Browser.Wait.WhileAjax();
                            }
                            this.Click(this._find.Element(context, by), sleepTimeout);
                        }
                        catch (InvalidOperationException e)
                        {
                            this.Log.Selector(by);
                            this.Log.Exception(e);
                        }
                    });
        }

        public void Click(IWebElement element, int sleepTimeout = 0)
        {
            //Browser.Js.ScrollIntoView(element); // Fix for "element not visible" exception
            element.Click();
            if (sleepTimeout != 0)
            {
                Thread.Sleep(sleepTimeout);
            }
        }

        /// <summary>
        ///     Клик по элементу с ожиданием алерта
        /// </summary>
        public void ClickAndWaitForAlert(string scssSelector, int timeout = BrowserTimeouts.AJAX)
        {
            this.ClickAndWaitForAlert(ScssBuilder.CreateBy(scssSelector), timeout);
        }

        public void ClickAndWaitForAlert(By by, int timeout = BrowserTimeouts.AJAX)
        {
            this.Click(by);
            this.Browser.Wait.Until(
                () =>
                    {
                        this.Browser.State.ActualizeHtmlAlert();
                        return this.Browser.State.HtmlAlert != null;
                    },
                timeout);
            this.Browser.State.Actualize();
        }

        /// <summary>
        ///     Клик и ожидание редиректа
        /// </summary>
        public void ClickAndWaitForRedirect(string scssSelector, bool waitForAjax = false, bool ajaxInevitable = false)
        {
            this.ClickAndWaitForRedirect(ScssBuilder.CreateBy(scssSelector), waitForAjax, ajaxInevitable);
        }

        public void ClickAndWaitForRedirect(By by, bool waitForAjax = false, bool ajaxInevitable = false)
        {
            this.RepeatAfterStale(
                () => this.ClickAndWaitForRedirect(this.Browser.Find.Element(by), waitForAjax, ajaxInevitable));
        }

        public void ClickAndWaitForRedirect(IWebElement element, bool waitForAjax = false, bool ajaxInevitable = false)
        {
            const int POLLING_INTERVAL = 200;
            var oldUrl = this.Browser.Window.Url;
            this.Click(element, 1000);
            for (var i = 0; i < 10; i++)
            {
                this.Browser.State.Actualize();
                if (this.Browser.State.SystemAlert != null || oldUrl != this.Browser.Window.Url)
                {
                    if (waitForAjax || ajaxInevitable)
                    {
                        this.Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
                    }
                    return;
                }
                Thread.Sleep(POLLING_INTERVAL);
            }
            this.Log.Info("No redirect after click");
        }

        /// <summary>
        ///     Клик и ожидание пока отрыбатывают Ajax Запросы
        /// </summary>
        public void ClickAndWaitWhileAjax(string scssSelector, int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            this.ClickAndWaitWhileAjax(ScssBuilder.CreateBy(scssSelector), sleepTimeout, ajaxInevitable);
        }

        public void ClickAndWaitWhileAjax(By by, int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            this.ClickAndWaitWhileAjax(this.Driver, by, sleepTimeout, ajaxInevitable);
        }

        public void ClickAndWaitWhileAjax(
            ISearchContext context,
            By by,
            int sleepTimeout = 0,
            bool ajaxInevitable = false)
        {
            this.RepeatAfterStale(
                () => this.ClickAndWaitWhileAjax(this.Browser.Find.Element(context, by), sleepTimeout, ajaxInevitable));
        }

        public void ClickAndWaitWhileAjax(IWebElement element, int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            this.Browser.Wait.WhileAjax();
            this.Click(element, sleepTimeout);
            this.Browser.Wait.WhileAjax(ajaxInevitable: ajaxInevitable);
            this.Browser.State.Actualize();
        }

        /// <summary>
        ///     Нажатие клавиши в поле ввода
        /// </summary>
        public void PressKey(string scssSelector, string key)
        {
            this.PressKey(ScssBuilder.CreateBy(scssSelector), key);
        }

        public void PressKey(By by, string key)
        {
            this.RepeatAfterStale(() => this.PressKey(this.Browser.Find.Element(by), key));
        }

        public void PressKey(IWebElement element, string key)
        {
            element.SendKeys(key);
        }

        /// <summary>
        ///     Очистить текстовое поле
        /// </summary>
        public void Clear(string scssSelector)
        {
            this.Clear(ScssBuilder.CreateBy(scssSelector));
        }

        public void Clear(By by)
        {
            this.RepeatAfterStale(() => this.Clear(this.Browser.Find.Element(by)));
        }

        public void Clear(IWebElement element)
        {
            element.Clear();
        }

        /// <summary>
        ///     Убрать фокус с текущего компонента
        /// </summary>
        public void ChangeFocus()
        {
            this.PressKey(this.Driver.SwitchTo().ActiveElement(), Keys.Tab);
        }

        /// <summary>
        ///     Кликнуть и подождать пока на странице отображается прогресс
        /// </summary>
        /// <param name="sleepTimeout">принудительное ожидание после выполнения клика</param>
        /// <param name="progressInevitable">
        ///     true означает что после клика прогресс точно должен появиться
        ///     поэтому сначала ожидаем его появления, потом ожидаем пока он не исчезнет
        /// </param>
        public void ClickAndWaitWhileProgress(
            string scssSelector,
            int sleepTimeout = 0,
            bool progressInevitable = false)
        {
            this.ClickAndWaitWhileProgress(ScssBuilder.CreateBy(scssSelector), sleepTimeout, progressInevitable);
        }

        public void ClickAndWaitWhileProgress(By by, int sleepTimeout = 0, bool progressInevitable = false)
        {
            this.Click(by, sleepTimeout);
            if (progressInevitable)
            {
                this.Browser.Wait.ForPageProgress();
            }
            this.Browser.Wait.WhilePageInProgress();
            this.Browser.State.Actualize();
        }

        /// <summary>
        ///     Нажать Enter в поле найденному по селектору
        /// </summary>
        public void PressEnter(string scssSelector)
        {
            this.PressEnter(ScssBuilder.CreateBy(scssSelector));
        }

        public void PressEnter(By by)
        {
            this.PressKey(by, Keys.Enter);
        }

        /// <summary>
        ///     Клик по всем элементам найденным по указанному селектору
        /// </summary>
        public void ClickByAll(string scssSelector)
        {
            this.ClickByAll(ScssBuilder.CreateBy(scssSelector));
        }

        public void ClickByAll(By by)
        {
            var elements = this.Browser.Find.Elements(by);
            foreach (var element in elements)
            {
                this.Browser.Action.Click(element);
            }
        }

        /// <summary>
        ///     Прокрутить страницу до низа
        /// </summary>
        public void ScrollToBottom()
        {
            var start = DateTime.Now;
            do
            {
                this.Browser.Js.ScrollToBottom();
                this.Browser.Wait.WhileAjax(ajaxInevitable: true);
            }
            while (!this.Browser.Js.IsPageBottom() && (DateTime.Now - start).TotalSeconds < 300);
        }

        /// <summary>
        ///     Сохранить
        /// </summary>
        /// <param name="marker">название файла скриншота</param>
        /// <param name="folder">папка для скриншотов</param>
        /// <summary>
        ///     Навести курсор на элемент
        /// </summary>
        public void MouseOver(string scssSelector, int sleepTimeout = 0)
        {
            this.MouseOver(ScssBuilder.CreateBy(scssSelector), sleepTimeout);
        }

        public void MouseOver(By by, int sleepTimeout = 0)
        {
            var action = new Actions(this.Driver);
            this.RepeatAfterStale(
                () =>
                    {
                        var element = this.Browser.Find.Element(by);
                        action.MoveToElement(element).Build().Perform();
                        if (sleepTimeout != 0)
                        {
                            Thread.Sleep(sleepTimeout);
                        }
                    });
        }

        public void SetFocus(IWebElement element)
        {
            if (element.TagName == "input")
            {
                element.SendKeys("");
            }
            else
            {
                new Actions(this.Driver).MoveToElement(element).Build().Perform();
            }
        }

        /// <summary>
        ///     Перетащить элемент на указанное количество пикселей по горизонтали
        /// </summary>
        public void DragByHorizontal(string scssSelector, int pixels)
        {
            this.DragByHorizontal(ScssBuilder.CreateBy(scssSelector), pixels);
        }

        public void DragByHorizontal(By by, int pixels)
        {
            var builder = new Actions(this.Driver);
            builder.DragAndDropToOffset(this.Browser.Find.Element(by), pixels, 0).Build().Perform();
        }
    }
}
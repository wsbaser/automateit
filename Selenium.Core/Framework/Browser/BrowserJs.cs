namespace Selenium.Core.Framework.Browser
{
    using OpenQA.Selenium;

    public class BrowserJs : DriverFacade
    {
        public BrowserJs(Browser browser)
            : base(browser)
        {
        }

        /// <summary>
        ///     Выполнить Java Script
        /// </summary>
        public T Excecute<T>(string js)
        {
            return (T)this.Excecute(js);
        }

        /// <summary>
        ///     Выполнить Java Script
        /// </summary>
        public object Excecute(string js, params object[] args)
        {
            var excecutor = this.Driver as IJavaScriptExecutor;
            js = string.Format(js, args);
            return excecutor.ExecuteScript(js);
        }

        /// <summary>
        ///     Прокрутить скролбарр по Y чтобы элемент стал видим
        /// </summary>
        /// <summary>
        ///     Получить обработчики события для указанного элемента
        /// </summary>
        /// <remarks>только для страниц с JQuery</remarks>
        public string GetEventHandlers(string css, JsEventType eventType)
        {
            var js = string.Format(@"var handlers= $._data($('{0}').get(0),'events').{1};
                          var s='';
                          for(var i=0;i<handlers.length;i++)
                            s+=handlers[i].handler.toString();
                          return s;", css, eventType);
            return this.Excecute<string>(js);
        }

        /// <summary>
        ///     Находимся ли внизу страницы
        /// </summary>
        public bool IsPageBottom()
        {
            return
                this.Excecute<bool>(
                    "return document.body.scrollHeight==="
                    + "document.body.scrollTop+document.documentElement.clientHeight");
        }

        /// <summary>
        ///     Прокрутить скроллбар до низа страницы
        /// </summary>
        public void ScrollToBottom()
        {
            this.Excecute(@"window.scrollTo(0,
                                       Math.max(document.documentElement.scrollHeight,
                                                document.body.scrollHeight,
                                                document.documentElement.clientHeight));");
        }

        /// <summary>
        ///     Прокрутить скроллбар до верха страницы
        /// </summary>
        public void ScrollToTop()
        {
            this.Excecute(@"window.scrollTo(0,0);");
        }
    }

    /// <summary>
    ///     типы стандартных js событий
    /// </summary>
    public enum JsEventType
    {
        click
    }
}
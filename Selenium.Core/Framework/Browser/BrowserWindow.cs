/**
* Created by VolkovA on 27.02.14.
*/ // Методы для работы с окнами/вкладками браузера

namespace Selenium.Core.Framework.Browser
{
    public class BrowserWindow : DriverFacade
    {
        public BrowserWindow(Browser browser)
            : base(browser)
        {
        }

        public string Url
        {
            get
            {
                return this.Driver.Url;
            }
        }
    }
}
/**
* Created by VolkovA on 27.02.14.
*/ // Методы для работы с алертами

namespace Selenium.Core.Framework.Browser
{
    using OpenQA.Selenium;

    public class BrowserAlert : DriverFacade
    {
        public BrowserAlert(Browser browser)
            : base(browser)
        {
        }

        /// <summary>
        ///     Получение системного алерта
        /// </summary>
        public IAlert GetSystemAlert()
        {
            try
            {
                return this.Driver.SwitchTo().Alert();
            }
            catch (NoAlertPresentException)
            {
                return null;
            }
        }
    }
}
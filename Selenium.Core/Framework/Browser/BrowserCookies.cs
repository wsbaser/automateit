namespace Selenium.Core.Framework.Browser
{
    public class BrowserCookies : DriverFacade
    {
        public BrowserCookies(Browser browser)
            : base(browser)
        {
        }

        /// <summary>
        ///     �������� ��� Cookie
        /// </summary>
        public void Clear()
        {
            this.Driver.Manage().Cookies.DeleteAllCookies();
        }
    }
}
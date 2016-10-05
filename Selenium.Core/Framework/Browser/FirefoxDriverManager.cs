/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Framework.Browser
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;

    public class FirefoxDriverManager : DriverManager
    {
        private FirefoxDriver _driver;

        #region DriverManager Members

        public IWebDriver GetDriver()
        {
            return this._driver;
        }

        public void InitDriver()
        {
            this._driver = new FirefoxDriver();
        }

        public void DestroyDriver()
        {
            this._driver.Quit();
        }

        #endregion
    }
}
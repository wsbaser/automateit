/**
 * Created by VolkovA on 03.03.14.
 */

namespace Selenium.Core.Framework.Browser
{
    using System;
    using System.IO;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    public class ChromeDriverFacrory : DriverManager
    {
        private IWebDriver _driver;

        #region DriverManager Members

        public void InitDriver()
        {
            var buildCheckoutDir = Environment.GetEnvironmentVariable("BuildCheckoutDir");
            this._driver = string.IsNullOrEmpty(buildCheckoutDir)
                               ? new ChromeDriver()
                               : new ChromeDriver(Path.Combine(buildCheckoutDir, "selenium.core\\"));
        }

        public IWebDriver GetDriver()
        {
            return this._driver;
        }

        public void DestroyDriver()
        {
            this._driver.Quit();
        }

        #endregion
    }
}
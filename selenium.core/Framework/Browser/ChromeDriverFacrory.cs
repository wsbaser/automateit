/**
 * Created by VolkovA on 03.03.14.
 */

using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace selenium.core.Framework.Browser {
    public class ChromeDriverFacrory : DriverManager {
        private IWebDriver _driver;

        #region DriverManager Members

        public void InitDriver() {
            string buildCheckoutDir = Environment.GetEnvironmentVariable("BuildCheckoutDir");
            _driver = string.IsNullOrEmpty(buildCheckoutDir)
                          ? new ChromeDriver()
                          : new ChromeDriver(Path.Combine(buildCheckoutDir, "selenium.core\\"));
        }

        public IWebDriver GetDriver() {
            return _driver;
        }

        public void DestroyDriver() {
            _driver.Quit();
        }

        #endregion
    }
}
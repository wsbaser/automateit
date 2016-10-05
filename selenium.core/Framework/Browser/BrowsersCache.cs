/**
 * Created by VolkovA on 26.02.14.
 */

namespace Selenium.Core.Framework.Browser
{
    using System.Collections.Generic;

    using Selenium.Core.Framework.Service;
    using Selenium.Core.Logging;

    public class BrowsersCache
    {
        private readonly Dictionary<BrowserType, Browser> _browsers;

        private readonly ITestLogger _log;

        private readonly Web _web;

        public BrowsersCache(Web web, ITestLogger log)
        {
            this._web = web;
            this._log = log;
            this._browsers = new Dictionary<BrowserType, Browser>();
        }

        public Browser GetBrowser(BrowserType browserType)
        {
            if (this._browsers.ContainsKey(browserType))
            {
                return this._browsers[browserType];
            }
            var browser = this.CreateBrowser(browserType);
            this._browsers.Add(browserType, browser);
            return browser;
        }

        private Browser CreateBrowser(BrowserType browserType)
        {
            var driverManager = this.getDriverFactory(browserType);
            return new Browser(this._web, this._log, driverManager);
        }

        private DriverManager getDriverFactory(BrowserType browserType)
        {
            switch (browserType)
            {
                case BrowserType.FIREFOX:
                    return new FirefoxDriverManager();
                case BrowserType.CHROME:
                    return new ChromeDriverFacrory();
                default:
                    return null;
            }
        }
    }
}
/**
 * Created by VolkovA on 26.02.14.
 */

using System.Collections.Generic;
using selenium.core.Framework.Service;
using selenium.core.Logging;

namespace selenium.core.Framework.Browser {
    public class BrowsersCache {
        private readonly Dictionary<BrowserType, Browser> _browsers;
        private readonly ITestLogger _log;
        private readonly Web _web;

        public BrowsersCache(Web web, ITestLogger log) {
            _web = web;
            _log = log;
            _browsers = new Dictionary<BrowserType, Browser>();
        }

        public Browser GetBrowser(BrowserType browserType) {
            if (_browsers.ContainsKey(browserType))
                return _browsers[browserType];
            Browser browser = CreateBrowser(browserType);
            _browsers.Add(browserType, browser);
            return browser;
        }

        private Browser CreateBrowser(BrowserType browserType) {
            DriverManager driverManager = getDriverFactory(browserType);
            return new Browser(_web, _log, driverManager);
        }

        private DriverManager getDriverFactory(BrowserType browserType) {
            switch (browserType) {
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
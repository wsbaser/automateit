/**
 * Created by VolkovA on 06.03.14.
 */

namespace Selenium.Core.Framework.Page
{
    using Selenium.Core.Framework.Browser;
    using Selenium.Core.Logging;

    public interface IPageObject
    {
        Browser Browser { get; }

        ITestLogger Log { get; }

        BrowserAction Action { get; }

        BrowserAlert Alert { get; }

        BrowserFind Find { get; }

        BrowserGet Get { get; }

        BrowserGo Go { get; }

        BrowserIs Is { get; }

        BrowserState State { get; }

        BrowserWait Wait { get; }

        BrowserJs Js { get; }

        BrowserWindow Window { get; }

        BrowserCookies Cookies { get; }
    }
}
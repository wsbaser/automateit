/**
 * Created by VolkovA on 27.02.14.
 */

using OpenQA.Selenium;

namespace selenium.core.Framework.Browser {
    public interface DriverManager {
        void InitDriver();
        IWebDriver GetDriver();
        void DestroyDriver();
    }
}
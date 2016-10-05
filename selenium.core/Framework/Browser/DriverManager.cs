/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Framework.Browser
{
    using OpenQA.Selenium;

    public interface DriverManager
    {
        void InitDriver();

        IWebDriver GetDriver();

        void DestroyDriver();
    }
}
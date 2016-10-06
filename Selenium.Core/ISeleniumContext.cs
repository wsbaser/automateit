namespace Selenium.Core
{
    using Selenium.Core.Framework.Browser;
    using Selenium.Core.Framework.Service;
    using Selenium.Core.Logging;

    public interface ISeleniumContext
    {
        Web Web { get; }

        ITestLogger Log { get; }

        Browser Browser { get; }
    }
}
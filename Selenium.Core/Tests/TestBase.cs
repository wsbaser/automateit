/**
 * Created by VolkovA on 26.02.14.
 */

namespace Selenium.Core.Tests
{
    using Selenium.Core.Framework.Browser;
    using Selenium.Core.Framework.Page;
    using Selenium.Core.Logging;

    public abstract class TestBase<P>
        where P : IPage
    {
        protected P Page { get; set; }

        protected abstract Browser Browser { get; }

        protected abstract ITestLogger Log { get; }

        protected void up()
        {
            this.Page = this.Browser.State.PageAs<P>();
        }
    }
}
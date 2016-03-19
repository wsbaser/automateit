/**
 * Created by VolkovA on 26.02.14.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using selenium.core.Framework.Browser;
using selenium.core.Framework.Page;
using selenium.core.Logging;

namespace selenium.core.Tests {
    [TestClass]
    public abstract class TestBase<P> where P : IPage {
        protected P Page { get; set; }

        protected abstract Browser Browser { get; }

        protected abstract TestLogger Log { get; }

        protected void up() {
            Page = Browser.State.PageAs<P>();
        }
    }
}
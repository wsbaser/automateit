using NUnit.Framework;
using selenium.widgets_ui_3._0.service.pages;
using selenium.widgets_ui_3._0.tests.@base;

namespace selenium.widgets_ui_3._0.tests
{
    [TestFixture]
    public class MyFirstTest : PageWithWidgetTestBase<WidgetPage>
    {
        [Test]
        public void Test1()
        {
            var page = GoToPageWithWidget(117166);
            Assert.IsNotNull(page);
        }
    }
}

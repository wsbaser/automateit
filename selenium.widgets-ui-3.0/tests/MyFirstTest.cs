using NUnit.Framework;
using selenium.widgets_ui_3._0.service.pages;

namespace selenium.widgets_ui_3._0.tests
{
    [TestFixture]
    public class MyFirstTest : PageWithWidgetTestBase<WidgetPage>
    {
        [Test]
        public void Test1()
       { 
            var page = Browser.Go.ToPage<WidgetPage>();
            Assert.IsNotNull(page);
        }
    }
}

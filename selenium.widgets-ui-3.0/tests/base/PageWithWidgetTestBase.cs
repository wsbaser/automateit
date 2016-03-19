using selenium.core.Framework.Browser;
using selenium.core.Framework.Page;
using selenium.core.Logging;

namespace selenium.widgets_ui_3._0.tests
{
    public abstract class PageWithWidgetTestBase<P> : PageTestBase<P> where P : IPage
    {
        protected override Browser Browser
        {
            get { return WidgetSeleniumContext.Inst.Browser; }
        }

        protected override TestLogger Log
        {
            get { return WidgetSeleniumContext.Inst.Log; }
        }
    }
}
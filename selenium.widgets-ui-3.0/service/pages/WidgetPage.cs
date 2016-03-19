using OpenQA.Selenium;
using selenium.core.Framework.Page;
using selenium.core.Framework.PageElements;

namespace selenium.widgets_ui_3._0.service.pages
{
    public class WidgetPage : PageWithWidgetBase
    {
        [WebComponent(".lt-label")]
        public LivetexWidget Widget { get; set; }
    }

    public class LivetexWidget:SimpleWebComponent
    {
        public LivetexWidget(IPage parent, string rootScss) : base(parent, rootScss)
        {
        }
    }
}

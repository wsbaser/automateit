using selenium.core.Framework.Page;
using selenium.core.Framework.PageElements;

namespace selenium.widget.v3.service.pages
{
    public abstract class PageWithWidget : SelfMatchingPageBase
    {
        public override string AbsolutePath
        {
            get { return ""; }
        }

        [WebComponent(".lt-label")]
        public LivetexWidgetLabel WidgetLabel { get; set; }
    }
}
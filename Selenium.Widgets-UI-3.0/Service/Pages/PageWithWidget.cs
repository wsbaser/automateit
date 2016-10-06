namespace Selenium.Widget.v3.Service.Pages
{
    using Selenium.Core.Framework.Page;
    using Selenium.Core.Framework.PageElements;

    public class PageWithWidget : SelfMatchingPageBase
    {
        public override string AbsolutePath
        {
            get
            {
                return "";
            }
        }

        [WebComponent(".lt-label")]
        public LivetexWidgetLabel WidgetLabel { get; set; }
    }
}
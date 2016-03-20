using selenium.core.Framework.Service;

namespace selenium.widget.v3.service
{
    internal class WidgetService : ServiceImpl
    {
        public WidgetService(BaseUrlInfo defaultBaseUrlInfo, BaseUrlPattern pattern, Router router)
            : base(defaultBaseUrlInfo, pattern, router)
        {
        }
    }
}
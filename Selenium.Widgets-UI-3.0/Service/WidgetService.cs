namespace Selenium.Widget.v3.Service
{
    using Selenium.Core.Framework.Service;

    internal class WidgetService : ServiceImpl
    {
        public WidgetService(BaseUrlInfo defaultBaseUrlInfo, BaseUrlPattern pattern, Router router)
            : base(defaultBaseUrlInfo, pattern, router)
        {
        }
    }
}
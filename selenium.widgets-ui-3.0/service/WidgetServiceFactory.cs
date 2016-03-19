/**
* Created by VolkovA on 28.02.14.
*/

using System.Collections.Generic;
using selenium.core.Framework.Service;

namespace selenium.widgets_ui_3._0.service
{
    public class WidgetsServiceFactory : ServiceFactory
    {
        private const string DEV_DOMAIN = "wsbaser.github.io";

        #region ServiceFactory Members

        public Router createRouter()
        {
            var router = new SelfMatchingPagesRouter();
            router.RegisterDerivedPages<PageWithWidgetBase>();
            return router;
        }

        public BaseUrlPattern createBaseUrlPattern()
        {
            var urlRegexBuilder = new BaseUrlRegexBuilder(new List<string> { DEV_DOMAIN });
            return new BaseUrlPattern(urlRegexBuilder.Build());
        }

        public BaseUrlInfo getDefaultBaseUrlInfo()
        {
            return new BaseUrlInfo(string.Empty, DEV_DOMAIN, "/");
        }

        public Service createService()
        {
            return new WidgetService(getDefaultBaseUrlInfo(), createBaseUrlPattern(), createRouter());
        }

        #endregion
    }
}
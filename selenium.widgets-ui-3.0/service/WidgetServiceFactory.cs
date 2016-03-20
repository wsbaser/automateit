/**
* Created by VolkovA on 28.02.14.
*/

using System.Collections.Generic;
using selenium.core.Framework.Service;
using selenium.widget.v3.service.pages;

namespace selenium.widget.v3.service
{
    public class WidgetsServiceFactory : ServiceFactory
    {
        private const string DEV_DOMAIN = "wsbaser.github.io";

        #region ServiceFactory Members

        public Router createRouter()
        {
            var router = new SelfMatchingPagesRouter();
            router.RegisterPage<PageWithWidget>();  // RegisterDerivedPages<PageWithWidget>();
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
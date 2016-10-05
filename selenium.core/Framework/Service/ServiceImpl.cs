/**
 * Created by VolkovA on 28.02.14.
 */

namespace Selenium.Core.Framework.Service
{
    using System;

    using Selenium.Core.Framework.Page;

    public abstract class ServiceImpl : Service
    {
        public ServiceImpl(BaseUrlInfo defaultBaseUrlInfo, BaseUrlPattern baseUrlPattern, Router router)
        {
            this.DefaultBaseUrlInfo = defaultBaseUrlInfo;
            this.BaseUrlPattern = baseUrlPattern;
            this.Router = router;
        }

        public IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo)
        {
            return this.Router.GetPage(requestData, baseUrlInfo);
        }

        public IPage GetEmailPage(Uri uri)
        {
            return this.Router.GetEmailPage(uri);
        }

        public RequestData GetRequestData(IPage page)
        {
            return this.Router.GetRequest(page, this.DefaultBaseUrlInfo);
        }

        #region Service Members

        public Router Router { get; }

        public BaseUrlPattern BaseUrlPattern { get; }

        public BaseUrlInfo DefaultBaseUrlInfo { get; }

        #endregion
    }
}
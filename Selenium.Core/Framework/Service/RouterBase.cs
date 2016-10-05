namespace Selenium.Core.Framework.Service
{
    using System;

    using Selenium.Core.Framework.Page;

    public abstract class RouterBase : Router
    {
        public abstract RequestData GetRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo);

        public abstract IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo);

        public abstract IPage GetEmailPage(Uri uri);

        public abstract bool HasPage(IPage page);
    }
}
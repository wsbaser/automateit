/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Framework.Service
{
    using System;

    using Selenium.Core.Framework.Page;

    public interface Router
    {
        RequestData GetRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo);

        IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo);

        IPage GetEmailPage(Uri uri);

        bool HasPage(IPage page);
    }
}
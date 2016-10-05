/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Framework.Service
{
    using Selenium.Core.Framework.Page;

    public interface RequestAction
    {
        IPage getPage(RequestData requestData, BaseUrlInfo baseUrlInfo);

        RequestData getRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo);
    }
}
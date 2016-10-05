/**
 * Created by VolkovA on 27.02.14.
 * Базовый класс для страниц со статичным Url
 */

namespace Selenium.Core.Framework.Page
{
    using Selenium.Core.Framework.Service;

    public interface ISelfMatchingPage
    {
        UriMatchResult Match(RequestData requestData, BaseUrlInfo baseUrlInfo);
    }

    public abstract class SelfMatchingPageBase : PageBase, ISelfMatchingPage
    {
        public abstract string AbsolutePath { get; }

        #region ISelfMatchingPage Members

        public virtual UriMatchResult Match(RequestData requestData, BaseUrlInfo baseUrlInfo)
        {
            return new UriMatcher(this.AbsolutePath, this.Data, this.Params).Match(
                requestData.Url,
                baseUrlInfo.AbsolutePath);
        }

        #endregion

        public virtual RequestData GetRequest(BaseUrlInfo defaultBaseUrlInfo)
        {
            var url =
                new UriAssembler(this.BaseUrlInfo, this.AbsolutePath, this.Data, this.Params).Assemble(
                    defaultBaseUrlInfo);
            return new RequestData(url);
        }
    }
}
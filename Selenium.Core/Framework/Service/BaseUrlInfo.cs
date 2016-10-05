/**
 * Created by VolkovA on 03.03.14.
 */

namespace Selenium.Core.Framework.Service
{
    public class BaseUrlInfo
    {
        public BaseUrlInfo(string subDomain, string domain, string absolutePath)
        {
            this.SubDomain = subDomain;
            this.Domain = domain;
            this.AbsolutePath = absolutePath;
        }

        public string SubDomain { get; }

        public string Domain { get; }

        public string AbsolutePath { get; }

        public BaseUrlInfo ApplyActual(BaseUrlInfo baseUrlInfo)
        {
            var subDomain = baseUrlInfo == null || string.IsNullOrEmpty(baseUrlInfo.SubDomain)
                                ? this.SubDomain
                                : baseUrlInfo.SubDomain;
            var domain = baseUrlInfo == null || string.IsNullOrEmpty(baseUrlInfo.Domain)
                             ? this.Domain
                             : baseUrlInfo.Domain;
            var absolutePath = baseUrlInfo == null || string.IsNullOrEmpty(baseUrlInfo.AbsolutePath)
                                   ? this.AbsolutePath
                                   : baseUrlInfo.AbsolutePath;
            return new BaseUrlInfo(subDomain, domain, absolutePath);
        }

        // Сформировать BaseUrl
        public string GetBaseUrl()
        {
            var s = this.Domain + this.AbsolutePath;
            if (!string.IsNullOrEmpty(this.SubDomain))
            {
                s = this.SubDomain + "." + s;
            }
            return s;
        }
    }
}
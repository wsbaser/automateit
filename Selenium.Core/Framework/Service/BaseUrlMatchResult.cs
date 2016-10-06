namespace Selenium.Core.Framework.Service
{
    public class BaseUrlMatchResult
    {
        public string AbsolutePath;

        public string Domain;

        public BaseUrlMatchLevel Level;

        public string SubDomain;

        public BaseUrlMatchResult(BaseUrlMatchLevel level, string subDomain, string domain, string absolutePath)
        {
            this.Level = level;
            this.SubDomain = subDomain;
            this.Domain = domain;
            this.AbsolutePath = absolutePath;
        }

        public static BaseUrlMatchResult Unmatched()
        {
            return new BaseUrlMatchResult(BaseUrlMatchLevel.Unmatched, null, null, null);
        }

        public BaseUrlInfo getBaseUrlInfo()
        {
            return new BaseUrlInfo(this.SubDomain, this.Domain, this.AbsolutePath);
        }
    }
}
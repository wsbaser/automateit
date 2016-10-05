/**
 * Created by VolkovA on 28.02.14.
 */

namespace Selenium.Core.Framework.Service
{
    using System.Collections.Generic;
    using System.Linq;

    public class BaseUrlRegexBuilder
    {
        private readonly string _domainPattern;

        private string _absolutePathPattern = "";

        private string _subDomainPattern = "((?<optionalsubdomain>[^\\.]+)\\.)?";

        public BaseUrlRegexBuilder(string domain)
            : this(new List<string> { domain })
        {
        }

        public BaseUrlRegexBuilder(List<string> domains)
        {
            this._domainPattern = this.GenerateDomainsPattern(domains);
        }

        private string GenerateDomainsPattern(List<string> domains)
        {
            var s = domains.Aggregate("", (current, domain) => current + domain + "|");
            s = s.Substring(0, s.Length - 1);
            s = s.Replace(".", "\\.");
            return string.Format("(?<domain>({0}))", s);
        }

        public void SetSubDomain(string value)
        {
            this._subDomainPattern = string.Format("(?<subdomain>{0})\\.", value);
        }

        public void SetAbsolutePathPattern(string pattern)
        {
            this._absolutePathPattern = string.Format("(?<abspath>\\/{0})", pattern);
        }

        // Сформировать Regex паттерн для BaseUrl сервиса
        public string Build()
        {
            return "(http(|s)://|)(www.|)" + this._subDomainPattern + this._domainPattern + this._absolutePathPattern
                   + ".*";
        }
    }
}
namespace Selenium.Core.Framework.Service
{
    using System.Text.RegularExpressions;

    public class BaseUrlPattern
    {
        private readonly string _regexPattern;

        public BaseUrlPattern(string regexPattern)
        {
            this._regexPattern = regexPattern;
        }

        // Соответствует ли указанный Url шаблону
        public BaseUrlMatchResult Match(string url)
        {
            var regex = new Regex(this._regexPattern);
            var match = regex.Match(url);
            if (!match.Success)
            {
                return BaseUrlMatchResult.Unmatched();
            }
            var domain = match.Groups["domain"].Value;
            var abspath = this.hasGroup(match, "abspath") ? match.Groups["abspath"].Value : "/";

            // У сервиса есть жестко заданный поддомен и он совпадает с поддоменом в Url
            if (this.hasGroup(match, "subdomain"))
            {
                return new BaseUrlMatchResult(
                    BaseUrlMatchLevel.FullDomain,
                    match.Groups["subdomain"].Value,
                    domain,
                    abspath);
            }

            var optionalsubdomain = match.Groups["optionalsubdomain"].Value;
            // У сервиса нет жестко заданного поддомена и в Url также нет поддомена
            if (string.IsNullOrEmpty(optionalsubdomain))
            {
                return new BaseUrlMatchResult(BaseUrlMatchLevel.FullDomain, null, domain, abspath);
            }

            // У сервиса нет жестко заданного поддомена, но в Url поддомен имеется
            return new BaseUrlMatchResult(BaseUrlMatchLevel.BaseDomain, optionalsubdomain, domain, abspath);
        }

        // Проверить, имеется ли группа в паттерне
        private bool hasGroup(Match match, string groupName)
        {
            return !string.IsNullOrEmpty(match.Groups[groupName].Value);
        }
    }
}
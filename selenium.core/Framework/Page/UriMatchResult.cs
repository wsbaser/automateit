namespace Selenium.Core.Framework.Page
{
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using OpenQA.Selenium;

    public class UriMatchResult
    {
        public UriMatchResult(bool success)
            : this(success, new Dictionary<string, string>())
        {
        }

        public UriMatchResult(bool success, Dictionary<string, string> data)
            : this(success, data, new StringDictionary())
        {
        }

        public UriMatchResult(bool success, Dictionary<string, string> data, StringDictionary _params)
            : this(success, data, _params, new List<Cookie>())
        {
        }

        public UriMatchResult(
            bool success,
            Dictionary<string, string> data,
            StringDictionary _params,
            List<Cookie> cookies)
        {
            this.Success = success;
            this.Data = data;
            this.Cookies = cookies;
            this.Params = _params;
        }

        public bool Success { get; private set; }

        public Dictionary<string, string> Data { get; private set; }

        public List<Cookie> Cookies { get; private set; }

        public StringDictionary Params { get; set; }

        public static UriMatchResult Matched()
        {
            return new UriMatchResult(true);
        }

        public static UriMatchResult Unmatched()
        {
            return new UriMatchResult(false);
        }
    }
}
namespace Selenium.Core.Framework.Page
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using global::Core.Extensions;

    public class UriMatcher
    {
        private readonly string _pageAbsolutePath;

        private readonly Dictionary<string, string> _pageData;

        private readonly StringDictionary _pageParams;

        public UriMatcher(string pageAbsolutePath, Dictionary<string, string> pageData, StringDictionary pageParams)
        {
            this._pageAbsolutePath = pageAbsolutePath;
            this._pageData = pageData;
            this._pageParams = pageParams;
        }

        public UriMatchResult Match(Uri uri, string siteAbsolutePath)
        {
            var realPath = uri.AbsolutePath.Substring(siteAbsolutePath.Length);

            var pageArr = this._pageAbsolutePath.Split('/');
            var realArr = realPath.Split('/');
            if (pageArr.Length != realArr.Length)
            {
                return UriMatchResult.Unmatched();
            }

            // Сравнить адрес страницы
            var actualData = new Dictionary<string, string>();
            for (var i = 0; i < pageArr.Length; i++)
            {
                var pageArrItem = pageArr[i];
                var realArrItem = realArr[i];
                if (pageArrItem.StartsWith("{") && pageArrItem.EndsWith("}"))
                {
                    var paramName = pageArrItem.Substring(1, pageArrItem.Length - 2);
                    actualData[paramName] = realArrItem;
                }
                else if (string.Compare(pageArrItem, realArrItem, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return UriMatchResult.Unmatched();
                }
            }

            // Извлечь список параметров
            var actualParams = new StringDictionary();
            var queryParamsArr = uri.Query.CutFirst('?').Split('&');
            foreach (var queryParam in queryParamsArr)
            {
                var keyvalue = queryParam.Split('=');
                if (keyvalue.Length < 2)
                {
                    continue;
                }
                actualParams.Add(keyvalue[0], keyvalue[1]);
            }

            // Сравнение Data
            if (this._pageData != null)
            {
                foreach (var key in this._pageData.Keys)
                {
                    if (!actualData.ContainsKey(key)
                        || string.Compare(actualData[key], this._pageData[key], StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        return UriMatchResult.Unmatched();
                    }
                }
            }

            // Сравнение Params
            if (this._pageParams != null)
            {
                foreach (string key in this._pageParams.Keys)
                {
                    if (!actualParams.ContainsKey(key)
                        || string.Compare(actualParams[key], this._pageParams[key], StringComparison.OrdinalIgnoreCase)
                        != 0)
                    {
                        return UriMatchResult.Unmatched();
                    }
                }
            }

            return new UriMatchResult(true, actualData, actualParams);
        }
    }
}
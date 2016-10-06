namespace Selenium.Core.Framework.Page
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    using global::Core.Extensions;

    using Selenium.Core.Framework.Service;

    public class UriAssembler
    {
        private readonly string _absolutePath;

        private readonly BaseUrlInfo _baseUrlInfo;

        private readonly Dictionary<string, string> _data;

        private readonly StringDictionary _params;

        public UriAssembler(
            BaseUrlInfo baseUrlInfo,
            string absolutePath,
            Dictionary<string, string> data,
            StringDictionary @params)
        {
            this._baseUrlInfo = baseUrlInfo;
            this._absolutePath = absolutePath;
            this._data = data;
            this._params = @params;
        }

        public string Assemble(BaseUrlInfo defaultBaseUrlInfo)
        {
            var url = string.Format("http://{0}{1}", this.GetBaseUrl(defaultBaseUrlInfo), this.GetPath());
            var query = this.GetQuery();
            if (string.IsNullOrEmpty(query))
            {
                url += "?" + query;
            }
            return url;
        }

        /// <summary>
        ///     —формировать строку с параметрами Url
        /// </summary>
        private string GetQuery()
        {
            if (this._params == null)
            {
                return string.Empty;
            }
            var query = this._params.Keys.Cast<string>()
                .Aggregate(string.Empty, (current, key) => current + key + "=" + this._params[key] + "&");
            return query.CutLast('&');
        }

        /// <summary>
        ///     ѕодставить параметры из Data в адрес страницы
        /// </summary>
        private string GetPath()
        {
            if (this._data == null)
            {
                return this._absolutePath;
            }
            var path = this._absolutePath;
            foreach (var key in this._data.Keys)
            {
                var param = "{" + key + "}";
                path = path.Replace(param, this._data[key]);
            }
            return path;
        }

        private string GetBaseUrl(BaseUrlInfo defaultBaseUrlInfo)
        {
            return defaultBaseUrlInfo.ApplyActual(this._baseUrlInfo).GetBaseUrl();
        }
    }
}
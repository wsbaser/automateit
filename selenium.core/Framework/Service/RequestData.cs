/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Framework.Service
{
    using System;
    using System.Collections.Generic;

    using OpenQA.Selenium;

    public class RequestData
    {
        public RequestData(string url)
            : this(url, new List<Cookie>())
        {
        }

        public RequestData(string url, List<Cookie> cookies)
        {
            this.Url = new Uri(url);
            this.Cookies = cookies;
        }

        public Uri Url { get; private set; }

        public List<Cookie> Cookies { get; private set; }
    }
}
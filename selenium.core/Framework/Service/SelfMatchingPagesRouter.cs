/**
 * Created by VolkovA on 28.02.14.
 */

namespace Selenium.Core.Framework.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Selenium.Core.Framework.Page;

    public class SelfMatchingPagesRouter : RouterBase
    {
        private readonly Dictionary<Type, ISelfMatchingPage> _pages;

        private readonly List<IEmailPage> _savedPages;

        public SelfMatchingPagesRouter()
        {
            this._pages = new Dictionary<Type, ISelfMatchingPage>();
            this._savedPages = new List<IEmailPage>();
        }

        public override RequestData GetRequest(IPage page, BaseUrlInfo defaultBaseUrlInfo)
        {
            var selfMatchingPage = page as SelfMatchingPageBase;
            if (selfMatchingPage == null)
            {
                return null;
            }
            return selfMatchingPage.GetRequest(defaultBaseUrlInfo);
        }

        public override IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo)
        {
            foreach (var dummyPage in this._pages.Values)
            {
                var match = dummyPage.Match(requestData, baseUrlInfo);
                if (match.Success)
                {
                    var instance = (SelfMatchingPageBase)Activator.CreateInstance(dummyPage.GetType());
                    instance.BaseUrlInfo = baseUrlInfo;
                    instance.Data = match.Data;
                    instance.Params = match.Params;
                    instance.Cookies = match.Cookies;
                    return instance;
                }
            }
            return null;
        }

        public override IPage GetEmailPage(Uri uri)
        {
            return (IPage)this._savedPages.FirstOrDefault(p => p.Match(uri));
        }

        public override bool HasPage(IPage page)
        {
            return this._pages.ContainsKey(page.GetType());
        }

        //        public void RegisterDerivedPages<T>() where T : SelfMatchingPageBase {
        //            Type superType = typeof (T);
        //            Assembly assembly = superType.GetTypeInfo().Assembly;
        //            IEnumerable<Type> derivedTypes =
        //                assembly.DefinedTypes.AsEnumerable().Where(t => !t.GetTypeInfo().IsAbstract && superType.IsAssignableFrom(t));
        //            foreach (Type derivedType in derivedTypes)
        //                RegisterPage(derivedType);
        //        }

        public void RegisterPage<T>()
        {
            this.RegisterPage(typeof(T));
        }

        private void RegisterPage(Type pageType)
        {
            var pageInstance = (ISelfMatchingPage)Activator.CreateInstance(pageType);
            this._pages.Add(pageType, pageInstance);
        }

        public void RegisterEmailPage<T>() where T : IEmailPage
        {
            var pageInstance = (IEmailPage)Activator.CreateInstance(typeof(T));
            this._savedPages.Add(pageInstance);
        }
    }
}
/**
 * Created by VolkovA on 27.02.14.
 * Коллекция поддерживаемых сервисов
 */

namespace Selenium.Core.Framework.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Selenium.Core.Framework.Page;

    public class Web
    {
        // Список зарегистрированных сервисов
        private readonly List<Service> _services;

        public Web()
        {
            this._services = new List<Service>();
        }

        // Определение сервиса, который должен обработать запрос(DNS маршрутизация и маршрутизация внутри домена)
        public ServiceMatchResult MatchService(RequestData request)
        {
            ServiceMatchResult baseDomainMatch = null;
            foreach (var service in this._services)
            {
                var baseUrlPattern = service.BaseUrlPattern;
                var result = baseUrlPattern.Match(request.Url.OriginalString);
                if (result.Level == BaseUrlMatchLevel.FullDomain)
                {
                    return new ServiceMatchResult(service, result.getBaseUrlInfo());
                }
                if (result.Level == BaseUrlMatchLevel.BaseDomain)
                {
                    if (baseDomainMatch != null)
                    {
                        throw new Exception(string.Format("Two BaseDomain matches for url {0}", request.Url));
                    }
                    baseDomainMatch = new ServiceMatchResult(service, result.getBaseUrlInfo());
                }
            }
            return baseDomainMatch;
        }

        // Поиск страницы в зарегистрированных сервисах
        // и получение ее Url
        public RequestData GetRequestData(IPage page)
        {
            var service = this._services.FirstOrDefault(s => s.Router.HasPage(page));
            if (service == null)
            {
                throw new PageNotRegisteredException(page);
            }
            return service.Router.GetRequest(page, service.DefaultBaseUrlInfo);
        }

        // Зарегистрировать сервис
        public void RegisterService(ServiceFactory serviceFactory)
        {
            var service = serviceFactory.createService();
            this._services.Add(service);
        }

        public IPage GetEmailPage(Uri uri)
        {
            foreach (var service in this._services)
            {
                var emailPage = service.GetEmailPage(uri);
                if (emailPage != null)
                {
                    return emailPage;
                }
            }
            return null;
        }
    }
}
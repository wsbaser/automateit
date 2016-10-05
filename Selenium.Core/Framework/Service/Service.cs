﻿/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Framework.Service
{
    using System;

    using Selenium.Core.Framework.Page;

    public interface Service
    {
        // Роутер сервиса
        Router Router { get; }

        // Получить паттерн, определяющий каким Url соответствует сервис
        BaseUrlPattern BaseUrlPattern { get; }

        // Получить параметры BaseUrl по умолчанию
        BaseUrlInfo DefaultBaseUrlInfo { get; }

        // Получить экземпляр класса страницы для указанного запроса
        IPage GetPage(RequestData requestData, BaseUrlInfo baseUrlInfo);

        IPage GetEmailPage(Uri uri);

        // Получить запрос для указанного экземпляра класса страницы
        RequestData GetRequestData(IPage page);
    }
}
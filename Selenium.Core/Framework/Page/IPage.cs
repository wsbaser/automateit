/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Framework.Page
{
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using OpenQA.Selenium;

    using Selenium.Core.Framework.Browser;
    using Selenium.Core.Framework.Service;
    using Selenium.Core.Logging;

    public interface IPage : IPageObject
    {
        /// <summary>
        ///     ����, ������� ������� �� ���������� ��������
        /// </summary>
        new List<Cookie> Cookies { get; set; }

        /// <summary>
        ///     ������, ������������ � Url
        /// </summary>
        Dictionary<string, string> Data { get; set; }

        /// <summary>
        ///     ���������, ������������ � Url
        /// </summary>
        StringDictionary Params { get; set; }

        /// <summary>
        ///     ���������� � ������, ��������� � ���������� ����
        /// </summary>
        BaseUrlInfo BaseUrlInfo { get; set; }

        List<IHtmlAlert> Alerts { get; }

        /// <summary>
        ///     ������ ������������������ ����������
        /// </summary>
        List<IProgressBar> ProgressBars { get; }

        /// <summary>
        ///     ���������������� ���������
        /// </summary>
        void RegisterComponent(IComponent component);

        /// <summary>
        ///     ���������������� ���������
        /// </summary>
        T RegisterComponent<T>(string componentName, params object[] args) where T : IComponent;

        /// <summary>
        ///     ������� ���������
        /// </summary>
        T CreateComponent<T>(params object[] args) where T : IComponent;

        /// <summary>
        ///     �������������� ��������
        /// </summary>
        void Activate(Browser browser, ITestLogger log);
    }
}
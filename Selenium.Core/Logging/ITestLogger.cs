/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Logging
{
    using System;

    using OpenQA.Selenium;

    public interface ITestLogger
    {
        /// <summary>
        ///     Залогировать действие
        /// </summary>
        void Action(string msg, params object[] args);

        /// <summary>
        ///     Залогировать информационное сообщение
        /// </summary>
        void Info(string msg, params object[] args);

        /// <summary>
        ///     Залогировать критическуюж ошибку
        /// </summary>
        void FatalError(string s, Exception e);

        /// <summary>
        ///     Сохранить значение в лог
        /// </summary>
        void WriteValue(string key, object value);

        /// <summary>
        ///     Прочитать сохраненное ранее значение из логи
        /// </summary>
        T GetValue<T>(string userphone);

        /// <summary>
        ///     Залогировать селектор
        /// </summary>
        void Selector(By by);

        /// <summary>
        ///     Залогировать исключение
        /// </summary>
        void Exception(Exception exception);
    }
}
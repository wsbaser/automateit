namespace Selenium.Core.Framework.Page
{
    using System.Collections.Generic;

    using OpenQA.Selenium;

    public interface IWebList<T> : IWebList
        where T : IItem
    {
        string ItemIdScss { get; }

        List<string> GetIds();

        List<T> GetItems();
    }

    public interface IWebList
    {
        List<T> GetItems<T>();
    }

    public interface IContainer : IComponent
    {
        string InnerScss(string relativeScss, params object[] args);
    }

    public interface IDropList
    {
        string ItemNameScss { get; }

        List<string> GetItems();

        By GetItemSelector(string name);
    }
}
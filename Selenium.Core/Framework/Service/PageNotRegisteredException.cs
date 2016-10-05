namespace Selenium.Core.Framework.Service
{
    using System;

    using Selenium.Core.Framework.Page;

    public class PageNotRegisteredException : Exception
    {
        public PageNotRegisteredException(IPage page)
            : base(string.Format("There are not services with registered page of type {0}", page.GetType().Name))
        {
        }
    }
}
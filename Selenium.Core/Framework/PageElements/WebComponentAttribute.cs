namespace Selenium.Core.Framework.PageElements
{
    using System;

    public class WebComponentAttribute : Attribute, IComponentAttribute
    {
        public WebComponentAttribute(params object[] args)
        {
            this.Args = args;
        }

        #region IComponentArgs Members

        public object[] Args { get; }

        public string ComponentName { get; set; }

        #endregion
    }
}
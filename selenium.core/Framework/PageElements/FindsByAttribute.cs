namespace Selenium.Core.Framework.PageElements
{
    using System;

    using OpenQA.Selenium;

    using Selenium.Core.SCSS;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class FindsByAttribute : Attribute
    {
        private By _finder;

        public string Name { get; set; }

        public string TagName { get; set; }

        public string Css { get; set; }

        public string ClassName { get; set; }

        public string ID { get; set; }

        public string XPath { get; set; }

        public string LinkText { get; set; }

        public string PartialLinkText { get; set; }

        public string Scss { get; set; }

        internal By Finder
        {
            get
            {
                return this._finder ?? (this._finder = this.CreateFinder());
            }
        }

        private By CreateFinder()
        {
            By by = null;
            if (this.SelectorNotEmpty(this.Name, by))
            {
                by = By.Name(this.Name);
            }
            if (this.SelectorNotEmpty(this.TagName, by))
            {
                by = By.TagName(this.TagName);
            }
            if (this.SelectorNotEmpty(this.Css, by))
            {
                by = By.CssSelector(this.Css);
            }
            if (this.SelectorNotEmpty(this.ClassName, by))
            {
                by = By.ClassName(this.ClassName);
            }
            if (this.SelectorNotEmpty(this.ID, by))
            {
                by = By.Id(this.ID);
            }
            if (this.SelectorNotEmpty(this.XPath, by))
            {
                by = By.XPath(this.XPath);
            }
            if (this.SelectorNotEmpty(this.LinkText, by))
            {
                by = By.LinkText(this.LinkText);
            }
            if (this.SelectorNotEmpty(this.PartialLinkText, by))
            {
                by = By.PartialLinkText(this.PartialLinkText);
            }
            if (this.SelectorNotEmpty(this.Scss, by))
            {
                by = ScssBuilder.CreateBy(this.Scss);
            }
            if (by == null)
            {
                throw new Exception("No one selector is defined");
            }
            return by;
        }

        private bool SelectorNotEmpty(string value, By by)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            if (by != null)
            {
                throw new Exception("More than one selector defined");
            }
            return true;
        }
    }
}
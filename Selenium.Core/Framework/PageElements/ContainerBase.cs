namespace Selenium.Core.Framework.PageElements
{
    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;
    using Selenium.Core.SCSS;

    public abstract class ContainerBase : ComponentBase, IContainer
    {
        private string _rootScss;

        protected ContainerBase(IPage parent)
            : this(parent, null)
        {
        }

        protected ContainerBase(IPage parent, string rootScss)
            : base(parent)
        {
            this._rootScss = rootScss;
        }

        protected virtual string RootScss
        {
            get
            {
                return this._rootScss ?? (this._rootScss = "html");
            }
        }

        protected By RootSelector
        {
            get
            {
                return ScssBuilder.CreateBy(this.RootScss);
            }
        }

        public override bool IsVisible()
        {
            return this.Is.Visible(this.RootSelector);
        }

        /// <summary>
        ///     Получает Scss для вложенного элемента
        /// </summary>
        public string InnerScss(string relativeScss, params object[] args)
        {
            relativeScss = string.Format(relativeScss, args);
            return ScssBuilder.Concat(this.RootScss, relativeScss).Value;
        }

        /// <summary>
        ///     Получает селектор для вложенного элемента
        /// </summary>
        public By InnerSelector(string relativeScss, params object[] args)
        {
            relativeScss = string.Format(relativeScss, args);
            return ScssBuilder.Concat(this.RootScss, relativeScss).By;
        }

        /// <summary>
        ///     Получает селектор для вложенного элемента
        /// </summary>
        public By InnerSelector(Scss innerScss)
        {
            var rootScss = ScssBuilder.Create(this.RootScss);
            return rootScss.Concat(innerScss).By;
        }
    }
}
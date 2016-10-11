namespace Selenium.Core.Framework.PageElements
{
    using Selenium.Core.Framework.Page;

    /// <summary>
    ///     Базовый класс для элемента страницы
    /// </summary>
    public abstract class ItemBase : ContainerBase, IItem
    {
        protected readonly IContainer Container;

        protected ItemBase(IContainer container, string id)
            : base(container.ParentPage)
        {
            this.Container = container;
            this.ID = id;
        }

        protected override string RootScss
        {
            get
            {
                return this.ItemScss;
            }
        }

        protected string ContainerInnerScss(string relativeXpath, params object[] args)
        {
            return this.Container.InnerScss(relativeXpath, args);
        }

        #region IItem Members

        public string ID { get; }

        public abstract string ItemScss { get; }

        #endregion
    }
}
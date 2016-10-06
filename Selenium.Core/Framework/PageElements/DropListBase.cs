namespace Selenium.Core.Framework.PageElements
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;

    public abstract class DropListBase : ContainerBase, IDropList
    {
        public DropListBase(IPage parent, string rootScss)
            : base(parent, rootScss)
        {
        }

        public abstract By GetItemSelector(string name);

        public abstract string ItemNameScss { get; }

        public List<string> GetItems()
        {
            return this.Get.Texts(this.ItemNameScss);
        }

        public void AssertContains(string item)
        {
            Assert.IsTrue(this.Contains(item));
        }

        /// <summary>
        ///     Содержит ли список указанное значение
        /// </summary>
        private bool Contains(string item)
        {
            return this.GetItems().Contains(item);
        }
    }
}
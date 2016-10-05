namespace Selenium.Core.Framework.PageElements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Core.Extensions;

    using Selenium.Core.Framework.Page;

    public abstract class ListBase<T> : ContainerBase, IWebList<T>
        where T : IItem, IComponent
    {
        public ListBase(IPage parent, string rootScss = null)
            : base(parent, rootScss)
        {
        }

        public List<T1> GetItems<T1>()
        {
            return this.GetItems().Cast<T1>().ToList();
        }

        public override bool IsVisible()
        {
            return this.Is.Visible(this.RootScss);
        }

        /// <summary>
        ///     Найти первый элемент удовлетворяющий условию фильтра
        /// </summary>
        /// <param name="filter">функция проверки соответствия элемента определенному условию</param>
        public T FindRandom(Func<T, bool> filter = null)
        {
            var list = this.GetItems();
            list.Shuffle();
            return filter == null ? list.FirstOrDefault() : list.FirstOrDefault(filter);
        }

        public T FindSingle(Func<T, bool> filter)
        {
            var list = this.GetItems();
            return list.SingleOrDefault(filter);
        }

        /// <summary>
        ///     Получить произвольный элемент списка
        /// </summary>
        public T RandomItem()
        {
            return this.GetItems().RandomItem();
        }

        #region IWebList<T> Members

        public abstract string ItemIdScss { get; }

        public virtual List<string> GetIds()
        {
            return this.Get.Texts(this.ItemIdScss);
        }

        public virtual List<T> GetItems()
        {
            return this.GetIds().Select(id => (T)WebPageBuilder.CreateComponent<T>(this, id)).ToList();
        }

        #endregion
    }
}
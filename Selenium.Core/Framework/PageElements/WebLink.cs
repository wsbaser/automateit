namespace Selenium.Core.Framework.PageElements
{
    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;

    public class WebLink : SimpleWebComponent, IClickable
    {
        public WebLink(IPage parent, By @by)
            : base(parent, @by)
        {
        }

        public string AnchorText
        {
            get
            {
                return this.Get.Text(this.By);
            }
        }

        #region IClickable Members

        /// <summary>
        ///     Выполнить клик по ссылке и подождать редиректа
        /// </summary>
        public void Click(int sleepTimeout = 0)
        {
            this.Log.Action("Клик по ссылке '{0}'", this.ComponentName);
            this.Action.ClickAndWaitForRedirect(this.By);
        }

        #endregion

        /// <summary>
        ///     Выполнить клик по ссылке и дождаться выполнения Ajax запросов
        /// </summary>
        public void ClickAndWaitWhileAjax(bool ajaxInevitable = false)
        {
            this.Log.Action("Клик по ссылке '{0}'", this.ComponentName);
            this.Action.ClickAndWaitWhileAjax(this.By, ajaxInevitable: ajaxInevitable);
        }

        /// <summary>
        ///     Навести курсор на ссылку
        /// </summary>
        public void MouseOver(int sleepTimeout = 0)
        {
            this.Log.Action("Наведение курсора на ссылку '{0}'", this.ComponentName);
            this.Action.MouseOver(this.By, sleepTimeout);
        }
    }
}
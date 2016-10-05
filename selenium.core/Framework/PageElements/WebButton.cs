namespace Selenium.Core.Framework.PageElements
{
    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;

    public class WebButton : SimpleWebComponent, IClickable
    {
        public WebButton(IPage parent, By by)
            : base(parent, by)
        {
        }

        #region IClickable Members

        /// <summary>
        ///     Выполнить клик по кнопке
        /// </summary>
        public void Click(int sleepTimeout = 0)
        {
            this.Log.Action("Клик по кнопке '{0}'", this.ComponentName);
            this.Action.Click(this.By, sleepTimeout);
        }

        #endregion

        public void ClickAndWaitWhileAjax(int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            this.Log.Action("Клик по кнопке '{0}'", this.ComponentName);
            this.Action.ClickAndWaitWhileAjax(this.By, sleepTimeout, ajaxInevitable);
        }

        public void ClickAndWaitForRedirect()
        {
            this.Log.Action("Клик по кнопке '{0}'", this.ComponentName);
            this.Action.ClickAndWaitForRedirect(this.By);
        }

        public void ClickAndWaitWhileProgress()
        {
            this.Log.Action("Клик по кнопке '{0}'", this.ComponentName);
            this.Action.ClickAndWaitWhileProgress(this.By, 1000);
        }
    }
}
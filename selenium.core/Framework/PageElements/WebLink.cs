using OpenQA.Selenium;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
    public class WebLink : SimpleWebComponent, IClickable {
        public WebLink(IPage parent, By @by)
            : base(parent, @by) {
        }

        public string AnchorText {
            get { return Get.Text(By); }
        }

        #region IClickable Members

        /// <summary>
        /// Выполнить клик по ссылке и подождать редиректа
        /// </summary>
        public void Click(int sleepTimeout=0) {
            Log.Action("Клик по ссылке '{0}'", ComponentName);
            Action.ClickAndWaitForRedirect(By);
        }

        #endregion

        /// <summary>
        /// Выполнить клик по ссылке и дождаться выполнения Ajax запросов
        /// </summary>
        public void ClickAndWaitWhileAjax(bool ajaxInevitable=false) {
            Log.Action("Клик по ссылке '{0}'", ComponentName);
            Action.ClickAndWaitWhileAjax(By, ajaxInevitable: ajaxInevitable);
        }

        /// <summary>
        /// Навести курсор на ссылку
        /// </summary>
        public void MouseOver(int sleepTimeout = 0) {
            Log.Action("Наведение курсора на ссылку '{0}'", ComponentName);
            Action.MouseOver(By, sleepTimeout);
        }
    }
}
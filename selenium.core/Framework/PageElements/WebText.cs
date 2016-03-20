using NUnit.Framework;
using OpenQA.Selenium;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
    public class WebText : SimpleWebComponent {
        public WebText(IPage parent, By by)
            : base(parent, by) {
        }

        public WebText(IPage parent, string rootScss)
            : base(parent, rootScss) {
        }

        public string Text {
            get { return Get.Text(By); }
        }

        public void AssertIsVisible() {
            Assert.IsTrue(IsVisible(),"{0} не отображается",ComponentName);
        }

        public void AssertIsHidden() {
            Assert.IsFalse(IsVisible(), "{0} отображается", ComponentName);
        }

        public void AssertEqual(string expected) {
            Assert.AreEqual(expected, Text, "Текст компонента '{0}' не соответствует ожидаемому", ComponentName);
        }

        public void Click(int sleepTimeout=0) {
            Log.Action("Клик по псевдоссылке '{0}'", ComponentName);
            Action.Click(By, sleepTimeout);
        }

        public void ClickAndWaitWhileAjaxRequests() {
            Log.Action("Клик по псевдоссылке '{0}'", ComponentName);
            Action.ClickAndWaitWhileAjax(By);
        }

        public void ClickAndWaitForRedirect() {
            Log.Action("Клик по псевдоссылке '{0}'", ComponentName);
            Action.ClickAndWaitForRedirect(By);
        }

        public T Value<T>() {
            return Get.Value<T>(By);
        }

        public void DragByHorizontal(int pixels) {
            Log.Action("Drag '{0}' at {1} pixels", ComponentName, pixels);
            Action.DragByHorizontal(By, pixels);
        }
    }
}
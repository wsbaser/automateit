namespace Selenium.Core.Framework.PageElements
{
    using NUnit.Framework;

    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;

    public class WebText : SimpleWebComponent
    {
        public WebText(IPage parent, By by)
            : base(parent, by)
        {
        }

        public WebText(IPage parent, string rootScss)
            : base(parent, rootScss)
        {
        }

        public string Text
        {
            get
            {
                return this.Get.Text(this.By);
            }
        }

        public void AssertIsVisible()
        {
            Assert.IsTrue(this.IsVisible(), "{0} не отображается", this.ComponentName);
        }

        public void AssertIsHidden()
        {
            Assert.IsFalse(this.IsVisible(), "{0} отображается", this.ComponentName);
        }

        public void AssertEqual(string expected)
        {
            Assert.AreEqual(
                expected,
                this.Text,
                "Текст компонента '{0}' не соответствует ожидаемому",
                this.ComponentName);
        }

        public void Click(int sleepTimeout = 0)
        {
            this.Log.Action("Клик по псевдоссылке '{0}'", this.ComponentName);
            this.Action.Click(this.By, sleepTimeout);
        }

        public void ClickAndWaitWhileAjaxRequests()
        {
            this.Log.Action("Клик по псевдоссылке '{0}'", this.ComponentName);
            this.Action.ClickAndWaitWhileAjax(this.By);
        }

        public void ClickAndWaitForRedirect()
        {
            this.Log.Action("Клик по псевдоссылке '{0}'", this.ComponentName);
            this.Action.ClickAndWaitForRedirect(this.By);
        }

        public T Value<T>()
        {
            return this.Get.Value<T>(this.By);
        }

        public void DragByHorizontal(int pixels)
        {
            this.Log.Action("Drag '{0}' at {1} pixels", this.ComponentName, pixels);
            this.Action.DragByHorizontal(this.By, pixels);
        }
    }
}
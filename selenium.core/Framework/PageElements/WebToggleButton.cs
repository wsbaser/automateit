namespace Selenium.Core.Framework.PageElements
{
    using NUnit.Framework;

    using Selenium.Core.Framework.Page;
    using Selenium.Core.SCSS;

    public class WebToggleButton : WebButton
    {
        private readonly string _selectedClass;

        public WebToggleButton(IPage parent, string scssSelector, string selectedClass)
            : base(parent, ScssBuilder.CreateBy(scssSelector))
        {
            this._selectedClass = selectedClass;
        }

        public bool IsActive()
        {
            return this.Is.HasClass(this.By, this._selectedClass);
        }

        public void SelectAndWaitWhileAjax(int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            if (!this.IsActive())
            {
                this.ClickAndWaitWhileAjax(sleepTimeout, ajaxInevitable);
            }
        }

        public void Select(int sleepTimeout = 0)
        {
            if (!this.IsActive())
            {
                this.Click(sleepTimeout);
            }
        }

        public void AssertIsSelected()
        {
            Assert.IsTrue(this.IsActive(), "'{0}' is not selected", this.ComponentName);
        }
    }
}
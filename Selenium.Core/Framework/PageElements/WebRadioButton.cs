namespace Selenium.Core.Framework.PageElements
{
    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;
    using Selenium.Core.SCSS;

    public class WebRadioButton : SimpleWebComponent
    {
        public WebRadioButton(IPage parent, By @by)
            : base(parent, @by)
        {
        }

        public virtual void SelectAndWaitWhileAjaxRequests(int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            this.Log.Action("Select radio button '{0}'", this.ComponentName);
            this.Action.ClickAndWaitWhileAjax(this.By, sleepTimeout, ajaxInevitable);
        }

        public virtual void Select(int sleepTimeout = 0)
        {
            this.Log.Action("Select radio button '{0}'", this.ComponentName);
            this.Action.Click(this.By, sleepTimeout);
        }
    }

    public class BeelineRadio : WebRadioButton
    {
        public readonly By LabelBy;

        public BeelineRadio(IPage parent, string inputScss, string labelScss)
            : base(parent, Scss.GetBy(inputScss))
        {
            this.LabelBy = Scss.GetBy(labelScss);
        }

        public override void SelectAndWaitWhileAjaxRequests(int sleepTimeout = 0, bool ajaxInevitable = false)
        {
            this.Log.Action("Select radio button '{0}'", this.ComponentName);
            // input is not displayed. hence will make click by label
            this.Action.ClickAndWaitWhileAjax(this.LabelBy, sleepTimeout, ajaxInevitable);
        }

        public override void Select(int sleepTimeout = 0)
        {
            this.Log.Action("Select radio button '{0}'", this.ComponentName);
            // input is not displayed. hence will make click by label
            this.Action.Click(this.LabelBy, sleepTimeout);
        }
    }
}
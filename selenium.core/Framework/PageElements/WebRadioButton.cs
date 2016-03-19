using OpenQA.Selenium;
using selenium.core.Framework.Page;
using selenium.core.SCSS;

namespace selenium.core.Framework.PageElements {
    public class WebRadioButton : SimpleWebComponent {
        public WebRadioButton(IPage parent, By @by)
            : base(parent, @by) {
        }

        public virtual void SelectAndWaitWhileAjaxRequests(int sleepTimeout=0, bool ajaxInevitable=false) {
            Log.Action("Select radio button '{0}'", ComponentName);
            Action.ClickAndWaitWhileAjax(By, sleepTimeout, ajaxInevitable);
        }

        public virtual void Select(int sleepTimeout = 0) {
            Log.Action("Select radio button '{0}'", ComponentName);
            Action.Click(By, sleepTimeout);
        }
    }

    public class BeelineRadio:WebRadioButton {
        public readonly By LabelBy;

        public BeelineRadio(IPage parent, string inputScss, string labelScss) : base(parent, Scss.GetBy(inputScss)) {
            LabelBy = Scss.GetBy(labelScss);
        }

        public override void SelectAndWaitWhileAjaxRequests(int sleepTimeout = 0, bool ajaxInevitable = false) {
            Log.Action("Select radio button '{0}'", ComponentName);
            // input is not displayed. hence will make click by label
            Action.ClickAndWaitWhileAjax(LabelBy, sleepTimeout, ajaxInevitable);
        }

        public override void Select(int sleepTimeout = 0) {
            Log.Action("Select radio button '{0}'", ComponentName);
            // input is not displayed. hence will make click by label
            Action.Click(LabelBy, sleepTimeout);
        }
    }
}
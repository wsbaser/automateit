namespace Selenium.Core.Framework.PageElements
{
    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;
    using Selenium.Core.SCSS;

    public abstract class SimpleWebComponent : ContainerBase
    {
        public readonly By By;

        protected SimpleWebComponent(IPage parent, By by)
            : base(parent)
        {
            this.By = by;
        }

        protected SimpleWebComponent(IPage parent, string rootScss)
            : base(parent, rootScss)
        {
            this.By = ScssBuilder.CreateBy(rootScss);
        }

        public override bool IsVisible()
        {
            return this.Is.Visible(this.By);
        }
    }

    public abstract class WebElementWrap : ComponentBase
    {
        public readonly IWebElement Element;

        protected WebElementWrap(IPage parent, IWebElement element)
            : base(parent)
        {
            this.Element = element;
        }

        public override bool IsVisible()
        {
            return this.Element.Displayed;
        }

        public virtual void ClickAndWaitWhileAjax(bool ajaxInevitable = false)
        {
            this.Action.ClickAndWaitWhileAjax(this.Element, ajaxInevitable: ajaxInevitable);
        }
    }
}
namespace Selenium.Core.Framework.Page
{
    using OpenQA.Selenium;

    public interface IProgressBar : IComponent
    {
        void WaitWhileVisible();
    }

    public class WebLoader : ComponentBase, IProgressBar
    {
        private readonly By _loaderSelector;

        public WebLoader(IPage parent, By loaderSelector)
            : base(parent)
        {
            this._loaderSelector = loaderSelector;
        }

        public override bool IsVisible()
        {
            return this.Is.Visible(this._loaderSelector);
        }

        public void WaitWhileVisible()
        {
            this.Wait.WhileElementVisible(this._loaderSelector);
        }
    }
}
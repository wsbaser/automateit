namespace Selenium.Core.Framework.Page
{
    using System;

    public abstract class AlertBase : ComponentBase, IHtmlAlert
    {
        protected AlertBase(IPage parent)
            : base(parent)
        {
        }

        #region IHtmlAlert Members

        public abstract void Dismiss();

        public virtual void Accept()
        {
            this.Dismiss();
        }

        public virtual void SendKeys(string keysToSend)
        {
            throw new NotImplementedException();
        }

        public void SetAuthenticationCredentials(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public virtual string Text
        {
            get
            {
                return this.GetType().Name;
            }
        }

        #endregion
    }
}
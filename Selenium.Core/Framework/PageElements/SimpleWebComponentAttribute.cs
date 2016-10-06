namespace Selenium.Core.Framework.PageElements
{
    public sealed class SimpleWebComponentAttribute : FindsByAttribute, IComponentAttribute
    {
        private string _componentName;

        public string ComponentName
        {
            get
            {
                return string.IsNullOrEmpty(this._componentName) && !string.IsNullOrEmpty(this.LinkText)
                           ? this.LinkText
                           : this._componentName;
            }
            set
            {
                this._componentName = value;
            }
        }

        #region IComponentArgs Members

        public object[] Args
        {
            get
            {
                return new object[] { this.Finder };
            }
        }

        #endregion
    }
}
namespace Selenium.Core.Framework.PageElements
{
    public class WebLoaderArgsAttribute : FindsByAttribute, IComponentAttribute
    {
        #region IComponentArgs Members

        public object[] Args
        {
            get
            {
                return new object[] { this.Finder };
            }
        }

        public string ComponentName { get; set; }

        #endregion
    }
}
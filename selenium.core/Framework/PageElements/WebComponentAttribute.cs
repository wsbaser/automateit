using System;

namespace selenium.core.Framework.PageElements {
    public class WebComponentAttribute : Attribute, IComponentAttribute {
        public WebComponentAttribute(params object[] args) {
            Args = args;
        }

        #region IComponentArgs Members

        public object[] Args { get; private set; }
        public string ComponentName { get; set; }

        #endregion
    }
}
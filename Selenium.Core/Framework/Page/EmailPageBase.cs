namespace Selenium.Core.Framework.Page
{
    using System;
    using System.IO;

    public abstract class EmailPageBase : PageBase, IEmailPage
    {
        private string FileName
        {
            get
            {
                return this.GetType().Name + ".html";
            }
        }

        #region IEmailPage Members

        public bool Match(Uri uri)
        {
            var name = new FileInfo(uri.AbsolutePath).Name;
            return string.Compare(name, this.FileName, StringComparison.InvariantCulture) == 0;
        }

        #endregion
    }

    public interface IEmailPage
    {
        bool Match(Uri uri);
    }
}
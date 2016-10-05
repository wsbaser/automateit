namespace Selenium.Core.SCSS
{
    using System;

    public class InvalidScssException : Exception
    {
        public InvalidScssException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }
}
namespace Selenium.Core.Exceptions
{
    using System;

    public class FrameworkException : Exception
    {
        public FrameworkException(string message)
            : base(message)
        {
        }
    }
}
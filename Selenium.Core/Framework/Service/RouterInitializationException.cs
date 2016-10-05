namespace Selenium.Core.Framework.Service
{
    using System;

    public class RouterInitializationException : Exception
    {
        public RouterInitializationException(Exception cause)
            : base("Error in router initialization", cause)
        {
        }
    }
}
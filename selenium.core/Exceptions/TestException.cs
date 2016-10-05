namespace Selenium.Core.Exceptions
{
    using System;

    public class TestException : Exception
    {
        public TestException(string message)
            : base(message)
        {
        }
    }
}
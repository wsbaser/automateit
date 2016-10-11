namespace Selenium.Core.Tests
{
    using System;

    using NUnit.Framework;

    using Selenium.Core.Framework.Browser;

    public class TestHelper
    {
        /// <summary>
        ///     ��������� ��������� ��������
        ///     ����� ��������� ��� ���� Exception ��������������� � AssertException
        /// </summary>
        public static void AssertAction(Action action, string msg, params object[] args)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                throw new AssertionException(string.Format(msg, args), e);
            }
        }

        /// <summary>
        ///     ��������� ��� � ����������� ������ ���� ����� window.print()
        /// </summary>
        public static void AssertHasPrintAction(ISeleniumContext context, string css)
        {
            var clickHandlers = context.Browser.Js.GetEventHandlers(css, JsEventType.click);
            Assert.IsTrue(
                clickHandlers.Contains("window.print()"),
                "���������� ����� �� �������� '{0}' �� �������� ������ window.print()");
        }
    }
}
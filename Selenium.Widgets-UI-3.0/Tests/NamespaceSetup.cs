namespace Selenium.Widget.v3.Tests
{
    using NUnit.Framework;

    [SetUpFixture]
    public class NamespaceSetup
    {
        [OneTimeSetUp]
        public void AssemblyInit()
        {
            WidgetSeleniumContext.Inst.Init();
        }

        [OneTimeTearDown]
        public static void SuiteTearDown()
        {
            WidgetSeleniumContext.Inst.Destroy();
        }
    }
}
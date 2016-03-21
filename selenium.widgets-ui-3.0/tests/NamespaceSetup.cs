using NUnit.Framework;

namespace selenium.widget.v3.tests
{
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
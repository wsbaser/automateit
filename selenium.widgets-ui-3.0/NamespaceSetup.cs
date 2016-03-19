using NUnit.Framework;
namespace selenium.widgets_ui_3._0
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
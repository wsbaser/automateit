namespace Selenium.Widget.v3
{
    using Selenium.Core;
    using Selenium.Core.Framework.Service;
    using Selenium.Widget.v3.Service;

    public class WidgetSeleniumContext : SeleniumContext<WidgetSeleniumContext>
    {
        protected override void InitWeb()
        {
            try
            {
                Web.RegisterService(new WidgetsServiceFactory());
            }
            catch (RouterInitializationException e)
            {
                Log.FatalError("Unable to initialize service", e);
            }
        }
    }
}
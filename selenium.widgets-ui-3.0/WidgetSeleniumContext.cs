using selenium.beeline.Pages.Base;
using selenium.core;
using selenium.core.Framework.Service;
namespace selenium.widgets_ui_3._0
{
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

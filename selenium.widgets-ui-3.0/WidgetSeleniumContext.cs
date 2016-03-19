using selenium.core;
using selenium.core.Framework.Service;
using selenium.widgets_ui_3._0.service;

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

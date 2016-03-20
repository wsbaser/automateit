using selenium.core;
using selenium.core.Framework.Service;
using selenium.widget.v3.service;

namespace selenium.widget.v3
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

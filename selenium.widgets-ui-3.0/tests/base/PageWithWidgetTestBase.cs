namespace Selenium.Widget.v3.Tests.@Base
{
    using Selenium.Core.Framework.Browser;
    using Selenium.Core.Framework.Page;
    using Selenium.Core.Logging;
    using Selenium.Widget.v3.Service.Pages;

    public abstract class PageWithWidgetTestBase<P> : PageTestBase<P>
        where P : IPage
    {
        private const string LIVETEX_SITE_CODE = @"window['liveTex'] = true,
                window['liveTexID'] = {0},
                window['liveTex_object'] = true;
                (function() {{
                    var t = document['createElement']('script');
                    t.type = 'text/javascript';
                    t.async = true;
                    t.src = '//cs15.livetex.ru/js/client.js';
                    var c = document['getElementsByTagName']('script')[0];
                    if (c) c['parentNode']['insertBefore'](t, c);
                    else document['documentElement']['firstChild']['appendChild'](t);
                }})();";

        protected override Browser Browser
        {
            get
            {
                return WidgetSeleniumContext.Inst.Browser;
            }
        }

        protected override ITestLogger Log
        {
            get
            {
                return WidgetSeleniumContext.Inst.Log;
            }
        }

        public PageWithWidget GoToPageWithWidget(int siteId)
        {
            // . navigate to page
            var page = this.Go.ToPage<PageWithWidget>();
            // . inject livetex site code
            this.Js.Excecute(LIVETEX_SITE_CODE, siteId);
            // . wait for widget is visible
            page.WidgetLabel.WaitForVisible();
            return page;
        }
    }
}
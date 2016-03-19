﻿using selenium.core.Framework.Browser;
using selenium.core.Framework.Page;
using selenium.core.Logging;
using selenium.widgets_ui_3._0.service.pages;

namespace selenium.widgets_ui_3._0.tests.@base
{
    public abstract class PageWithWidgetTestBase<P> : PageTestBase<P> where P : IPage
    {
        private const string LIVETEX_SITE_CODE =
            @"window['liveTex'] = true,
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
            get { return WidgetSeleniumContext.Inst.Browser; }
        }

        protected override TestLogger Log
        {
            get { return WidgetSeleniumContext.Inst.Log; }
        }

        public WidgetPage GoToPageWithWidget(int siteId)
        {
            // . navigate to page
            var page = Go.ToPage<WidgetPage>();
            // . inject livetex site code
            Js.Excecute(LIVETEX_SITE_CODE, siteId);
            // . wait for widget is visible
            page.Widget.WaitForVisible();
            return page;
        }
    }
}
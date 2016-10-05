/**
 * Created by VolkovA on 26.02.14.
 */

namespace Selenium.Core
{
    using System;

    using Selenium.Core.Framework.Browser;
    using Selenium.Core.Framework.Service;
    using Selenium.Core.Logging;

    public class SeleniumContext<T> : ISeleniumContext
        where T : ISeleniumContext
    {
        private readonly BrowsersCache _browsersCache;

        protected SeleniumContext()
        {
            this.Log = new TestLogger();
            this.Web = new Web();
            this._browsersCache = new BrowsersCache(this.Web, this.Log);
        }

        public static T Inst
        {
            get
            {
                return SingletonCreator<T>.CreatorInstance;
            }
        }

        protected virtual void InitWeb()
        {
        }

        public virtual void Init()
        {
            this.InitWeb();
        }

        public void Destroy()
        {
            Inst.Browser.Destroy();
        }

        #region Nested type: SingletonCreator

        /// ‘абрика используетс€ дл€ отложенной инициализации экземпл€ра класса
        private sealed class SingletonCreator<S>
            where S : ISeleniumContext
        {
            //»спользуетс€ Reflection дл€ создани€ экземпл€ра класса без публичного конструктора

            //            (S).GetConstructor(
            //                BindingFlags.Instance | BindingFlags.NonPublic,
            //                null,
            //                new Type[0],
            //                new ParameterModifier[0]).Invoke(null);

            public static S CreatorInstance { get; } = (S)Activator.CreateInstance(typeof(S));
        }

        #endregion

        #region ISeleniumContext Members

        public Web Web { get; }

        public ITestLogger Log { get; }

        public Browser Browser
        {
            get
            {
                return this._browsersCache.GetBrowser(BrowserType.CHROME);
            }
        }

        #endregion
    }
}
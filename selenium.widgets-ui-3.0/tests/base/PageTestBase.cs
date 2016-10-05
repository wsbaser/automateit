namespace Selenium.Widget.v3.Tests.@Base
{
    using System;

    using Selenium.Core.Exceptions;
    using Selenium.Core.Framework.Page;

    public abstract class PageTestBase<P> : TestBase
        where P : IPage
    {
        protected P Page
        {
            get
            {
                return this.Browser.State.PageAs<P>();
            }
        }

        /// <summary>
        ///     Имитация FixtureSetup - метода который вызывается один раз перед всеми тестами класса
        /// </summary>
        /// <remarks>
        ///     Т.к. все тесты класса запускаются раннером последовательно, то мы можем гарантировать что приведя страницу в нужное
        ///     нам состояние в методе FixtureSetup
        ///     во всех тестах класса страница будет находиться именно в этом состоянии, при условии что тесты не будут выполнять
        ///     на ней дополнительных действий
        /// </remarks>
        /// <param name="action">Действие, которое нужно выполнить один раз</param>
        protected void PreparePage(Action action)
        {
            if (this.Browser.State.PageIs<P>())
            {
                return;
            }
            action.Invoke();
            this.Browser.State.PageAs<P>();
            if (!this.Browser.State.PageIs<P>())
            {
                throw Throw.FrameworkException("Не перешли на страницу '{0}'", typeof(P).Name);
            }
        }
    }
}
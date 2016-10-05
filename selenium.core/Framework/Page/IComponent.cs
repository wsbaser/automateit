/**
 * Created by VolkovA on 27.02.14.
 */

namespace Selenium.Core.Framework.Page
{
    public interface IComponent : IPageObject
    {
        IPage ParentPage { get; }

        string ComponentName { get; set; }

        bool IsVisible();
    }
}
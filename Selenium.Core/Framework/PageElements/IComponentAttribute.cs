namespace Selenium.Core.Framework.PageElements
{
    public interface IComponentAttribute
    {
        object[] Args { get; }

        string ComponentName { get; set; }
    }
}
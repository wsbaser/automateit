namespace Selenium.Core.SCSS
{
    using global::Core.Extensions;

    internal enum AttributeMatchStyle
    {
        [StringValue("=")]
        Equal,

        [StringValue("~")]
        Contains
    }
}
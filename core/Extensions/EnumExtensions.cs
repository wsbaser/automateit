namespace Core.Extensions
{
    using System;

    public static class EnumExtensions
    {
        public static string StringValue(this Enum value)
        {
            string output = null;
            var type = value.GetType();
            var fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs != null && attrs.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }
    }
}
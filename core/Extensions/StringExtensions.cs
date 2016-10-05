namespace Core.Extensions
{
    using NUnit.Framework;

    using Сore;

    public static class StringExtensions
    {
        public static string CutFirst(this string s)
        {
            return s.Substring(1, s.Length - 1);
        }

        public static string CutFirst(this string s, char symbol)
        {
            return s.StartsWith(symbol.ToString()) ? s.Substring(1, s.Length - 1) : s;
        }

        public static string CutLast(this string s, char symbol)
        {
            return s.EndsWith(symbol.ToString()) ? s.Substring(0, s.Length - 1) : s;
        }

        public static int AsInt(this string s)
        {
            return int.Parse(FindInt(s));
        }

        public static decimal AsDecimal(this string s)
        {
            return decimal.Parse(s.FindNumber());
        }

        public static string FindNumber(this string s)
        {
            return RegexHelper.GetString(s, "((?:-|)\\d+(?:(?:\\.|,)\\d+)?)");
        }

        public static string FindInt(this string s)
        {
            return RegexHelper.GetString(s, "((?:-|)\\d+)");
        }

        public static string FindUInt(this string s)
        {
            return RegexHelper.GetString(s, "(\\d+)");
        }
    }

    [TestFixture]
    public class StringExtensionsTest
    {
        [TestCase("-5", "-5")]
        [TestCase("text -5 text", "-5")]
        [TestCase("text 1.2 text", "1.2")]
        [TestCase("text 1,2 text", "1,2")]
        public void FindNumber(string text, string expected)
        {
            Assert.AreEqual(expected, text.FindNumber());
        }

        [TestCase("-5", -5)]
        public void AsDecimal(string text, decimal expected)
        {
            Assert.AreEqual(expected, text.AsDecimal());
        }

        [TestCase("1", "1")]
        [TestCase("text 1 text", "1")]
        [TestCase("text 1.2 text", "1")]
        [TestCase("-1", "-1")]
        [TestCase("text -1 text", "-1")]
        [TestCase("text -1.2 text", "-1")]
        public void FindInt(string text, string expected)
        {
            Assert.AreEqual(expected, text.FindInt());
        }
    }
}
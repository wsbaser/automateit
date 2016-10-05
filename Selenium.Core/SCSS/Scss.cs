namespace Selenium.Core.SCSS
{
    using System.Linq;

    using NUnit.Framework;

    using OpenQA.Selenium;

    using Сore;

    public class Scss
    {
        public readonly string Css;

        public readonly string Xpath;

        public Scss(string xpath, string css)
        {
            this.Css = css;
            this.Xpath = xpath;
        }

        public By By
        {
            get
            {
                return string.IsNullOrEmpty(this.Css) ? By.XPath(this.Xpath) : By.CssSelector(this.Css);
            }
        }

        public string Value
        {
            get
            {
                return string.IsNullOrEmpty(this.Css) ? this.Xpath : this.Css;
            }
        }

        public static string Concat(string scssSelector1, string scssSelector2)
        {
            return ScssBuilder.Concat(scssSelector1, scssSelector2).Value;
        }

        public Scss Concat(Scss scss2)
        {
            string resultXpath = XPathBuilder.Concat(this.Xpath, scss2.Xpath);
            var resultCss = string.IsNullOrEmpty(this.Css) || string.IsNullOrEmpty(scss2.Css)
                                ? string.Empty
                                : CssBuilder.Concat(this.Css, scss2.Css);
            return new Scss(resultXpath, resultCss);
        }

        public static By GetBy(string scssSelector1, string scssSelector2)
        {
            return ScssBuilder.Concat(scssSelector1, scssSelector2).By;
        }

        public static By GetBy(string scssSelector)
        {
            return ScssBuilder.CreateBy(scssSelector);
        }
    }

    public class CssBuilder
    {
        private const char CSS_PARTS_DELIMITER = ',';

        public static string Concat(string rootCss, string relativeCss)
        {
            if (string.IsNullOrWhiteSpace(relativeCss))
            {
                return rootCss;
            }
            if (string.IsNullOrEmpty(rootCss))
            {
                return relativeCss;
            }
            var roots = rootCss.Split(CSS_PARTS_DELIMITER);
            if (roots.Length == 1)
            {
                // Выход из рекурсии
                return string.Format("{0} {1}", rootCss, relativeCss);
            }
            var s = roots.Aggregate(
                string.Empty,
                (current, rootXpath) => current + Concat(rootXpath.Trim(), relativeCss) + ",");
            return s.Substring(0, s.Length - 1);
        }
    }

    [TestFixture]
    public class ScssTests
    {
        [TestCase("div", "div", "//div/descendant::div", "div div")]
        public void Run(string scssSelector1, string scssSelector2, string resultXpath, string resultCss)
        {
            var scss1 = ScssBuilder.Create(scssSelector1);
            var scss2 = ScssBuilder.Create(scssSelector2);
            var resultScss = scss1.Concat(scss2);
            Assert.AreEqual(resultXpath, resultScss.Xpath);
            Assert.AreEqual(resultCss, resultScss.Css);
        }
    }
}
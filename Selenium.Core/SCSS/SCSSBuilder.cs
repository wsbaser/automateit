﻿namespace Selenium.Core.SCSS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using global::Core.Extensions;

    using NUnit.Framework;

    using OpenQA.Selenium;

    using Сore;

    public class ScssBuilder
    {
        private const string DESCENDANT_AXIS = "descendant::";

        private const string CHILD_AXIS = "child::";

        private static readonly Regex _identRegex = new Regex("\\A-?[_a-zA-Z][_a-zA-Z0-9-]*\\z", RegexOptions.Compiled);

        private static readonly Regex _nameRegex = new Regex("\\A[_a-zA-Z0-9-]+\\z", RegexOptions.Compiled);

        private static readonly Regex _numberRegex = new Regex("\\A\\d+\\z", RegexOptions.Compiled);

        public static By CreateBy(string scssSelector, params object[] args)
        {
            scssSelector = string.Format(scssSelector, args);
            return Create(scssSelector).By;
        }

        public static Scss Create(string scssSelector)
        {
            if (string.IsNullOrEmpty(scssSelector))
            {
                return new Scss(string.Empty, string.Empty);
            }
            var xpath = string.Empty;
            string css = null;
            try
            {
                var isTrueCss = true;
                var parts = SplitIgnoringConditions(scssSelector, true, ',');
                foreach (var partScssSelector in parts)
                {
                    bool partScssIsTrueCss;
                    var xpathPart = ScssPartToXpath(partScssSelector, out partScssIsTrueCss);
                    xpath += "//" + RemoveDescendantAxis(xpathPart) + "|";
                    isTrueCss &= partScssIsTrueCss;
                }
                xpath = xpath.Substring(0, xpath.Length - 1);
                if (isTrueCss)
                {
                    css = scssSelector;
                }
            }
            catch (InvalidScssException e)
            {
                // Это не scss, возможно это xpath
                if (XPathBuilder.IsXPath(scssSelector))
                {
                    xpath = scssSelector;
                }
                else
                {
                    throw e;
                }
            }
            return new Scss(xpath, css);
        }

        /// <summary>
        ///     Разбить строку на подстроки
        /// </summary>
        /// <example>
        ///     "td['Категории, на которые вы уже подписаны'],td.category" => (delimiters=[','], cutDelimiters=true)
        ///     "td['Категории, на которые вы уже подписаны']"
        ///     "td.category"
        ///     ">div.toggle-drop>ul>li>span['Вечером']" => (delimiters=['>',' ','+'], cutDelimiters=false)
        ///     ">div.toggle-drop"
        ///     ">ul"
        ///     ">li"
        ///     ">span['Вечером']"
        /// </example>
        /// <param name="scssSelector">исходная строка</param>
        /// <param name="cutDelimiters">удалить разделители</param>
        /// <param name="delimiters">список разделителей</param>
        /// <returns>список подстрок</returns>
        private static List<string> SplitIgnoringConditions(
            string scssSelector,
            bool cutDelimiters,
            params char[] delimiters)
        {
            var scssParts = new List<string>();
            var value = string.Empty;
            var readCondition = false;
            var conditionOpenBracketsCount = 0;
            for (var i = 0; i < scssSelector.Length; i++)
            {
                var c = scssSelector[i];
                if (readCondition)
                {
                    if (IsClosingConditionBracket(ref conditionOpenBracketsCount, c))
                    {
                        readCondition = false;
                    }
                }
                else if (delimiters.Contains(c))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        scssParts.Add(value);
                    }
                    value = string.Empty;
                    if (cutDelimiters)
                    {
                        continue; // выбрасывать разделители
                    }
                }
                else if (c == '[')
                {
                    readCondition = true;
                }
                value += c;
            }
            if (string.IsNullOrEmpty(value) || readCondition)
            {
                throw new InvalidScssException("SplitIgnoringConditions. unexpected end of line");
            }
            scssParts.Add(value);
            return scssParts;
        }

        /// <summary>
        ///     scss = scsspart;scsspart;...scsspart
        ///     Преобразует scsspart в xpath
        /// </summary>
        /// <param name="scssPart"></param>
        /// <param name="isTrueCss">является ли указанный селектор оригинальным Css</param>
        private static string ScssPartToXpath(string scssPart, out bool isTrueCss)
        {
            var elementScssSelectors = SplitIgnoringConditions(scssPart, false, ' ', '>', '+');
            var xpath = string.Empty;
            isTrueCss = true;
            for (var i = 0; i < elementScssSelectors.Count; i++)
            {
                var elementScssSelector = elementScssSelectors[i];
                bool elementScssIsTrueCss;
                var elementXpath = ElementScssToXpath(elementScssSelector, out elementScssIsTrueCss);
                if (i > 0)
                {
                    elementXpath = "/" + RemoveChildAxis(elementXpath);
                }
                xpath += elementXpath;
                isTrueCss &= elementScssIsTrueCss;
            }
            return xpath;
        }

        private static string RemoveDescendantAxis(string elementXpath)
        {
            if (elementXpath.StartsWith(DESCENDANT_AXIS))
            {
                elementXpath = elementXpath.Remove(0, DESCENDANT_AXIS.Length);
            }
            return elementXpath;
        }

        private static string RemoveChildAxis(string elementXpath)
        {
            if (elementXpath.StartsWith(CHILD_AXIS))
            {
                elementXpath = elementXpath.Remove(0, CHILD_AXIS.Length);
            }
            return elementXpath;
        }

        private static string ElementScssToXpath(string elementScss)
        {
            bool dummyBool;
            return ElementScssToXpath(elementScss, out dummyBool);
        }

        /// <summary>
        ///     scsspart = elementScss elementScss ... elementScss
        ///     Преобразует elementScss в xpath
        /// </summary>
        private static string ElementScssToXpath(string elementScss, out bool isTrueCss)
        {
            if (string.IsNullOrWhiteSpace(elementScss))
            {
                throw new InvalidScssException("Invalid scss: {0}", elementScss);
            }
            var combinator = string.Empty;
            switch (elementScss[0])
            {
                case ' ':
                case '>':
                    combinator = elementScss[0].ToString();
                    elementScss = elementScss.CutFirst();
                    break;
            }
            var tag = string.Empty;
            var id = string.Empty;
            var className = string.Empty;
            var condition = string.Empty;
            var classNames = new List<string>();
            var attributes = new List<ScssAttribute>();
            var conditions = new List<string>();
            var subelementXpaths = new List<string>();
            var state = State.ReadTag;
            var conditionOpenBracketsCount = 0; // количество открытых скобок [ внутри условия
            for (var i = 0; i < elementScss.Length; i++)
            {
                var c = elementScss[i];
                if (state == State.ReadCondition && !IsClosingConditionBracket(ref conditionOpenBracketsCount, c))
                {
                    // внутри условия могут быть символы . # [ на которые нужно не обращать внимания
                    condition += c;
                    continue;
                }
                switch (c)
                {
                    case '.':
                        switch (state)
                        {
                            case State.ReadClass:
                                if (string.IsNullOrEmpty(className))
                                {
                                    ThrowIncorrectSymbol(state, i, elementScss);
                                }
                                classNames.Add(className);
                                className = string.Empty;
                                break;
                            case State.ReadId:
                                if (string.IsNullOrEmpty(id))
                                {
                                    ThrowIncorrectSymbol(state, i, elementScss);
                                }
                                break;
                            case State.ReadTag:
                            case State.Undefined:
                                break; // допустимые состояния
                            default:
                                ThrowIncorrectSymbol(state, i, elementScss);
                                break;
                        }
                        state = State.ReadClass;
                        break;
                    case '#':
                        if (!string.IsNullOrEmpty(id))
                        {
                            throw new InvalidScssException("two ids are illegal");
                        }
                        switch (state)
                        {
                            case State.ReadClass:
                                if (string.IsNullOrEmpty(className))
                                {
                                    ThrowIncorrectSymbol(state, i, elementScss);
                                }
                                classNames.Add(className);
                                className = string.Empty;
                                break;
                            case State.ReadTag:
                            case State.Undefined:
                                break;
                            default:
                                ThrowIncorrectSymbol(state, i, elementScss);
                                break;
                        }
                        state = State.ReadId;
                        break;
                    case '[':
                        switch (state)
                        {
                            case State.ReadClass:
                                if (string.IsNullOrEmpty(className))
                                {
                                    ThrowIncorrectSymbol(state, i, elementScss);
                                }
                                classNames.Add(className);
                                className = string.Empty;
                                break;
                            case State.ReadId:
                                if (string.IsNullOrEmpty(id))
                                {
                                    ThrowIncorrectSymbol(state, i, elementScss);
                                }
                                break;
                            case State.ReadTag:
                            case State.Undefined:
                                break; // допустимые состояния
                            default:
                                ThrowIncorrectSymbol(state, i, elementScss);
                                break;
                        }
                        state = State.ReadCondition;
                        break;
                    case ']':
                        if (state != State.ReadCondition)
                        {
                            ThrowIncorrectSymbol(state, i, elementScss);
                        }
                        if (IsText(condition) || IsNumber(condition) || IsFunction(condition))
                        {
                            // текстовое условие
                            conditions.Add(condition);
                        }
                        else
                        {
                            var attribute = ParseAttribute(condition);
                            if (attribute != null)
                            {
                                attributes.Add(attribute);
                            }
                            else
                            {
                                // вложенный селектор
                                bool dummyBool;
                                var xpathPart = ScssPartToXpath(condition, out dummyBool);
                                subelementXpaths.Add(RemoveChildAxis(xpathPart));
                            }
                        }
                        condition = string.Empty;
                        state = State.Undefined;
                        break;
                    default:
                        switch (state)
                        {
                            case State.ReadId:
                                id += c;
                                break;
                            case State.ReadTag:
                                tag += c;
                                break;
                            case State.ReadClass:
                                className += c;
                                break;
                            case State.ReadCondition:
                                condition += c;
                                break;
                            case State.Undefined:
                                ThrowIncorrectSymbol(state, i, elementScss);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                }
            }
            switch (state)
            {
                case State.Undefined:
                case State.ReadId:
                case State.ReadTag:
                    break;
                case State.ReadClass:
                    if (string.IsNullOrEmpty(className))
                    {
                        ThrowIncorrectSymbol(state, elementScss.Length, elementScss);
                    }
                    classNames.Add(className);
                    break;
                case State.ReadCondition:
                    if (string.IsNullOrEmpty(className))
                    {
                        ThrowIncorrectSymbol(state, elementScss.Length, elementScss);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            isTrueCss = conditions.Count == 0 && subelementXpaths.Count == 0
                        && attributes.All(a => IsCssMatchStyle(a.MatchStyle));
            Validate(tag, id, classNames, attributes);
            return AggregateToXpath(combinator, tag, id, classNames, attributes, conditions, subelementXpaths);
        }

        private static bool IsCssMatchStyle(AttributeMatchStyle matchStyle)
        {
            switch (matchStyle)
            {
                case AttributeMatchStyle.Equal:
                    return true;
                default:
                    return false;
            }
        }

        private static ScssAttribute ParseAttribute(string condition)
        {
            foreach (AttributeMatchStyle matchStyle in Enum.GetValues(typeof(AttributeMatchStyle)))
            {
                string[] arr = condition.Split(
                    new[] { matchStyle.StringValue() },
                    StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 2 && IsText(arr[1]))
                {
                    return new ScssAttribute(arr[0], arr[1], matchStyle);
                }
            }
            return null;
        }

        private static bool IsFunction(string condition)
        {
            // TODO: можно заменить на contains function чтобы использовать выражения вида last()-1
            switch (condition)
            {
                case "last()":
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsNumber(string condition)
        {
            return _numberRegex.IsMatch(condition);
        }

        private static void Validate(string tag, string id, List<string> classNames, List<ScssAttribute> attributes)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                ValidateIsElementName(tag);
            }
            if (!string.IsNullOrEmpty(id))
            {
                ValidateIsName(id);
            }
            foreach (var className in classNames)
            {
                ValidateIsIdent(className);
            }
            foreach (var attribute in attributes)
            {
                ValidateIsIdent(attribute.Name);
            }
        }

        private static void ValidateIsName(string value)
        {
            if (!IsName(value))
            {
                throw new InvalidScssException("'{0}' is not name", value);
            }
        }

        private static void ValidateIsIdent(string value)
        {
            if (!IsIdent(value))
            {
                throw new InvalidScssException("'{0}' is not ident", value);
            }
        }

        private static void ValidateIsElementName(string value)
        {
            if (!IsElementName(value))
            {
                throw new InvalidScssException("'{0}' is not element name", value);
            }
        }

        private static bool IsIdent(string value)
        {
            // ident		-?[_a-z][_a-z0-9-]*
            return _identRegex.IsMatch(value);
        }

        private static bool IsElementName(string value)
        {
            // element_name : IDENT | '*'
            return value == "*" || IsIdent(value);
        }

        private static bool IsName(string value)
        {
            // name		[_a-z0-9-]+
            return _nameRegex.IsMatch(value);
        }

        private static bool IsClosingConditionBracket(ref int conditionOpenBracketsCount, char c)
        {
            if (c == '[')
            {
                conditionOpenBracketsCount++;
            }
            else if (c == ']')
            {
                if (conditionOpenBracketsCount == 0)
                {
                    return true;
                }
                conditionOpenBracketsCount--;
            }
            return false;
        }

        private static string AggregateToXpath(
            string axis,
            string tag,
            string id,
            List<string> classNames,
            List<ScssAttribute> attributes,
            List<string> conditions,
            List<string> subelementXpaths)
        {
            tag = string.IsNullOrEmpty(tag) ? "*" : tag;
            var xpath = XpathAxis(axis) + tag;
            if (!string.IsNullOrEmpty(id))
            {
                xpath += XpathAttributeCondition("id", string.Format("'{0}'", id));
            }
            foreach (var className in classNames)
            {
                xpath += XpathAttributeCondition(
                    "class",
                    string.Format("'{0}'", className),
                    AttributeMatchStyle.Contains);
            }
            foreach (var attribute in attributes)
            {
                xpath += XpathAttributeCondition(attribute.Name, attribute.Value, attribute.MatchStyle);
            }
            foreach (var condition in conditions)
            {
                if (IsText(condition))
                {
                    xpath += XpathTextCondition(condition);
                }
                else
                {
                    xpath += XpathCondition(condition);
                }
            }
            foreach (var subelementXpath in subelementXpaths)
            {
                xpath += XpathCondition(subelementXpath);
            }
            return xpath;
        }

        private static string XpathCondition(string condition)
        {
            return string.Format("[{0}]", condition);
        }

        private static string XpathAxis(string axis)
        {
            switch (axis)
            {
                case "":
                case " ":
                    return DESCENDANT_AXIS;
                case ">":
                    return CHILD_AXIS;
                default:
                    throw new ArgumentOutOfRangeException("axis");
            }
        }

        private static string XpathTextCondition(string text)
        {
            if (text.StartsWith("~"))
            {
                text = text.CutFirst('~');
                return string.Format("[contains(text(),{0})]", text);
            }
            return string.Format("[text()={0}]", text);
        }

        private static string XpathAttributeCondition(
            string name,
            string value,
            AttributeMatchStyle style = AttributeMatchStyle.Equal)
        {
            switch (style)
            {
                case AttributeMatchStyle.Equal:
                    return string.Format("[@{0}={1}]", name, value);
                case AttributeMatchStyle.Contains:
                    return string.Format("[contains(@{0},{1})]", name, value);
                default:
                    throw new ArgumentOutOfRangeException("style");
            }
        }

        private static bool IsText(string stringValue)
        {
            stringValue = stringValue.CutFirst('~');
            return stringValue.Length > 1
                   && ((stringValue.StartsWith("'") && stringValue.EndsWith("'"))
                       || (stringValue.StartsWith("\"") && stringValue.EndsWith("\"")));
        }

        private static void ThrowIncorrectSymbol(State state, int index, string scss)
        {
            throw new InvalidScssException(
                "incorrect symbol for state: state: {0}, index: {1}, scss: {2}",
                state,
                index,
                scss);
        }

        public static Scss Concat(string scssSelector1, string scssSelector2)
        {
            var scss1 = Create(scssSelector1);
            var scss2 = Create(scssSelector2);
            return scss1.Concat(scss2);
        }

        #region Nested type: State

        private enum State
        {
            Undefined,

            ReadId,

            ReadTag,

            ReadClass,

            ReadCondition
        }

        #endregion
    }

    [TestFixture]
    public class ScssBuilderTests
    {
        [TestCase("div[~'text']", "//div[contains(text(),'text')]")]
        [TestCase("div['text']", "//div[text()='text']")]
        [TestCase("div[src='1.png']['text']", "//div[@src='1.png'][text()='text']")]
        [TestCase("div[src=\"1.png\"]['text']", "//div[@src=\"1.png\"][text()='text']")]
        [TestCase(".classname#myid['text']", "//*[@id='myid'][contains(@class,'classname')][text()='text']")]
        [TestCase(".classname['mytext']", "//*[contains(@class,'classname')][text()='mytext']")]
        [TestCase("div.classname['mytext']", "//div[contains(@class,'classname')][text()='mytext']")]
        [TestCase(".classname1.classname2['mytext']",
            "//*[contains(@class,'classname1')][contains(@class,'classname2')][text()='mytext']")]
        [TestCase("div.classname1.classname2['mytext']",
            "//div[contains(@class,'classname1')][contains(@class,'classname2')][text()='mytext']")]
        [TestCase(".classname1['mytext'] .classname2['mytext']",
            "//*[contains(@class,'classname1')][text()='mytext']/descendant::*[contains(@class,'classname2')][text()='mytext']"
            )]
        [TestCase("div.classname1['mytext'] div.classname2['mytext']",
            "//div[contains(@class,'classname1')][text()='mytext']/descendant::div[contains(@class,'classname2')][text()='mytext']"
            )]
        [TestCase("#myid div['mytext']", "//*[@id='myid']/descendant::div[text()='mytext']")]
        [TestCase("div#myid div['mytext']", "//div[@id='myid']/descendant::div[text()='mytext']")]
        [TestCase("div#myid.classname div['mytext']",
            "//div[@id='myid'][contains(@class,'classname')]/descendant::div[text()='mytext']")]
        [TestCase("div#main-basket-info-div>ul>li['Тариф']>a",
            "//div[@id='main-basket-info-div']/ul/li[text()='Тариф']/a")]
        [TestCase("li[>h5>strong>a['mytext']]", "//li[h5/strong/a[text()='mytext']]")]
        [TestCase("li[>a]", "//li[a]")]
        [TestCase("li[>a[div]]", "//li[a[descendant::div]]")]
        [TestCase("tr[1]>td[last()]", "//tr[1]/td[last()]")]
        [TestCase("img[src~'111.png']", "//img[contains(@src,'111.png')]")]
        [TestCase("#showThemesPanel,.genre-filter['text']",
            "//*[@id='showThemesPanel']|//*[contains(@class,'genre-filter')][text()='text']")]
        [TestCase(">div.toggle-drop>ul>li>span['Вечером']",
            "//child::div[contains(@class,'toggle-drop')]/ul/li/span[text()='Вечером']")]
        [TestCase("li[10]>div.news-block", "//li[10]/div[contains(@class,'news-block')]")]
        [TestCase("td[h3>span['Категории, на которые вы уже подписаны']]>div>div",
            "//td[descendant::h3/span[text()='Категории, на которые вы уже подписаны']]/div/div")]
        //[TestCase("tr[span.ng-binding[descendant-or-self::*['{0}']]]", "tr[descendant::span[contains(@class,'ng-binding')][descendant-or-self::*[text()='{0}'])]]")]
        public void ConvertScssToXpath(string scssSelector, string result)
        {
            var scss = ScssBuilder.Create(scssSelector);
            Assert.AreEqual(result, scss.Xpath);
            Assert.IsEmpty(scss.Css);
        }

        [TestCase("#myid", "#myid")]
        [TestCase("div#myid", "div#myid")]
        [TestCase("div#myid.classname", "div#myid.classname")]
        [TestCase(".classname", ".classname")]
        [TestCase("div.classname", "div.classname")]
        [TestCase(".classname1.classname2", ".classname1.classname2")]
        [TestCase("div.classname1.classname2", "div.classname1.classname2")]
        [TestCase(".classname1 .classname2", ".classname1 .classname2")]
        [TestCase("div.classname1 div.classname2", "div.classname1 div.classname2")]
        [TestCase("div[src='1.png']", "div[src='1.png']")]
        [TestCase("div[src=\"1.png\"]", "div[src=\"1.png\"]")]
        [TestCase(">.search-bar", ">.search-bar")]
        [TestCase(".nav-section >.search-bar", ".nav-section >.search-bar")]
        [TestCase(".nav-section >.search-bar ul", ".nav-section >.search-bar ul")]
        public void ConvertScssToCss(string scssSelector, string result)
        {
            var scss = ScssBuilder.Create(scssSelector);
            Assert.AreEqual(result, scss.Css);
            Assert.IsEmpty(scss.Xpath);
        }
    }
}
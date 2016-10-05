namespace Selenium.Core.tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using NUnit.Framework;

    using Selenium.Core.Framework.Page;

    [TestFixture]
    public class UriMatcherTest
    {
        [Test]
        public void AbsolutePathDoesNotMatch()
        {
            var uriMatcher = new UriMatcher(
                "newssubscriptions/subscribeaction111/",
                new Dictionary<string, string>(),
                new StringDictionary());
            var uri = new Uri("http://moskva.dr-bee.ru/newssubscriptions/subscribeaction/?type=AddSubscription");
            var uriMatchResult = uriMatcher.Match(uri, "/");
            Assert.False(uriMatchResult.Success);
        }

        [Test]
        public void AbsolutePathMatches()
        {
            var uriMatcher = new UriMatcher(
                "newssubscriptions/subscribeaction/",
                new Dictionary<string, string>(),
                new StringDictionary());
            var uri = new Uri("http://moskva.dr-bee.ru/newssubscriptions/subscribeaction/?type=AddSubscription");
            var uriMatchResult = uriMatcher.Match(uri, "/");
            Assert.True(uriMatchResult.Success);
        }

        [Test]
        public void DataDoesNotMatch()
        {
            var data = new Dictionary<string, string>();
            data.Add("tariff", "wrong_tariff");
            var uriMatcher = new UriMatcher(
                "customers/products/mobile/services/details/{tariff}/",
                data,
                new StringDictionary());
            var uri =
                new Uri(
                    "http://moskva.beeline.ru/"
                    + "customers/products/mobile/services/details/highway-1gb-nedelya-besplatno/?deviceTypeId=cellphone");
            var uriMatchResult = uriMatcher.Match(uri, "/");
            Assert.False(uriMatchResult.Success, "���������� ���� Data �� ����������� ��� ������������� ��������");
        }

        [Test]
        public void DataMatch()
        {
            var data = new Dictionary<string, string>();
            data.Add("tariff", "highway-1gb-nedelya-besplatno");
            var uriMatcher = new UriMatcher(
                "customers/products/mobile/services/details/{tariff}/",
                data,
                new StringDictionary());
            var uri =
                new Uri(
                    "http://moskva.beeline.ru/"
                    + "customers/products/mobile/services/details/highway-1gb-nedelya-besplatno/?deviceTypeId=cellphone");
            var uriMatchResult = uriMatcher.Match(uri, "/");
            Assert.True(uriMatchResult.Success);
        }

        [Test]
        public void ExtractDataFromUri()
        {
            var uriMatcher = new UriMatcher(
                "customers/products/mobile/services/details/{tariff}/",
                new Dictionary<string, string>(),
                new StringDictionary());
            var uri =
                new Uri(
                    "http://moskva.beeline.ru/"
                    + "customers/products/mobile/services/details/highway-1gb-nedelya-besplatno/?deviceTypeId=cellphone");
            var uriMatchResult = uriMatcher.Match(uri, "/");
            Assert.True(uriMatchResult.Success);
            Assert.True(uriMatchResult.Data.ContainsKey("tariff"), "������ �� ��������� �������� � Url");
            Assert.AreEqual(
                "highway-1gb-nedelya-besplatno",
                uriMatchResult.Data["tariff"],
                "������ ��������� ������������ ��������");
        }

        [Test]
        public void ExtractParamFromUri()
        {
            var uriMatcher = new UriMatcher(
                "customers/products/mobile/services/details/{tariff}/",
                new Dictionary<string, string>(),
                new StringDictionary());
            var uri =
                new Uri(
                    "http://moskva.beeline.ru/"
                    + "customers/products/mobile/services/details/highway-1gb-nedelya-besplatno/?deviceTypeId=cellphone");
            var uriMatchResult = uriMatcher.Match(uri, "/");
            Assert.True(uriMatchResult.Success);
            Assert.True(uriMatchResult.Params.ContainsKey("deviceTypeId"), "������ �� ��������� �������� � Url");
            Assert.AreEqual(
                "cellphone",
                uriMatchResult.Params["deviceTypeId"],
                "������ ��������� ������������ ��������");
        }

        [Test]
        public void ParamsDoesNotMatch()
        {
            var _params = new StringDictionary();
            _params.Add("deviceTypeId", "wrong_params");
            var uriMatcher = new UriMatcher(
                "customers/products/mobile/services/details/{tariff}/",
                new Dictionary<string, string>(),
                _params);
            var uri =
                new Uri(
                    "http://moskva.beeline.ru/"
                    + "customers/products/mobile/services/details/highway-1gb-nedelya-besplatno/?deviceTypeId=cellphone");
            var uriMatchResult = uriMatcher.Match(uri, "/");
            Assert.False(uriMatchResult.Success, "���������� ���� Params �� ����������� ��� ������������� ��������");
        }

        [Test]
        public void ParamsMatch()
        {
            var _params = new StringDictionary();
            _params.Add("deviceTypeId", "cellphone");
            var uriMatcher = new UriMatcher(
                "customers/products/mobile/services/details/{tariff}/",
                new Dictionary<string, string>(),
                _params);
            var uri =
                new Uri(
                    "http://moskva.beeline.ru/"
                    + "customers/products/mobile/services/details/highway-1gb-nedelya-besplatno/?deviceTypeId=cellphone");
            var uriMatchResult = uriMatcher.Match(uri, "/");
            Assert.True(uriMatchResult.Success, "���������� ���� Params �� ����������� ��� ������������� ��������");
        }
    }
}
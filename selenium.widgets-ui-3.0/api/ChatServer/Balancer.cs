using System;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;

namespace selenium.widget.v3.api.ChatServer
{
    public class Balancer
    {
        public string Host { get; }

        public Balancer(string host)
        {
            Host = host;
        }

        public string RequestChatServerEndpoint(string login)
        {
            var builder = new UriBuilder("http", Host, 80);
            builder.Query = $@"sender=operator&action=getserver&login={login}&protocol=0.1.0";
            using (var webClient = new WebClient())
            {
                string responseXml = webClient.DownloadString(builder.Uri);
                return ReadEndPoint(responseXml);
            }
        }

        private string ReadEndPoint(string balancerRresponseXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(balancerRresponseXml);
            var xPathNavigator = doc.CreateNavigator();
            XPathNavigator ipNode = xPathNavigator.SelectSingleNode("//server_ip");
            XPathNavigator portNode = xPathNavigator.SelectSingleNode("//server_port");
            if (ipNode == null || portNode == null)
                return null;
            return BuildEndPointUrl(ipNode.Value, portNode.Value);
        }

        private string BuildEndPointUrl(string ip, string port)
        {
            return $@"ws://{ip}:{port}";
        }
    }

    [TestFixture]
    public class BalancerTest
    {
        [TestCase("aleksey.v%40livetex.ru")]
        [TestCase("aleksey.v@livetex.ru")]
        public void GetChatServerEndpoint(string login)
        {
            // .Arrange
            var balancer = new Balancer("balancer-cloud.global.livetex");
            // .Act
            string endPointUrl = balancer.RequestChatServerEndpoint(login);
            // .Assert
            Assert.AreEqual(endPointUrl, "ws://action-1.unstable.livetex:19090");
        }
    }
}
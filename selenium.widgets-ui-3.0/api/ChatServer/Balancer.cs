namespace Selenium.Widget.v3.API.ChatServer
{
    using System;
    using System.Net;
    using System.Xml;

    using NUnit.Framework;

    public class Balancer
    {
        public Balancer(string host)
        {
            this.Host = host;
        }

        public string Host { get; }

        public string RequestChatServerEndpoint(string login)
        {
            var builder = new UriBuilder("http", this.Host, 80);
            builder.Query = $@"sender=operator&action=getserver&login={login}&protocol=0.1.0";
            using (var webClient = new WebClient())
            {
                var responseXml = webClient.DownloadString(builder.Uri);
                return this.ReadEndPoint(responseXml);
            }
        }

        private string ReadEndPoint(string balancerRresponseXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(balancerRresponseXml);
            var xPathNavigator = doc.CreateNavigator();
            var ipNode = xPathNavigator.SelectSingleNode("//server_ip");
            var portNode = xPathNavigator.SelectSingleNode("//server_port");
            if (ipNode == null || portNode == null)
            {
                return null;
            }
            return this.BuildEndPointUrl(ipNode.Value, portNode.Value);
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
            var endPointUrl = balancer.RequestChatServerEndpoint(login);
            // .Assert
            Assert.AreEqual(endPointUrl, "ws://action-1.unstable.livetex:19090");
        }
    }
}
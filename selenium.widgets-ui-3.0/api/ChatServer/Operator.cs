using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;
using WebSocketSharp;

namespace selenium.widget.v3.api.ChatServer
{
    public class Operator
    {
        public string BalancerUrl { get; }
        public string Login { get; }
        private WebSocket _webSocket;
        private List<string> _messages; 

        public Operator(string login, string balancerUrl)
        {
            BalancerUrl = balancerUrl;
            Login = login;
            _messages = new List<string>();
            Connect();
        }

        //        operatorName=Admin
        //login = widgets.auto@yandex.ru
        //  password = 1qaz2wsx3edc
        //    customerCare = https://customer-care.env-03.testing-02/
        //apiv2=apiv2.env-03.testing-02
        //balancer=balancer.livetex.build1
        //siteId = 10009394

        private void Connect()
        {
            _webSocket = new WebSocket(BalancerUrl);
            _webSocket.OnMessage += OnMessage;
            try
            {
                _webSocket.Connect();
            }
            catch (Exception e)
            {
                throw new Exception($@"Unable to connect to {BalancerUrl}", e);
            }
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            _messages.Add(e.Data);
            Console.WriteLine("OnMessage: " + e.Data);
        }

        private void Send(Object obj)
        {
            _webSocket.Send(JsonConvert.SerializeObject(obj));
        }

        public void Authenticate(string password)
        {
            var request = new
            {
                request = "auth",
                body = new
                {
                    client_version = "272",
                    is_busy = false,
                    os = new
                    {
                        arch = "x64",
                        build = "3.9-1-amd64",
                        name = "",
                        type = "Linux",
                        version = ""
                    },
                    protocol_version = "0.1.0",
                    source = "operator",
                    login = Login,
                    password = password
                }
            };
            Send(request);
        }
    }

    [TestFixture]
    public class OperatorTests {
        [Test]
        public void Authenticate()
        {
            // .Arrange
            var _operator = new Operator("operator_for_chat@tt.tt", "ws://balancer.livetex.build1xxx");
            // .Act
            _operator.Authenticate("1qaz2wsx");
            // .Assert
        }
    }
}
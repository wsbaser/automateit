using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;
using WebSocketSharp;

namespace selenium.widget.v3.api.ChatServer
{
    public class Operator
    {
        public string Login { get; }
        private WebSocket _webSocket;
        private readonly Balancer _balancer;
        private readonly List<string> _messages;

        public Operator(string login, Balancer balancer)
        {
            _balancer = balancer;
            Login = login;
            _messages = new List<string>();
            Connect();
        }

        private WebSocket CreateSocket(Balancer balancer)
        {
            string chatServerEndPointUrl = balancer.RequestChatServerEndpoint(Login);
            var webSocket = new WebSocket(chatServerEndPointUrl);
            webSocket.OnError += OnError;
            webSocket.OnClose += OnClose;
            webSocket.OnOpen += OnOpen;
            webSocket.OnMessage += OnMessage;
            return webSocket;
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine($@"Socket closed: Code {e.Code}, Reason {e.Reason}, WasClean {e.WasClean}");
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($@"Socket error: {e.Message}");
        }

        private void OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("Socket closed");
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
            if (_webSocket == null)
            {
                _webSocket = CreateSocket(_balancer);
            }
            try
            {
                _webSocket.Connect();
            }
            catch (Exception e)
            {
                throw new Exception($@"Unable to connect to {_webSocket.Url}", e);
            }
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            _messages.Add(e.Data);
            Console.WriteLine("OnMessage: " + e.Data);
        }

        private void Send(Object messageObj)
        {
            string messageString = JsonConvert.SerializeObject(messageObj);
            _webSocket.Send(messageString);
        }

        public ChatServerResponse? Authenticate(string password)
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
            return WaitForMessage(10000);
        }

        private ChatServerResponse? WaitForMessage(int waitTime)
        {
            int oldLength = _messages.Count;
            int i = 0;
            int pollingTime = 500;
            int iterationsCount = waitTime/pollingTime;
            while (oldLength == _messages.Count)
            {
                if (i++ >= iterationsCount)
                {
                    return null;
                }
                Thread.Sleep(pollingTime);
            }
            string receivedMessage = _messages.Last();
            return ParseResponseMessage(receivedMessage);
        }

        private ChatServerResponse ParseResponseMessage(string message)
        {
            return JsonConvert.DeserializeObject<ChatServerResponse>(message);
        }

//
//        private ArraySegment<byte> EncodeMessage(object messageObj)
//        {
//            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageObj));
//            return new ArraySegment<byte>(bytes);
//        }
//
//        private dynamic DecodeMessage(ArraySegment<byte> buffer)
//        {
//            string messageString = Encoding.UTF8.GetString(buffer.Array);
//            Console.WriteLine("Message received: "+messageString);
//            return JsonConvert.DeserializeObject(messageString);
//        }

//        private async Task<dynamic> ReadAsync()
//        {
//            byte[] buffer = new byte[1024];
//            var buffer = new ArraySegment<byte>();
//            var result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
//            if (result.MessageType ==  CloseStatus == WebSocketCloseStatus.Empty)
//            {
//                return DecodeMessage(buffer);
//            }
//            throw new Exception("No connection");
//        }
    }

    public struct ChatServerResponse
    {
        public string response;
        public int errno;
        public string errtext;
        public object body;
    }

    [TestFixture]
    public class OperatorTests
    {
        [Test]
        public void Authenticate()
        {
            // .Arrange
            var balancer = new Balancer("balancer-cloud.global.livetex");
            var _operator = new Operator("test1@test.test", balancer);
            // .Act
            ChatServerResponse? response = _operator.Authenticate("159236");
            // .Assert
            Assert.IsNotNull(response, "Chat Server did not respond");
            Assert.AreEqual(0, ((ChatServerResponse) response).errno, "Authentication error");
            Assert.AreEqual("OK",((ChatServerResponse) response).errtext, "Authentication error");
            Assert.AreEqual("chat_list", ((ChatServerResponse) response).response, "Invalid response");
        }
    }
}
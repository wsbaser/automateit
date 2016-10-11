namespace Selenium.Widget.v3.API.ChatServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using WebSocketSharp;

    public class Operator
    {
        private readonly Balancer _balancer;

        private readonly List<string> _messages;

        private WebSocket _webSocket;

        public Operator(string login, Balancer balancer)
        {
            this._balancer = balancer;
            this.Login = login;
            this._messages = new List<string>();
            this.Connect();
        }

        public string Login { get; }

        private WebSocket CreateSocket(Balancer balancer)
        {
            var chatServerEndPointUrl = balancer.RequestChatServerEndpoint(this.Login);
            var webSocket = new WebSocket(chatServerEndPointUrl);
            webSocket.OnError += this.OnError;
            webSocket.OnClose += this.OnClose;
            webSocket.OnOpen += this.OnOpen;
            webSocket.OnMessage += this.OnMessage;
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
            if (this._webSocket == null)
            {
                this._webSocket = this.CreateSocket(this._balancer);
            }
            try
            {
                this._webSocket.Connect();
            }
            catch (Exception e)
            {
                throw new Exception($@"Unable to connect to {this._webSocket.Url}", e);
            }
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            this._messages.Add(e.Data);
            Console.WriteLine("OnMessage: " + e.Data);
        }

        private void Send(object messageObj)
        {
            var messageString = JsonConvert.SerializeObject(messageObj);
            this._webSocket.Send(messageString);
        }

        public ChatServerResponse? Authenticate(string password)
        {
            var request =
                new
                    {
                        request = "auth",
                        body =
                            new
                                {
                                    client_version = "272",
                                    is_busy = false,
                                    os =
                                        new { arch = "x64", build = "3.9-1-amd64", name = "", type = "Linux", version = "" },
                                    protocol_version = "0.1.0",
                                    source = "operator",
                                    login = this.Login,
                                    password
                                }
                    };
            this.Send(request);
            return this.WaitForMessage(10000);
        }

        private ChatServerResponse? WaitForMessage(int waitTime)
        {
            var oldLength = this._messages.Count;
            var i = 0;
            var pollingTime = 500;
            var iterationsCount = waitTime / pollingTime;
            while (oldLength == this._messages.Count)
            {
                if (i++ >= iterationsCount)
                {
                    return null;
                }
                Thread.Sleep(pollingTime);
            }
            var receivedMessage = this._messages.Last();
            return this.ParseResponseMessage(receivedMessage);
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
        public void Disconnect()
        {
            this._webSocket.Close();
        }

        public ChatServerResponse? GoOffline()
        {
            throw new NotImplementedException();
        }
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
        [TearDown]
        public void TearDown()
        {
            this._operator.Disconnect();
        }

        private Operator _operator;

        private ChatServerResponse? Authenticate(string login = "test1@test.test")
        {
            var balancer = new Balancer("balancer-cloud.global.livetex");
            this._operator = new Operator(login, balancer);
            return this._operator.Authenticate("159236");
        }

        [Test]
        public void SuccessfullAuthentication()
        {
            // .Arrange
            // .Act
            var response = this.Authenticate();
            // .Assert
            Assert.IsNotNull(response, "Chat Server did not respond");
            Assert.AreEqual(0, ((ChatServerResponse)response).errno, "Authentication error");
            Assert.AreEqual("OK", ((ChatServerResponse)response).errtext, "Authentication error");
            Assert.AreEqual("chat_list", ((ChatServerResponse)response).response, "Invalid response");
        }
    }
}
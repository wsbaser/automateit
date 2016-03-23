using System;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace selenium.widget.v3.api.apiv2
{
    public class APIv2
    {
        public APIv2AccountSettings AccountSettings { get; }

        public APIv2(string host, string login, string password)
        {
            AccountSettings = new APIv2AccountSettings(host, login, password);
        }
    }

    public class APIHttpClient
    {
        private readonly WebClient _webClient;

        public APIHttpClient()
        {
            _webClient = new WebClient();
        }

        public string SendGetRequestWithBasicAuth(string url, string login, string password)
        {
            url = url.ToLower();
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(login+ ":" + password));
            _webClient.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            _webClient.Headers["x-livetex-development"] = "1";
            try
            {
                return _webClient.DownloadString(url);
            }
            catch (WebException)
            {
                return null;
            }

//            URL mUrl = new URL(url);
//            URLConnection urlConnection = mUrl.openConnection();
//            urlConnection.setRequestProperty("x-livetex-development", "1");
//
//            if (login != null && password != null)
//            {
//                String authString = login + ":" + password;
//                byte[] authEncBytes = Base64.encodeBase64(authString.getBytes());
//                String authStringEnc = new String(authEncBytes);
//                urlConnection.setRequestProperty("Authorization", "Basic " + authStringEnc);
//
//            }
//
//            InputStream is = urlConnection.getInputStream();
//            BufferedReader br = new BufferedReader(new InputStreamReader(is));
//            String inputStr;
//
//            String result = "";
//            while ((inputStr = br.readLine()) != null)
//            {
//                result += inputStr;
//            }
//
//            return result;
        }

    }

    public class APIWebsocketClient
    {
        public APIWebsocketClient()
        {
        }
    }

    public abstract class APIAdapterBase
    {
        public string Host { get; }
        public string Login { get; }
        public string Password { get; }
        private APIHttpClient _httpClient;
        public APIHttpClient HttpClient => _httpClient ?? (_httpClient = new APIHttpClient());

        private APIWebsocketClient _websocketClient;
        public APIWebsocketClient WebsockerClient => _websocketClient ?? (_websocketClient = new APIWebsocketClient());

        protected APIAdapterBase(string host, string login, string password)
        {
            Host = host;
            Login = login;
            Password = password;
        }
    }


    public abstract class APIV2AdapterBase : APIAdapterBase
    {
        public APIV2AdapterBase(string host, string login, string password) : base(host, login, password)
        {
        }

        protected string FormatRequestUrl(string pathAndQuery)
        {
            return $@"http://{Host}/v2" + pathAndQuery;
        }
    }

    public class APIv2AccountSettings : APIV2AdapterBase
    {

        public APIv2AccountSettings(string host, string login, string password) : base(host, login, password)
        {
        }

        public string SetIsFileTransfer(bool isFileTransfer)
        {
            string requestUrl = FormatRequestUrl($@"/settings/updateFileTransferSettings?is_active={isFileTransfer}");
            return HttpClient.SendGetRequestWithBasicAuth(requestUrl, Login,Password);
        }      
    }

    [TestFixture]
    public class APIv2AccountSettingsTests
    {
        [Test]
        public void SetIsFileTransferIsSuccessfull()
        {
            // .Arrange
            var api = new APIv2AccountSettings("apiv2.env-14.testing-02", "test1@test.test", "159236");
            // .Act
            string response = api.SetIsFileTransfer(true);
            // .Assert
            Assert.IsNotEmpty(response);
        }
    }

}
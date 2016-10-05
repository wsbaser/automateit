namespace Selenium.Widget.v3.API.APIv2
{
    using System;
    using System.Net;
    using System.Text;

    using NUnit.Framework;

    public class APIv2
    {
        public APIv2(string host, string login, string password)
        {
            this.AccountSettings = new APIv2AccountSettings(host, login, password);
        }

        public APIv2AccountSettings AccountSettings { get; }
    }

    public class APIHttpClient
    {
        private readonly WebClient _webClient;

        public APIHttpClient()
        {
            this._webClient = new WebClient();
        }

        public string SendGetRequestWithBasicAuth(string url, string login, string password)
        {
            url = url.ToLower();
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(login + ":" + password));
            this._webClient.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            this._webClient.Headers["x-livetex-development"] = "1";
            try
            {
                return this._webClient.DownloadString(url);
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
    }

    public abstract class APIAdapterBase
    {
        private APIHttpClient _httpClient;

        private APIWebsocketClient _websocketClient;

        protected APIAdapterBase(string host, string login, string password)
        {
            this.Host = host;
            this.Login = login;
            this.Password = password;
        }

        public string Host { get; }

        public string Login { get; }

        public string Password { get; }

        public APIHttpClient HttpClient => this._httpClient ?? (this._httpClient = new APIHttpClient());

        public APIWebsocketClient WebsockerClient
            => this._websocketClient ?? (this._websocketClient = new APIWebsocketClient());
    }

    public abstract class APIV2AdapterBase : APIAdapterBase
    {
        public APIV2AdapterBase(string host, string login, string password)
            : base(host, login, password)
        {
        }

        protected string FormatRequestUrl(string pathAndQuery)
        {
            return $@"http://{this.Host}/v2" + pathAndQuery;
        }
    }

    public class APIv2AccountSettings : APIV2AdapterBase
    {
        public APIv2AccountSettings(string host, string login, string password)
            : base(host, login, password)
        {
        }

        public string SetIsFileTransfer(bool isFileTransfer)
        {
            var requestUrl = this.FormatRequestUrl($@"/settings/updateFileTransferSettings?is_active={isFileTransfer}");
            return this.HttpClient.SendGetRequestWithBasicAuth(requestUrl, this.Login, this.Password);
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
            var response = api.SetIsFileTransfer(true);
            // .Assert
            Assert.IsNotEmpty(response);
        }
    }
}
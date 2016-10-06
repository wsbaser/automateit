//using System.Threading;
//using NUnit.Framework;
//using Selenium.Core.Exceptions;
//
//namespace Selenium.Core.Auxiliary {
//    public class MailHelper {
//        public static void DeleteMessages(string email) {
//            var password = MailAccounts.GetPassword(email);
//            if (string.IsNullOrEmpty(password))
//                throw Throw.TestException("В конфиге отсутствует пароль для email '{0}'", email);
//            var client = GMailClient.Connect(email, password);
//            client.DeleteAll();
//        }
//
//        public static string GetMessage(string email, string titlePattern) {
//            var client = CreateClient(email);
//            string letter = null;
//            for (int i = 0; i < 300; i++) {
//                letter = client.GetLetter(titlePattern);
//                if (letter != null)
//                    break;
//                Thread.Sleep(900);
//            }
//            return letter;
//        }
//
//        private static GMailClient CreateClient(string email) {
//            var password = MailAccounts.GetPassword(email);
//            if (string.IsNullOrEmpty(password))
//                throw Throw.TestException("В конфиге отсутствует пароль для email '{0}'", email);
//            var client = GMailClient.Connect(email, password);
//            return client;
//        }
//    }
//
//    [TestFixture]
//    public class MailHelperTests {
//        [Test]
//        public void DeleteAllTest() {
//            MailHelper.DeleteMessages("quantumart.selenium@gmail.com");
//        }
//    }
//
//}

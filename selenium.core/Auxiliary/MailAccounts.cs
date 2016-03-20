//using System.Xml;
//using System.Xml.XPath;
//
//namespace selenium.core.Auxiliary {
//    public static class MailAccounts {
//        private static readonly XPathNavigator _doc;
//
//        static MailAccounts() {
//            var doc = new XmlDocument();
//            doc.Load("Configs/mail_accounts.xml");
//            _doc = doc.CreateNavigator();
//        }
//
//        public static string GetPassword(string email) {
//            XPathNavigator node = _doc.SelectSingleNode(string.Format("//account[@login='{0}']", email));
//            if (node == null)
//                return null;
//            return node.GetAttribute("password", string.Empty);
//        }
//    }
//}

// TODO: Use System.Xml.XmlDictionaryReader.CreateTextReader(System.IO.Stream,System.Xml.XmlDictionaryReaderQuotas), and poll whether null to detect if it has been disposed. (Close() is deprecated.).
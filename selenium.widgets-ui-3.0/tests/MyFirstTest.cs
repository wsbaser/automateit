using NUnit.Framework;
using selenium.widget.v3.service.pages;
using selenium.widget.v3.tests.@base;

namespace selenium.widget.v3.tests
{
    [TestFixture]
    public class MyFirstTest : PageWithWidgetTestBase<PageWithWidget>
    {

        public void SetUp()
        {
            //        InvitationHelper inviteHelper = new InvitationHelper();
            //        inviteHelper.deleteAllInvitations();
            //
            //        OfflineHelper offlineHelper = new OfflineHelper();
            //        offlineHelper.setOfflineContactType(CONTACT_EMAIL_AND_PHONE, REQUIRED_EMAIL);
        }

        [TestCaseSource(typeof(Mother),"INVALID_EMAILS")]
        public void SendValidEmail(string email)
        {
            // . Arrange
            var page = GoToPageWithWidget(117166);
            // . Act
            //page.WidgetLabel
            // . Assert
//            label.clickLabel();
//            widget.fillName(NAME)
//                .fillEmail(email)
//                .fillPhone(PHONE)
//                .fillMessage(MESSAGE)
//                .send();
//            Assert.assertEquals(OFFLINE_SUCCESS_MESSAGE, offline.getOfflineSentMessage());
        }
    }
}
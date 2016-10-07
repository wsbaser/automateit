namespace Selenium.Widget.v3.Tests
{
    using NUnit.Framework;

    using Selenium.Widget.v3.Mother;
    using Selenium.Widget.v3.Service.Pages;
    using Selenium.Widget.v3.Tests.Base;

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

        [Test, TestCaseSource(typeof(Mother), "VALID_EMAILS")]
        public void SendValidEmail(string email)
        {
            // . Arrange
            var page = this.GoToPageWithWidget(117166);
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
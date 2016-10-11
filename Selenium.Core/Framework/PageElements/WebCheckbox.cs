namespace Selenium.Core.Framework.PageElements
{
    using NUnit.Framework;

    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;

    public class WebCheckbox : SimpleWebComponent, IWebCheckbox
    {
        public WebCheckbox(IPage parent, By by)
            : base(parent, by)
        {
        }

        public WebCheckbox(IPage parent, string rootScss)
            : base(parent, rootScss)
        {
        }

        /// <summary>
        ///     ���������� ����� � ��������
        /// </summary>
        public void Select()
        {
            if (!this.Is.Checked(this.By))
            {
                this.Log.Action("������������� ������� {0}", this.ComponentName);
                this.Action.Click(this.By);
            }
        }

        /// <summary>
        ///     ����� ����� �� ��������
        /// </summary>
        public void Deselect()
        {
            if (this.Checked())
            {
                this.Log.Action("������� ������� {0}", this.ComponentName);
                this.Action.Click(this.By);
            }
        }

        public bool Checked()
        {
            return this.Is.Checked(this.By);
        }

        public void AssertIsDisabled()
        {
            Assert.AreEqual("disabled", this.Get.Attr(this.By, "disabled"), "������� �������");
        }
    }

    public interface IWebCheckbox
    {
        void Select();

        void Deselect();

        bool Checked();
    }
}
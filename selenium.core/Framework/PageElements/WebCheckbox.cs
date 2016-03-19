using NUnit.Framework;
using OpenQA.Selenium;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
    public class WebCheckbox : SimpleWebComponent, IWebCheckbox {
        public WebCheckbox(IPage parent, By by)
            : base(parent, by) {
        }

        public WebCheckbox(IPage parent, string rootScss)
            : base(parent, rootScss) {
        }

        /// <summary>
        /// Установить галку в чекбоксе
        /// </summary>
        public void Select() {
            if (!Is.Checked(By)) {
                Log.Action("Устанавливаем чекбокс {0}", ComponentName);
                Action.Click(By);
            }
        }

        /// <summary>
        /// Снять галку из чекбокса
        /// </summary>
        public void Deselect() {
            if (Checked()) {
                Log.Action("Снимаем чекбокс {0}", ComponentName);
                Action.Click(By);
            }
        }

        public bool Checked() {
            return Is.Checked(By);
        }

        public void AssertIsDisabled() {
            Assert.AreEqual("disabled", Get.Attr(By, "disabled"), "Чекбокс активен");
        }
    }

    public interface IWebCheckbox {
        void Select();
        void Deselect();
        bool Checked();
    }
}
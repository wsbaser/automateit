using OpenQA.Selenium;
using selenium.core.Framework.Page;

namespace selenium.core.Framework.PageElements {
    public class WebImage:SimpleWebComponent {
        public WebImage(IPage parent, By @by) : base(parent, @by) {
        }

        /// <summary>
        /// Получает имя файла из атрибута src элемента img
        /// </summary>
        public string GetFileName() {
            return Get.ImgFileName(By);
        }
    }
}
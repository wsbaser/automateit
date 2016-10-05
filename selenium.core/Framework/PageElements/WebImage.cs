namespace Selenium.Core.Framework.PageElements
{
    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;

    public class WebImage : SimpleWebComponent
    {
        public WebImage(IPage parent, By @by)
            : base(parent, @by)
        {
        }

        /// <summary>
        ///     Получает имя файла из атрибута src элемента img
        /// </summary>
        public string GetFileName()
        {
            return this.Get.ImgFileName(this.By);
        }
    }
}
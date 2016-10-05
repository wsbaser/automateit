namespace Selenium.Core.Framework.PageElements
{
    using NUnit.Framework;

    using OpenQA.Selenium;

    using Selenium.Core.Framework.Page;
    using Selenium.Core.TestData;

    public class WebInput : SimpleWebComponent
    {
        public WebInput(IPage parent, By @by)
            : base(parent, @by)
        {
        }

        public WebInput(IPage parent, string rootScss)
            : base(parent, rootScss)
        {
        }

        public virtual string Text
        {
            get
            {
                return this.Get.InputValue(this.By);
            }
        }

        /// <summary>
        ///     Ввести значение в поле ввода
        /// </summary>
        /// <param name="value">значение</param>
        public virtual void TypeIn(object value)
        {
            this.Log.Action("Вводим '{0}' в поле '{1}'", value, this.ComponentName);
            this.Action.TypeIn(this.By, value);
        }

        /// <summary>
        ///     Проверить что поле ввода не содержит текста
        /// </summary>
        public void AssertIsEmpty()
        {
            Assert.IsTrue(this.IsEmpty(), "Поле '{0}' не пустое", this.ComponentName);
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(this.Text);
        }

        /// <summary>
        ///     Проверить что поле ввода содержит текст
        /// </summary>
        public void AssertIsNotEmpty()
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(this.Text), string.Format("Поле '{0}' пустое", this.ComponentName));
        }

        /// <summary>
        ///     Заполнить поле произвольными цифрами
        /// </summary>
        public string TypeInRandomNumber(int length = 10)
        {
            var random = RandomDataHelper.Cifers(length);
            this.TypeIn(random);
            return random;
        }

        /// <summary>
        ///     Удалить последний символ в поле ввода
        /// </summary>
        public void RemoveLast(bool changeFocus = false)
        {
            this.Action.PressKey(this.By, Keys.End);
            this.Action.PressKey(this.By, Keys.Backspace);
            if (changeFocus)
            {
                this.Action.ChangeFocus();
            }
        }

        /// <summary>
        ///     Очистить текстовое поле
        /// </summary>
        public virtual void Clear()
        {
            this.Action.Clear(this.By);
        }

        public void TypeInAndSubmit(string query)
        {
            this.TypeIn(query);
            this.Action.PressEnter(this.By);
            this.Wait.WhileAjax();
        }

        public void AssertEqual(string expected)
        {
            Assert.AreEqual(expected, this.Text, "Некоректное значение в поле '{0}'", this.ComponentName);
        }
    }
}
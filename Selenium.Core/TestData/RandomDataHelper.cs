﻿namespace Selenium.Core.TestData
{
    using System;

    public static class RandomDataHelper
    {
        /// <summary>
        ///     Сгенерировать числовую последовательность указанной длинны
        /// </summary>
        /// <param name="length">длина последовательности</param>
        public static string Cifers(int length = 10)
        {
            var s = string.Empty;
            var random = new Random();
            for (var i = 0; i < length; i++)
            {
                s += random.Next(10);
            }
            return s;
        }
    }
}
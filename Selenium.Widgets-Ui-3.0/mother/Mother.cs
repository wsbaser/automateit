namespace Selenium.Widget.v3.Tests
{
    public static class Mother
    {
        public static object[] VALID_EMAILS =
            {
                new object[] { "a@aa.aa" }, new object[] { "-@ss.ss" },
                new object[] { "1@ss.ss" }, new object[] { "aa@a.aa" },
                new object[] { "vasya.pupkin@test.asd" },
                new object[] { "vasya-pupkin@test.asd" },
                new object[] { "VasyaPupkin@test.asd" },
                new object[] { "vasya_pupkin@test.asd" },
                new object[] { "vasya123@test.asd" },
                new object[] { "vasya@test.tester" },
                new object[] { "vasya@test.asd.com" }
            };
    }
}
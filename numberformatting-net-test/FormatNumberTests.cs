using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using numberformatter_net;

namespace numberformatting_net_test
{
    [TestClass]
    public class FormatNumberTests
    {
        private static void TestResult(string number, string locale, string format, string expect)
        {
            var result = NumberFormatter.FormatNumber(number, new FormatOptions
            {
                Locale = locale,
                Format = format
            });

            Console.WriteLine($"L: {locale} N: {number} F: {format} E: {expect} R: {result}");
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void FormatTextTest1()
        {
            var number = "4335.20";
            var locale = "us";
            var format = "#,###.00";
            var expect = "4,335.20";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest2()
        {
            var number = "4335.20";
            var locale = "us";
            var format = "#,###.0";
            var expect = "4,335.2";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest3()
        {
            var number = "4335.20";
            var locale = "us";
            var format = "#,###.#";
            var expect = "4,335.2";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest4()
        {
            var number = "4335.20";
            var locale = "us";
            var format = "####.00";
            var expect = "4335.20";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest5()
        {
            var number = "4335.20";
            var locale = "us";
            var format = "#,###";
            var expect = "4,335";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest6()
        {
            var number = "4335.80";
            var locale = "us";
            var format = "#,###";
            var expect = "4,336";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest7()
        {
            var number = "4335.80";
            var locale = "us";
            var format = "#";
            var expect = "4336";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest8()
        {
            var number = "556";
            var locale = "us";
            var format = "0000";
            var expect = "0556";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest9()
        {
            var number = "556";
            var locale = "us";
            var format = "0";
            var expect = "556";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest10()
        {
            var number = "556.0";
            var locale = "us";
            var format = "#";
            var expect = "556";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest11()
        {
            var number = "10450213";
            var locale = "us";
            var format = "#,###.00";
            var expect = "10,450,213.00";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest12()
        {
            var number = "-2342.34";
            var locale = "us";
            var format = "-#,###.00";
            var expect = "-2,342.34";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest12A()
        {
            var number = "-2342.34";
            var locale = "us";
            var format = "#,###.00";
            var expect = "-2,342.34";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest13()
        {
            var number = "2342.34";
            var locale = "us";
            var format = "$#,###.00";
            var expect = "$2,342.34";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest13A()
        {
            var number = "2342.34";
            var locale = "us";
            var format = "USD#,###.00";
            var expect = "USD2,342.34";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest13B()
        {
            var number = "2342.34";
            var locale = "us";
            var format = "#,###.00USD";
            var expect = "2,342.34USD";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest13C()
        {
            var number = "2342.34";
            var locale = "us";
            var format = "#,###.00 USD";
            var expect = "2,342.34 USD";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest13D()
        {
            var number = "-2342.34";
            var locale = "us";
            var format = "-$#,###.00";
            var expect = "-$2,342.34";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest13E()
        {
            var number = "-2342.34";
            var locale = "us";
            var format = "$#,###.00";
            var expect = "$-2,342.34";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest14()
        {
            var number = "0.233";
            var locale = "us";
            var format = "#.##%";
            var expect = "23.3%";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest14A()
        {
            var number = "0.233";
            var locale = "us";
            var format = "#.00%";
            var expect = "23.30%";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest15()
        {
            var number = "-434.4343";
            var locale = "us";
            var format = "-#,###.00";
            var expect = "-434.43";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest15A()
        {
            var number = "434.4343";
            var locale = "us";
            var format = "-#,###.00";
            var expect = "434.43";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest16()
        {
            var number = "434.4343";
            var locale = "us";
            var format = "#,###.###";
            var expect = "434.434";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest17()
        {
            var number = "1345234";
            var locale = "us";
            var format = "#,###,###";
            var expect = "1,345,234";

            TestResult(number, locale, format, expect);
        }

        [TestMethod]
        public void FormatTextTest18()
        {
            var number = "1000";
            var locale = "us";
            var format = "##,##";
            var expect = "10,00";

            TestResult(number, locale, format, expect);
        }
    }
}

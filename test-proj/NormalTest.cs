using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_proj
{
    internal class NormalTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestMatch()
        {
            string pattern = "ael";
            string str1 = "abcelsdf";
            string str2 = "slcaeafl";
            string str3 = "sscaaleee;a";
            string str4 = "ael";
            Assert.Warn("IsMatch {0} {1}", str1, MathName(str1, pattern));
            Assert.Warn("IsMatch {0} {1}", str2, MathName(str2, pattern));
            Assert.Warn("IsMatch {0} {1}", str3, MathName(str3, pattern));
            Assert.Warn("IsMatch {0} {1}", str4, MathName(str4, pattern));
        }

        private bool MathName(string str, string pattern)
        {
            if (str is null)
                return false;

            if (string.IsNullOrEmpty(pattern))
                return true;

            int index = 0;
            foreach (var item in str)
            {
                if (item == pattern[index])
                {
                    index++;
                    if (index >= pattern.Length)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

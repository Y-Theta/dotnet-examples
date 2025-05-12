using expressionabout;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_proj
{
    class expressionTest
    {
        [SetUp]
        public void Setup()
        {

        }

        public delegate T Function<T>(string input);

        [Test]
        public void TestScriptRunner()
        {
            string input = "ret";
            int test = 1;

            var func = ScriptRunner.GenerateExpression<Func<int, int, int>>(" (int)Math.Pow((int){0}, (int){1}) ");
            var result = func?.Invoke(2, 2);

            Assert.AreEqual(result, test);
        }

    }
}

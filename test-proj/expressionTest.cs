using expressionabout;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void TestPropQuickAccess()
        {
            PropQuickAccess.Init();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            PropQuickAccess.GetByReflection();
            sw.Stop();
            Assert.Warn($" Reflection {sw.ElapsedMilliseconds}");
            sw.Restart();
            PropQuickAccess.GetByExpression();
            sw.Stop();
            Assert.Warn($" Expression {sw.ElapsedMilliseconds}");
            sw.Restart();
            PropQuickAccess.GetByReflection();
            sw.Stop();
            Assert.Warn($" Reflection Assign {sw.ElapsedMilliseconds}");
            sw.Restart();
            PropQuickAccess.GetByExpression();
            sw.Stop();
            Assert.Warn($" Expression Assign {sw.ElapsedMilliseconds}");
        }

        [Test]
        public void TestScriptRunnerFunc1()
        {
            string input = "ret";

            var func = ScriptRunner.GenerateExpression<Func<string>>($"return $\"{input}2\";");
            var result = func?.Invoke();

            Assert.AreEqual(result, input + "2");
        }

        [Test]
        public void TestScriptRunnerFunc2()
        {
            string input = "ret";

            var func = ScriptRunner.GenerateExpression<Func<string, string>>("return $\"{{{0}}}2\";");
            var result = func?.Invoke(input);

            Assert.AreEqual(result, input + "2");
        }

    }
}

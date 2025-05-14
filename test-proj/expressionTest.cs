using expressionabout;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_proj
{
    class expressionTest
    {
        [SetUp]
        public void Setup()
        {
            PropQuickAccess.Init();
        }

        public delegate T Function<T>(string input);

        [Test]
        public void TestWin()
        {
            //Application.Run(new Form());
            Assert.Pass();
        }

        /// <summary>
        /// Reflection 在.net8中被优化过 差距不是很大
        /// </summary>
        [Test]
        public void TestPropQuickAccess()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                PropQuickAccess.GetByReflection();
                sw.Stop();
                Assert.Warn($" Reflection {sw.ElapsedMilliseconds}");
            }));
            tasks.Add(Task.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                PropQuickAccess.GetByExpression();
                sw.Stop();
                Assert.Warn($" Expression {sw.ElapsedMilliseconds}");
            }));
            tasks.Add(Task.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                PropQuickAccess.GetByReflection();
                sw.Stop();
                Assert.Warn($" Reflection Assign {sw.ElapsedMilliseconds}");
            }));
            tasks.Add(Task.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                PropQuickAccess.GetByExpression();
                sw.Stop();
                Assert.Warn($" Expression Assign {sw.ElapsedMilliseconds}");
            }));
            Task.WaitAll(tasks.ToArray());
            Task.Delay(200).Wait();
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

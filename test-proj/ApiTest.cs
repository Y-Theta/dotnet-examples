using ApiClients;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_proj
{
    
    internal class ApiTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestFirefly1()
        {
            Program.Main();
        }
    }
}

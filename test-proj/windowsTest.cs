using form_test;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test_proj
{
    internal class windowsTest
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        [STAThread]
        [Apartment(ApartmentState.STA)]
        public static void TencentDNSTool()
        {
            TencentDNSTool window = new TencentDNSTool();
            window.ShowDialog();
        }
    }
}

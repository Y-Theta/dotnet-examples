using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace form_test
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            TencentDNSTool window = new TencentDNSTool();
            window.ShowDialog();

            //Application.Run(new TestForm());
        }
    }
}

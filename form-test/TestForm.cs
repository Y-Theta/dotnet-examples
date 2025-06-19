using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace form_test
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            panel1.LayoutItems.Add(panel2);
            panel1.LayoutItems.Add(panel3);
            panel1.LayoutItems.Add(panel4);

            panel4.MouseDown += Panel4_MouseHover;
        }

        private void Panel4_MouseHover(object? sender, EventArgs e)
        {
            this.panel1.EnableAlter = !this.panel1.EnableAlter;
            this.panel1.PerformLayout();
        }

        private void panel3_MouseHover(object sender, EventArgs e)
        {
            this.panel1.EnableAlter = !this.panel1.EnableAlter;
            this.panel1.PerformLayout();
        }
    }
}

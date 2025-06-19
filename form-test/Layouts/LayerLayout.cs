using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace form_test.Layouts
{
    internal class LayerLayout : Panel
    {
        public bool EnableAlter { get; set; } = false;

        public List<Control> LayoutItems { get; } = new List<Control>();

        public override LayoutEngine LayoutEngine => new MyLayoutEngine();
    }


    internal class MyLayoutEngine : LayoutEngine
    {

        public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
        {
            LayerLayout parent = container as LayerLayout;

            // Use DisplayRectangle so that parent.Padding is honored.
            Rectangle parentDisplayRectangle = parent.DisplayRectangle;
            Point nextControlLocation = parentDisplayRectangle.Location;

            Control lefttop = null;
            Control main = null;
            Control alter = null;
            foreach (var item in parent.LayoutItems)
            {
                if (item is Control ctl)
                {
                    if (ctl.Tag?.ToString() == "0")
                    {
                        lefttop = ctl;
                    }
                    else if (ctl.Tag?.ToString() == "1")
                    {
                        main = ctl;
                    }
                    else if (ctl.Tag?.ToString() == "2")
                    {
                        alter = ctl;
                    }
                }
            }

            if (lefttop != null && !parent.EnableAlter)
            {
                if (alter != null)
                {
                    alter.Visible = false;
                }
                lefttop.Visible = true;
                lefttop.Location = new Point(parentDisplayRectangle.Left + 20, parentDisplayRectangle.Top + 20);
                int w = 40, h = 40;
                if (parentDisplayRectangle.Size.Width < 60)
                {
                    w = parentDisplayRectangle.Width - 20 - 4;
                }
                if (parentDisplayRectangle.Size.Height < 60)
                {
                    h = parentDisplayRectangle.Height - 20 - 4;
                }
                w = Math.Max(w, 0);
                h = Math.Max(h, 0);
                int min = Math.Min(w, h);
                lefttop.Size = new Size(min, min);
            }

            if (alter != null && parent.EnableAlter)
            {
                if (lefttop != null)
                {
                    lefttop.Visible = false;
                }
                alter.Visible = true;
                alter.Location = new Point(parentDisplayRectangle.Left + 20, parentDisplayRectangle.Top + 20);
                int w = 40, h = parentDisplayRectangle.Height - 40;
                if (parentDisplayRectangle.Size.Width < 60)
                {
                    w = parentDisplayRectangle.Width - 20 - 4;
                }
                w = Math.Max(w, 0);
                alter.Size = new Size(w, h);
            }

            if (main != null)
            {
                main.Location = nextControlLocation;
                main.Size = parentDisplayRectangle.Size;
            }

            return true;
        }

    }
}

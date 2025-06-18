using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace form_test.Utils
{
    /// <summary>
    /// TextInputContent.xaml 的交互逻辑
    /// </summary>
    public partial class TextInputContent : UserControl, IInputContainer
    {
        public TextInputContent()
        {
            InitializeComponent();
        }

        #region   IInputContainer

        public double? DesiredWidth => 360;

        public double? DesiredHeight => 48;

        UIElement IInputContainer.Content => this;

        public object GetResult()
        {
            return PART_RESULT.Text;
        }
        #endregion

    }
}

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
using System.Windows.Shapes;

namespace form_test.Utils
{
    /// <summary>
    /// ContentWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ContentWindow : Window
    {
        private IInputContainer _container;

        public object Result => _container?.GetResult();

        public ContentWindow()
        {
            InitializeComponent();
        }

        internal void SetContent(IInputContainer content)
        {
            if (content is null)
                return;

            _container = content;
            this.PART_CONTENT.Content = content.Content;
            if (content.DesiredWidth.HasValue)
            {
                this.Width = content.DesiredWidth.Value;
            }
            if (content.DesiredHeight.HasValue)
            {
                this.Height = content.DesiredHeight.Value + 32;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
            //base.OnMouseLeftButtonDown(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

using TencentCloud.Common;
using TencentCloud.Dnspod.V20210323;
using TencentCloud.Dnspod.V20210323.Models;

namespace form_test
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class TencentDNSTool : Window
    {
        private string TENCENT_DNS_KEY;
        private string TENCENT_DNS_SECRET;
        DnspodClient _client;
        readonly List<RecordListItem> _originItems = new List<RecordListItem>();

        public TencentDNSTool()
        {
            InitializeComponent();

            var envs = GetEnvVariables();

            if (envs.TryGetValue(nameof(TENCENT_DNS_KEY), out var key))
            {
                TENCENT_DNS_KEY = key;
            }

            if (envs.TryGetValue(nameof(TENCENT_DNS_SECRET), out var secret))
            {
                TENCENT_DNS_SECRET = secret;
            }

            DNSID.Text = TENCENT_DNS_KEY;
            DNSKEY.Text = TENCENT_DNS_SECRET;
        }

        private IReadOnlyDictionary<string, string> GetEnvVariables()
        {
            var uservariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);
            var systemvariables = Environment.GetEnvironmentVariables();
            Dictionary<string, string> envs = new Dictionary<string, string>();
            foreach (var item in systemvariables.Keys)
            {
                var key = item?.ToString();
                if (key != null)
                {
                    envs[key] = systemvariables[item]?.ToString();
                }
            }

            foreach (var item in uservariables.Keys)
            {
                var key = item?.ToString();
                if (key != null)
                {
                    envs[key] = uservariables[item]?.ToString();
                }
            }

            return envs;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_client != null)
            {
            }

            var crt = new Credential { SecretId = DNSID.Text, SecretKey = DNSKEY.Text };
            _client = new DnspodClient(crt, null);
            var list = await _client.DescribeRecordList(new DescribeRecordListRequest { Domain = "y-theta.cn" });

            _originItems.Clear();
            foreach (var item in list.RecordList)
            {
                if (item.Type != "A")
                    continue;

                _originItems.Add(item);
            }

            Data.ItemsSource = _originItems.ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dc = (sender as Button)?.DataContext as RecordListItem;

        }

        private bool _changing = false;
        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_changing) return;
            try
            {
                _changing = true;
                await Task.Delay(2000);
                OnTextBufferedChanged(sender as TextBox);
            }
            finally
            {
                _changing = false;
            }
        }

        private void OnTextBufferedChanged(TextBox box)
        {
            Debug.WriteLine("2222");
            if (_originItems != null)
            {
                var filtered = _originItems.Where(i => i.Name.Contains(box.Text));
                Data.ItemsSource = filtered.ToList();
            }
        }
    }
}

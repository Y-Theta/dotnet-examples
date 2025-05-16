using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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

            try
            {
                Data.ItemsSource = null;
                var crt = new Credential { SecretId = DNSID.Text, SecretKey = DNSKEY.Text };
                _client = new DnspodClient(crt, null);
                var list = await _client.DescribeRecordList(new DescribeRecordListRequest { Domain = DNSDOMAIN.Text });

                _originItems.Clear();
                foreach (var item in list.RecordList)
                {
                    //if (item.Type != "A")
                    //    continue;

                    _originItems.Add(item);
                }

                Data.ItemsSource = GetOrderedItem(_originItems);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dc = (sender as Button)?.DataContext as RecordListItem;
            if (_client != null)
            {
                try
                {
                    var response = _client.ModifyRecordBatchSync(new ModifyRecordBatchRequest
                    {
                        Change = nameof(dc.Value).ToLower(),
                        ChangeTo = dc.Value,
                        RecordIdList = new ulong?[] { dc.RecordId }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
                var filtered = _originItems.Where(i => IsSubSet(i.Name, box.Text));
                Data.ItemsSource = GetOrderedItem(filtered);
            }
        }

        private IList<RecordListItem> GetOrderedItem(IEnumerable<RecordListItem> items)
        {
            var result = items.ToList();
            result.Sort((a, b) => Shell.StrCmpLogicalW(a.Name, b.Name));
            var list = result.GroupBy(r => r.Type);
            return list.SelectMany(s => s.ToList()).ToList();
        }

        private bool IsSubSet(string source, string pattern)
        {
            if (source is null || string.IsNullOrEmpty(pattern))
                return true;

            if (pattern.Length > source.Length)
                return false;

            int index = 0;
            var sourcelist = source.ToArray();
            var patternlist = pattern.ToArray();
            for (int i = 0; i < sourcelist.Length; i++)
            {
                var c = sourcelist[i];
                if (index >= pattern.Length)
                {
                    return true;
                }
                if (c == patternlist[index])
                {
                    index++;
                }
            }

            if (index == patternlist.Length)
            {
                return true;
            }

            return false;
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dc = (sender as Button)?.DataContext as RecordListItem;

        }
    }

    public class Shell
    {

        public static int StrCmpLogicalW(string x, string y)
        {
            // windows 无法比较这种情况
            if (x == null && y == null)
                return 0;

            return __StrCmpLogicalW(x, y);
        }

        [DllImport("shlwapi.dll", EntryPoint = "StrCmpLogicalW", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int __StrCmpLogicalW(string x, string y);
    }
}

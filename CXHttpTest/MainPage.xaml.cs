using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CXHttpNS;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CXHttpTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Test1()
        {
            textBox.Text += "Test1 Started\n";
            var t = await CXHttp.Connect("http://www.baidu.com").Get();
            var body = await t.Content();

            if (body.Contains("百度"))
            {
                textBox.Text += "Test1 Succeed: Get Baidu.com\n";
            }
            else
            {
                textBox.Text += "Test1 Failed: Get Baidu.com\n";
            }
        }

        private async void Test2()
        {
            textBox.Text += "Test2 Started\n";
            var t = await CXHttp.Connect("http://gw.bnu.edu.cn:803/srun_portal_pc.php?ac_id=1")
                .Data("action", "login")
                .Data("ac_id", "1")
                .Data("username", "学号")
                .Data("password", "密码")
                .Post();
            var body = await t.Content();

            if (body.Contains("成功"))
            {
                textBox.Text += "Test2 Succeed: Post BNU Gateway Login\n";
            }
            else
            {
                textBox.Text += "Test2 Failed: Post BNU Gateway Login\n";
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = "Test started.\n";
            Test1();
            Test2();
        }
    }
}

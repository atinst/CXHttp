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
using System.Text.RegularExpressions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CXHttpTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";

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
            
            if (body.Contains("成功") || body.Contains("Error"))
            {
                textBox.Text += "Test2 Succeed: Post BNU Gateway Login\n";
            }
            else
            {
                textBox.Text += "Test2 Failed: Post BNU Gateway Login\n";
            }
        }

        private async void Test3()
        {
            var URL = "http://cas.bnu.edu.cn/cas/login?service=http%3A%2F%2Fzyfw.bnu.edu.cn%2FMainFrm.html";

            textBox.Text += "Test3 Started\n";

            var t = await CXHttp.Session("zyfw").req
                .Url(URL)
                .Header("User-Agent", USER_AGENT)
                .Get();

            string body = await t.Content();

            Match mc = Regex.Match(body, "input type=\"hidden\" name=\"lt\" value=\"(.*)\"");
            string lt = mc.Groups[1].Value;

            mc = Regex.Match(body, "input type=\"hidden\" name=\"execution\" value=\"(.*)\"");
            string exec = mc.Groups[1].Value;

            
            t = await CXHttp.Session("zyfw").req
                .Url(URL)
                .Header("User-Agent", USER_AGENT)
                .Data("username", "学号")
                .Data("password", "密码")
                .Data("code", "code")
                .Data("lt", lt)
                .Data("execution", exec)
                .Data("_eventId", "submit")
                .Post();

            body = await t.Content("GBK");

            if (body.Contains("注销"))
            {
                textBox.Text += "Test3 Succeed: Session Login BNU 教务\n";
            }
            else
            {
                textBox.Text += "Test3 Failed: Session Login BNU 教务\n";
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = "Test started.\n";
            Test1();
            Test2();
            Test3();
        }
    }
}

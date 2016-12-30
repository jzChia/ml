using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PanGu.Framework;
using PanGu;
using PanGu.Match;
using System.Diagnostics;
using Lucene.Net.Analysis;
using System.IO;
using IKAnalyzerNet;
using Common;
using Common.Entities;
using Common.DAO;

namespace MLearning
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var words = DisplaySegment();
            //var words = Lucene();
            //var words =IKAnalyzerDisplay();
            //outputwords.Text = words;
             //   outputwords.Text += item+"/";

           // UserInfo user = new UserInfo { userName = "jz", passWord = "111111" };

            //UserInfoDB.ImportUserInfo(user);
            //Factor f = new Factor{ factorId="28e2189a-21ec-405b-aba2-361999b7d30e", name="myname",createTime=DateTime.Now,updateTime=DateTime.Now,author="jz"};
           // FactorDB.DeleteFactor(f);

	       
        }

        private string DisplaySegment()
        {
            string textBoxSource = inputwords.Text;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var words = new Segment().DoSegment(textBoxSource);
            stopwatch.Stop();
            string tipswords = "源字符串长度：" + textBoxSource.Length + "  分词时间：" + stopwatch.Elapsed.ToString() + "  分词速度：";
            
            if (stopwatch.ElapsedMilliseconds == 0L)
            {
                tipswords += "无穷大";
            }
            else
            {
                tipswords += ((((long)textBoxSource.Length) / stopwatch.ElapsedMilliseconds) * 1000.0).ToString();
            }
            StringBuilder builder = new StringBuilder();
            foreach (WordInfo info in words)
            {
                if (info != null)
                {
                    builder.AppendFormat("{0}/", info.Word);
                }
            }

            this.TipsWords.Content = tipswords+" char/s";

            return builder.ToString();
        }

        private string Lucene()
        {
            string textBoxSource = inputwords.Text;
            Stopwatch stopwatch = new Stopwatch();
            StringBuilder sb = new StringBuilder();
            sb.Remove(0, sb.Length);
            stopwatch.Start();
            Analyzer analyzer = new Lucene.China.ChineseAnalyzer();
            
            StringReader sr = new StringReader(textBoxSource);
            TokenStream stream = analyzer.TokenStream(null, sr);
            stopwatch.Stop();
            string tipswords = "源字符串长度：" + textBoxSource.Length + "  分词时间：" + stopwatch.Elapsed.ToString() + "  分词速度：";

            if (stopwatch.ElapsedMilliseconds == 0L)
            {
                tipswords += "无穷大";
            }
            else
            {
                tipswords += ((((long)textBoxSource.Length) / stopwatch.ElapsedMilliseconds) * 1000.0).ToString();
            }
            Token t = stream.Next();
            while (t != null)
            {
                string g =  t.TermText();

                sb.Append(g+"/");
                t = stream.Next();
            }
            this.TipsWords.Content = tipswords + " char/s";
            return sb.ToString();
        }

        private string IKAnalyzerDisplay()
        {
            string textBoxSource = inputwords.Text;
            Stopwatch stopwatch = new Stopwatch();
            StringBuilder sb = new StringBuilder();
            sb.Remove(0, sb.Length);
            stopwatch.Start();
            IKAnalyzer ika = new IKAnalyzer();
           
            StringReader sr = new StringReader(textBoxSource);
            TokenStream stream = ika.TokenStream("TestField", sr);
            stopwatch.Stop();
            string tipswords = "源字符串长度：" + textBoxSource.Length + "  分词时间：" + stopwatch.Elapsed.ToString() + "  分词速度：";

            if (stopwatch.ElapsedMilliseconds == 0L)
            {
                tipswords += "无穷大";
            }
            else
            {
                tipswords += ((((long)textBoxSource.Length) / stopwatch.ElapsedMilliseconds) * 1000.0).ToString();
            }
            Token t = stream.Next();
            while (t != null)
            {
                string g = t.TermText();

                sb.Append(g + "/");
                t = stream.Next();
            }
            this.TipsWords.Content = tipswords + " char/s";
            return sb.ToString();
        }

    }
}

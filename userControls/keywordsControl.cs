using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using PanGu;
using Microsoft.VisualBasic;

namespace userControls
{
    public partial class keywordsControl : UserControl
    {
        public event EventHandler savekeywordsClick;
        List<String> words = new List<string>();
        public keywordsControl()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            words = DisplaySegment();
            checkedListBoxControl1.DataSource  = words;           
        }

        private List<String> DisplaySegment()
        {
            string textBoxSource = richTextBox1.Text;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var words = new Segment().DoSegment(textBoxSource);
            stopwatch.Stop();
            string tipswords = String.Format("源字符串长度：\n{0} \n分词时间：\n{1} \n分词速度：\n", textBoxSource.Length, stopwatch.Elapsed);

            if (stopwatch.ElapsedMilliseconds == 0L)
            {
                tipswords += "无穷大";
            }
            else
            {
                tipswords += ((((long)textBoxSource.Length) / stopwatch.ElapsedMilliseconds) * 1000.0).ToString();
            }
            StringBuilder builder = new StringBuilder();
            List<String> wordss = new List<string>();
            foreach (WordInfo info in words)
            {
                if (info != null)
                {
                    builder.AppendFormat("{0}/", info.Word);
                    wordss.Add(info.Word);
                }
            }

            this.labelControl2.Text = tipswords + " char/s";

            return wordss;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string keyword = Interaction.InputBox("请输入需要添加的关键词", "关键词", "", -1, -1);
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                if (words == null)
                    words = new List<string>();
                words.Add(keyword);
                checkedListBoxControl1.DataSource = words;
                checkedListBoxControl1.Refresh();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (savekeywordsClick != null)
                savekeywordsClick(sender, e);
        }

        public List<String> getkeywords()
        {
            List<String> res = new List<string>();
            DevExpress.XtraEditors.BaseCheckedListBoxControl.CheckedItemCollection checkitems = checkedListBoxControl1.CheckedItems;
            
            res.Clear();
            for (int i = 0; i < checkitems.Count; i++)
            {
                res.Add(checkitems[i].ToString());
            }

            return res;
        }
    }
}

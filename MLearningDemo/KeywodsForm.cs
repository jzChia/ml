using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLearningDemo
{
    public partial class KeywodsForm : Form
    {
        public delegate void KeywodsDoReportProgress(List<String> strInfor);
        public KeywodsDoReportProgress keywodsOnReportProgress;
        public KeywodsForm()
        {
            InitializeComponent();
        }

        private void keywordsControl1_savekeywordsClick(object sender, EventArgs e)
        {
            if (keywodsOnReportProgress != null)
                keywodsOnReportProgress( keywordsControl1.getkeywords());
        }
    }
}

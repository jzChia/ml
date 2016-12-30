using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MLCommon.Entities;

namespace MLearningDemo
{
    public partial class InitParaForm : DevExpress.XtraEditors.XtraForm
    {

        private List<Factor> initFactor = new List<Factor>();
        private List<String> initkeywords = new List<string>();
        public InitParaForm(List<Factor> initFactor , List<String> initkeywords)
        {
            InitializeComponent();
            this.initFactor = initFactor;
            this.initkeywords = initkeywords;
        }

        private void InitParaForm_Load(object sender, EventArgs e)
        {
            if (initkeywords != null)
            {
                foreach (var item in initkeywords)
                {
                    richTextBoxkeywords.Text += (item + "/");
                }
            }
            gridControlfactors.DataSource = initFactor;
        }

        private void simpleButtonclose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MLCommon.Entities;
using MLCommon.DAO;

namespace MLearningDemo
{
    public partial class FactorTreeForm : Form
    {
        private List<Factor> fs = new List<Factor>();
        public FactorTreeForm(List<Factor> fs)
        {
            InitializeComponent();
            this.fs = fs;
        }

        private void FactorTreeForm_Load(object sender, EventArgs e)
        {
            selectFactorControl21.initLabelAndSave("", "", false);
            selectFactorControl21.initTreeView2(fs != null ? fs : new List<Factor>());
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.Owner is AddModelForm)
            {
                ((AddModelForm)this.Owner).fs = FactorDB.GetFactorsByFactors(selectFactorControl21.getFactors());
                this.Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}

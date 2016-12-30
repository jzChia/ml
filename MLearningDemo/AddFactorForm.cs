using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MLCommon.Entities;
using DevExpress.XtraEditors;
using MLCommon.DAO;

namespace MLearningDemo
{
    public partial class AddFactorForm : Form
    {
        private Factor factor;
        public AddFactorForm()
        {
            InitializeComponent();
        }
        public AddFactorForm(Factor f)
        {
            this.factor = f;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           addFactorControl1.initCombox();
            if (factor == null)
                simpleButtonOK.Text = "添加";
            else
            {
                simpleButtonOK.Text = "修改";
                factor = FactorDB.GetFactorById(factor.factorId);
                addFactorControl1.showfactor(factor);
            }
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            String messagestr= (factor == null ? "添加" : "修改");
            if (XtraMessageBox.Show(String.Format("是否{0}控矿要素", messagestr), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                factor = addFactorControl1.addFactor();
                if (FactorDB.insertFactor(factor) > 0)
                {
                    XtraMessageBox.Show(String.Format("{0}控矿要素{1}", messagestr, "成功"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    this.Dispose();
                }
                else XtraMessageBox.Show(String.Format("{0}控矿要素{1}", messagestr, "失败"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}

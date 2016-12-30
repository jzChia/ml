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
    public partial class AddModelForm : Form
    {
        private ProspectingModel pm;
        public List<Factor> fs;
        public AddModelForm()
        {
            InitializeComponent();
        }
        public AddModelForm(ProspectingModel p)
        {
            this.pm = p;
            InitializeComponent();
            //addModelControl1.showFactors(fs);
        }

        private void addModelControl1_addFactorButtonClick(object sender, EventArgs e)
        {
            FactorTreeForm ftf = new FactorTreeForm(pm!=null && pm.factors!=null?pm.factors.ToList():new List<Factor>());
            ftf.Owner = this;
            ftf.ShowDialog();
            addModelControl1.showFactors(fs);
        }

        private void addModelForm_Load(object sender, EventArgs e)
        {
            if (pm == null)
                simpleButton1.Text = "添加";
            else
            {
                simpleButton1.Text = "修改";
                pm = ProspectingModelDB.GetProspectingModelById(pm.modelId);
                addModelControl1.showModel(pm);
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            String messagestr = (pm == null ? "添加" : "修改");
            if (XtraMessageBox.Show("是否" + messagestr + "找矿模型", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                pm = addModelControl1.addModel();
                if (fs != null)
                {
                    foreach (var item in fs)
                    {
                        if (item.prospectingModels == null)
                            item.prospectingModels = new List<ProspectingModel>();
                        item.prospectingModels.Add(pm);
                    }
                }
                if (ProspectingModelDB.insertProspectingModel(pm) > 0)
                {
                    XtraMessageBox.Show(messagestr+"找矿模型成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    this.Dispose();
                }
                else XtraMessageBox.Show(messagestr+"找矿模型失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}

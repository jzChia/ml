using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MLCommon.Entities;
using MLCommon.DAO;
using DevExpress.XtraEditors;

namespace userControls
{
    public partial class FactorTreeControl : UserControl
    {
        public event EventHandler addFactorTree;
        public event EventHandler updateFactorTree;
        List<Factor> f;
        public FactorTreeControl()
        {
            InitializeComponent();
        }

        private void FactorTreeControl_Load(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                marqueeProgressBarControl1.Properties.ShowTitle = true;
                marqueeProgressBarControl1.Text = "数据加载中...请稍后...";
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void bindTreeview(List<Factor> factors, TreeView tv)
        {
            tv.Nodes.Clear();
            if (factors == null)
                return;
            List<String> cls = FactorDB.getClassification(factors);
            List<String> ctr = null;
            List<Factor> fs = null;
            TreeNode tn = null, tn1 = null, tn2 = null;
            for (int i = 0; i < cls.Count; i++)
            {
                tn2 = new TreeNode();
                tn2.Text = cls[i];
                tn2.Tag = "classification";
                tn2.ImageIndex = 1;
                ctr = FactorDB.getCategory(factors, cls[i]);
                if (ctr != null)
                    for (int j = 0; j < ctr.Count; j++)
                    {
                        tn1 = new TreeNode();
                        tn1.Text = ctr[j];
                        tn1.Tag = "category";
                        tn1.ImageIndex = 2;
                        fs = FactorDB.getFactorNames(factors, cls[i], ctr[j]);
                        if (fs != null)
                        {
                            for (int m = 0; m < fs.Count; m++)
                            {
                                tn = new TreeNode();
                                tn.Text = fs[m].name;
                                tn.Name = fs[m].author;
                                tn.Tag = fs[m];
                                tn.ImageIndex = 3;
                                tn1.Nodes.Add(tn);
                            }
                        }
                        tn2.Nodes.Add(tn1);
                    }
                tv.Nodes.Add(tn2);
            }
            tv.ExpandAll();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            f = FactorDB.GetFactors();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bindTreeview(f,treeView1);
            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        public void buttonExpandorCollapse_Click(String lb)
        {
                if (lb.Equals("buttonExpand"))
                {
                    treeView1.ExpandAll();
                }
                else if (lb.Equals("buttonCollapse"))
                {
                    treeView1.CollapseAll();
                }
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                treeView1.ContextMenuStrip = contextMenuStrip1;
                删除ToolStripMenuItem.Enabled = false;

                if (treeView1.SelectedNode != null)
                {
                    删除ToolStripMenuItem.Enabled = treeView1.SelectedNode.Tag is Factor;

                }
            }
            else treeView1.ContextMenuStrip = null;
        }


        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                if ((sender as ToolStripMenuItem).Name.StartsWith("刷新"))
                {
                    if (!backgroundWorker1.IsBusy)
                    {
                        layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        marqueeProgressBarControl1.Properties.ShowTitle = true;
                        marqueeProgressBarControl1.Text = "数据加载中...请稍后...";
                        backgroundWorker1.RunWorkerAsync();
                    }
                }
                else if ((sender as ToolStripMenuItem).Name.StartsWith("添加"))
                {
                    if (addFactorTree != null)
                        addFactorTree(sender, e);
                }
                else if ((sender as ToolStripMenuItem).Name.StartsWith("修改"))
                {
                    if (updateFactorTree != null)
                        updateFactorTree(sender, e);
                }
                else if ((sender as ToolStripMenuItem).Name.Contains("ToolStripMenuItem"))
                {
                    menuClickMenthod((sender as ToolStripMenuItem).Name);
                }
            }
        }

        private void menuClickMenthod(String lb)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is Factor)
                {
                    if (XtraMessageBox.Show("是否删除" + (treeView1.SelectedNode.Tag as Factor).name, " 提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        FactorDB.removeModel((treeView1.SelectedNode.Tag as Factor).factorId);
                        if (FactorDB.DeleteFactor((treeView1.SelectedNode.Tag as Factor))>0)
                        {
                            if (!backgroundWorker1.IsBusy)
                            {
                                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                marqueeProgressBarControl1.Properties.ShowTitle = true;
                                marqueeProgressBarControl1.Text = "数据加载中...请稍后...";
                                backgroundWorker1.RunWorkerAsync();
                            }
                            bindTreeview(f.FindAll(m => m.name.Contains(textEdit1.Text.Trim()) || m.classification.Contains(textEdit1.Text.Trim()) || m.category.Contains(textEdit1.Text.Trim())), treeView1);

                            XtraMessageBox.Show("删除成功", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                            XtraMessageBox.Show("删除成功", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                }
                else if (treeView1.SelectedNode.Tag is String)
                {
                    String str = (String)treeView1.SelectedNode.Tag;
                    if (str.Equals("classification"))
                    {
                        XtraMessageBox.Show(str, " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        if (str.Equals("category"))
                        {
                            XtraMessageBox.Show(str, " 提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                }
            }
            else
            {
                XtraMessageBox.Show("未选中任何选项！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            List<Factor> p = f.FindAll(m => m.name.Contains(textEdit1.Text.Trim()) || m.classification.Contains(textEdit1.Text.Trim()) || m.category.Contains(textEdit1.Text.Trim()));
            bindTreeview(p, treeView1);

        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bindTreeview(f.FindAll(m => m.name.Contains(textEdit1.Text.Trim()) || m.classification.Contains(textEdit1.Text.Trim()) || m.category.Contains(textEdit1.Text.Trim())), treeView1);
            }
        }
    }
}

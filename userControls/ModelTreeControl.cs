using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using MLCommon.Entities;
using MLCommon.DAO;
using System.Threading;
using DevExpress.XtraEditors;

namespace userControls
{
    public partial class ModelTreeControl : UserControl
    {
        public event EventHandler addModelTree;
        public event EventHandler updateModelTree;
        List<ProspectingModel> pms;
        public ProspectingModel selectedPM;
        public ModelTreeControl()
        {
            InitializeComponent();
        }

        private void ModelTreeControl_Load(object sender, EventArgs e)
        {
           // treeView1.ContextMenuStrip = contextMenuStrip1;

            if (!backgroundWorker1.IsBusy)
            {
                layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                marqueeProgressBarControl1.Properties.ShowTitle = true;
                marqueeProgressBarControl1.Text = "数据加载中...请稍后...";
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag is ProspectingModel)
            {
                if (treeView1.SelectedNode.Nodes .Count== 0)
                {
                    ProspectingModel p = (treeView1.SelectedNode.Tag as ProspectingModel);
                    ProspectingModel pm = ProspectingModelDB.GetProspectingModelById(p.modelId);

                    if (pm.factors != null)
                    {
                        List<Factor> fs = pm.factors.ToList();
                        bindTreeview(fs, treeView1.SelectedNode.Nodes);
                    }
                }

            }
        }

        private static void bindTreeview(List<Factor> f, TreeNodeCollection tv)
        {
            tv.Clear();
            if (f == null)
                return;
            List<String> cls = FactorDB.getClassification(f);
            List<String> ctr = null;
            List<Factor> fs = null;
            TreeNode tn = null, tn1 = null, tn2 = null;
            for (int i = 0; i < cls.Count; i++)
            {
                tn2 = new TreeNode();
                tn2.Text = cls[i];
                tn2.Tag = "classification";
                tn2.ImageIndex = 1;
                ctr = FactorDB.getCategory(f, cls[i]);
                if (ctr != null)
                    for (int j = 0; j < ctr.Count; j++)
                    {
                        tn1 = new TreeNode();
                        tn1.Text = ctr[j];
                        tn1.Tag = "category";
                        tn1.ImageIndex = 2;
                        fs = FactorDB.getFactorNames(f, cls[i], ctr[j]);
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
                tv.Add(tn2);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            pms = ProspectingModelDB.GetProspectingModels();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            initTreeview(pms);
            layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        private void initTreeview(List<ProspectingModel> p)
        {
            treeView1.Nodes.Clear();
            if (p == null)
                return;
            TreeNode tn = null;
            foreach (var item in p)
            {
                tn = new TreeNode();
                tn.Text = item.name;
                tn.ImageIndex = 0;
                tn.Tag = item;
                treeView1.Nodes.Add(tn);
            }
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            List<ProspectingModel> p = pms.FindAll(m => m.name.Contains(textEdit1.Text.Trim()));
            initTreeview(p);
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                treeView1.ContextMenuStrip = contextMenuStrip1;

                移除ToolStripMenuItem.Enabled = false;
                删除ToolStripMenuItem.Enabled = false;
                
                if (treeView1.SelectedNode != null)
                {
                    删除ToolStripMenuItem.Enabled = treeView1.SelectedNode.Tag is ProspectingModel;
                    移除ToolStripMenuItem.Enabled = !(treeView1.SelectedNode.Tag is ProspectingModel);

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
                        layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        marqueeProgressBarControl1.Properties.ShowTitle = true;
                        marqueeProgressBarControl1.Text = "数据加载中...请稍后...";
                        backgroundWorker1.RunWorkerAsync();
                    }
                }
               else if ((sender as ToolStripMenuItem).Name.StartsWith("添加"))
               {
                   if (addModelTree != null)
                       addModelTree(sender, e);
               }
               else if ((sender as ToolStripMenuItem).Name.StartsWith("修改"))
               {
                   if (updateModelTree != null)
                       updateModelTree(sender, e);
               }
               else if ((sender as ToolStripMenuItem).Name.Contains("ToolStripMenuItem"))
               {
                   menuClickMenthod((sender as ToolStripMenuItem).Name);
               }
            }
        }

        private void menuClickMenthod(String lb)
        {
            if (String.IsNullOrEmpty(lb))
                return;

            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is ProspectingModel)
                {
                    if(lb.StartsWith( "删除"))
                    {
                        if (XtraMessageBox.Show("是否删除" + (treeView1.SelectedNode.Tag as ProspectingModel).name, " 提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            ProspectingModelDB.removeFacor((treeView1.SelectedNode.Tag as ProspectingModel).modelId);
                            if (ProspectingModelDB.DeleteProspectingModel(treeView1.SelectedNode.Tag as ProspectingModel) > 0)
                            {
                                if (!backgroundWorker1.IsBusy)
                                {
                                    layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                    marqueeProgressBarControl1.Properties.ShowTitle = true;
                                    marqueeProgressBarControl1.Text = "数据加载中...请稍后...";
                                    backgroundWorker1.RunWorkerAsync();
                                }
                                XtraMessageBox.Show("删除成功", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                
                            }
                            else
                                XtraMessageBox.Show("删除成功", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else if (treeView1.SelectedNode.Tag is Factor)
                {
                    Factor f = treeView1.SelectedNode.Tag as Factor;
                    if ("移除".Equals(lb))
                    {
                        if (XtraMessageBox.Show("移除"+f.name+"？", " 提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        { 
                            if(treeView1.SelectedNode.Parent.Parent.Parent.Tag is ProspectingModel)
                            {
                                if(ProspectingModelDB.removeFacor((treeView1.SelectedNode.Parent.Parent.Parent.Tag as ProspectingModel).modelId, f )>0)
                                {
                                    XtraMessageBox.Show("移除"+f.name+"成功", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }else
                                    XtraMessageBox.Show("移除"+f.name+"失败", " 提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            }
                        }
                    }
                        
                }
                else if (treeView1.SelectedNode.Tag is String)
                {
                    String str = (String)treeView1.SelectedNode.Tag;
                    if (str.Equals("classification"))
                    {
                        if (XtraMessageBox.Show("移除" + str, " 提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            if (treeView1.SelectedNode.Parent.Tag is ProspectingModel)
                            {

                                if (ProspectingModelDB.removeFacor((treeView1.SelectedNode.Parent.Parent.Parent.Tag as ProspectingModel).modelId, str,null) > 0)
                                {
                                    XtraMessageBox.Show("移除" + str + "成功", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                    XtraMessageBox.Show("移除" + str + "失败", " 提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                        if (str.Equals("category"))
                        {
                            if (XtraMessageBox.Show("移除" + str, " 提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                if (treeView1.SelectedNode.Parent.Parent.Tag is ProspectingModel)
                                {

                                    if (ProspectingModelDB.removeFacor((treeView1.SelectedNode.Parent.Parent.Parent.Tag as ProspectingModel).modelId, null, str) > 0)
                                    {
                                        XtraMessageBox.Show("移除" + str + "成功", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                        XtraMessageBox.Show("移除" + str + "失败", " 提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                }
                            }
                        }
                }
            }
            else
            {
                XtraMessageBox.Show("未选中任何选项！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                initTreeview(pms.FindAll(m => m.name.Contains(textEdit1.Text.Trim())));
            }

        }

    }
}

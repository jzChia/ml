using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using MLCommon.Entities;
using MLCommon.DAO;
using DevExpress.Utils;

namespace userControls
{
    public partial class SelectFactorControl2 : UserControl
    {
        private WaitDialogForm waitDialogForm = null; 
        private List<Factor> Initfactors = new List<Factor>();
        private  HashSet<Factor> _resultFactors = new HashSet<Factor>();
        public event EventHandler savebtnClick;
       
        public SelectFactorControl2()
        {
            InitializeComponent();
        }

        private void SelectFactorControl2_Load(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
           waitDialogForm = new WaitDialogForm("", "正在加载数据.....");
            
                toolTip1.SetToolTip(buttonCollapse, "折叠所有节点");
                toolTip1.SetToolTip(buttonExpand, "展开所有节点");
                toolTip1.SetToolTip(button1Collapse, "折叠所有节点");
                toolTip1.SetToolTip(button1Expand, "展开所有节点");
            
        }

        private  void bindTreeview(List<Factor> f,TreeView tv)
        {
            tv.Nodes.Clear();
            if (f == null)
                return;
            List<String> cls = FactorDB.getClassification(f);
            List<String> ctr = null;
            List<Factor> fs = null;
            TreeNode tn = null, tn1 = null,tn2=null;
            for (int i = 0; i < cls.Count; i++)
            {
                tn2 = new TreeNode();
                tn2.Text = cls[i];
                tn2.ImageIndex =0;
                ctr = FactorDB.getCategory(f,cls[i]);
                if(ctr!=null)
                for(int j=0;j<ctr.Count;j++)
                {
                    tn1 = new TreeNode();
                    tn1.Text = ctr[j];
                    tn1.ImageIndex = 1;
                    fs = FactorDB.getFactorNames(f,cls[i],ctr[j]);
                    if (fs != null)
                    {
                        for(int m=0;m<fs.Count;m++)
                        {
                            tn = new TreeNode();
                            tn.Text = fs[m].name;
                            tn.Name = fs[m].author;
                            tn.Tag = fs[m];
                            tn.ImageIndex = 2;
                            tn1.Nodes.Add(tn);
                        }
                    }
                    tn2.Nodes.Add(tn1);
                }
                tv.Nodes.Add(tn2);
            }
            tv.ExpandAll();
        }

        public void initTreeView2(List<Factor> f)
        {
            if (_resultFactors != null)
                _resultFactors = new HashSet<Factor>();
            foreach (var ff in f)
            {
                _resultFactors.Add(ff);
            }
            bindTreeview(_resultFactors.ToList(), treeView2);
        }

        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            addTreeview2Node();
        }

        private void simpleButtonRemove_Click(object sender, EventArgs e)
        {
            removeTreeview2Node();
        }

        private void removeTreeview2Node()
        {
            if (_resultFactors == null)
                _resultFactors = new HashSet<Factor>();

            if (treeView2.SelectedNode != null && treeView2.SelectedNode.Tag != null && treeView2.SelectedNode.Tag is Factor)
            {
                if (  _resultFactors.Remove((Factor)treeView2.SelectedNode.Tag))
                {
                    bindTreeview(_resultFactors.ToList(), treeView2);
                }
            }
        }

        private void simpleButtonRemoveAll_Click(object sender, EventArgs e)
        {
            treeView2.Nodes.Clear();
            _resultFactors = new HashSet<Factor>();
        }

        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            if (savebtnClick != null)
                savebtnClick(sender, e);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            bindTreeview(Initfactors.
                Where(f =>
                    f.name.IndexOf(textEdit1.Text,StringComparison.OrdinalIgnoreCase)>=0 ||
                    f.classification.IndexOf(textEdit1.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    f.category.IndexOf(textEdit1.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList(), treeView1);
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (sender is TreeView)
            {
                
                    if ((sender as TreeView).Name.Equals("treeView1"))
                    {
                        addTreeview2Node();
                    }
                    else
                        removeTreeview2Node();
                

            }

        }

        private void addTreeview2Node()
        {
            if (_resultFactors == null)
                _resultFactors = new HashSet<Factor>();

            if (treeView1.SelectedNode!=null && treeView1.SelectedNode.Tag != null
                && treeView1.SelectedNode.Tag is Factor && 
                _resultFactors.Add((Factor)treeView1.SelectedNode.Tag))
            {
                bindTreeview(_resultFactors.ToList(), treeView2);
            }
        }

        private void buttonExpandorCollapse_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                if((sender as Button).Name.Equals("buttonExpand"))
                {
                    treeView1.ExpandAll();
                }
                else if ((sender as Button).Name.Equals("buttonCollapse"))
                {
                    treeView1.CollapseAll();
                }
                else if ((sender as Button).Name.Equals("button1Expand"))
                {
                    treeView2.ExpandAll();
                }
                else if ((sender as Button).Name.Equals("button1Collapse"))
                {
                    treeView2.CollapseAll();
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Initfactors = FactorDB.GetFactors();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bindTreeview(Initfactors, treeView1);
            if (waitDialogForm != null)
            {
                waitDialogForm.Close();
                waitDialogForm.Dispose();
                waitDialogForm = null;
            }
        }

        public List<Factor> getFactors()
        {
            if (_resultFactors != null)
                return  _resultFactors.ToList();
            else return new List<Factor>();
        }

        public void initLabelAndSave(String str1,String str2,bool isShow)
        {
            labelControl1.Text = str1;
            labelControl2.Text = str2;
            simpleButtonSave.Visible = isShow;
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bindTreeview(Initfactors.
                    Where(f =>
                        f.name.IndexOf(textEdit1.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        f.classification.IndexOf(textEdit1.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        f.category.IndexOf(textEdit1.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList(), treeView1);
            }
        }
    }
}

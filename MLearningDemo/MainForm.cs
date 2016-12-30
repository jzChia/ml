using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using System.Collections;
using MLCommon.DAO;
using MLCommon.Entities;
using MLCommon;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;

namespace MLearningDemo
{
    public partial class MainForm : RibbonForm
    {
        private DataSet  ds=null;
        private Timer mlcalTimer = new Timer();
        private DateTime beginTime;
        private List<Factor> initFactor = new List<Factor>();
        private List<String> initkeywords= new List<string>();
        private Dictionary<ProspectingModel, HashSet<String>> pmdic = null;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            barStaticItem1.Caption = String.Format("关键字：{0}；控矿因子：{1}", initkeywords != null ? initkeywords.Count : 0, initFactor != null ? initFactor.Count : 0);
            dockPanel_container.Dock = DockStyle.Fill;
            mlcalTimer.Interval = 100;
            mlcalTimer.Tick += new EventHandler(mlcalTimer_Tick);
    
        }

        void mlcalTimer_Tick(object sender, EventArgs e)
        {
            label1.Text ="运行时间："+ ((double)(DateTime.Now - beginTime  ).TotalMilliseconds / 1000.0).ToString() +"秒";
        }
        

        private void buttonExpandOrCollapse_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                if (xtraTabControl2.SelectedTabPage == xtraTabPageModeltree)
                    modelTreeControl1.buttonExpandorCollapse_Click((sender as ToolStripMenuItem).Name);
                else if (xtraTabControl2.SelectedTabPage == xtraTabPageFactorTree)
                    factorTreeControl1.buttonExpandorCollapse_Click((sender as ToolStripMenuItem).Name);
            }
        }

        private void addFactorbarButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddFactorForm ff = new AddFactorForm();
            ff.ShowDialog();
        }

        private void modelTreeControl1_addModelTree(object sender, EventArgs e)
        {
            addModelbarButtonItem2.PerformClick();
        }

        private void addModelbarButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddModelForm amf = new AddModelForm();
            amf.ShowDialog();
        }

        private void barButtonItemCalc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!backgroundWorkerMLCalc.IsBusy)
            {
                if (initkeywords == null || initkeywords.Count < 1)
                {
                    XtraMessageBox.Show("关键词为空，请添加关键词！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                barButtonItemCalc.Enabled = false;
                backgroundWorkerMLCalc.RunWorkerAsync();
                barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                beginTime = DateTime.Now;
                mlcalTimer.Start();
                richTextBox1.Text += "开始时间：" + beginTime.ToLocalTime().ToString() + "\r\n";
            }
            else
            {
                XtraMessageBox.Show("正在计算中，请稍后！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void backgroundWorkerMLCalc_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            bw.ReportProgress(1);
            pmdic = ProspectingModelDB.GetProspectingModelByName(initkeywords.ToArray());
            bw.ReportProgress(2);
            using (DataTable mdoeldt = MLCalc.calcModelDisplays(pmdic))
            {
                bw.ReportProgress(3);
                using (DataTable factordt = MLCalc.calcFactorDisplays(pmdic))
                {
                    bw.ReportProgress(4);
                    ds = new DataSet();
                    ds.Tables.AddRange(new DataTable[] { mdoeldt.Copy(), factordt.Copy() });
                    DataRelation relation = new DataRelation("FactorTable", ds.Tables[0].Columns[0], ds.Tables[1].Columns[0], false);
                    ds.Relations.Add(relation);
                    bw.ReportProgress(5);
                }
            }
        }

        private void backgroundWorkerMLCalc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            barEditItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            if (this.MdiChildren != null)
            {
                foreach (var item in this.MdiChildren)
                {
                    if (item is CalcModelForm)
                    {
                        XtraMessageBox.Show("计算模型窗体已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (ds != null)
                        {
                            (item as CalcModelForm).showData(ds);
                            richTextBox1.Text += "计算结果显示……\r\n";
                            richTextBox1.Text += "结束时间：" + DateTime.Now.ToLocalTime().ToString() + "\r\n"; ;
                            mlcalTimer.Stop();
                        }
                        item.Activate();
                        barButtonItemCalc.Enabled = true;
                        return;
                    }
                }
            }
            CalcModelForm cmf = new CalcModelForm();
            cmf.MdiParent = this;
            cmf.Show();
            if (ds != null)
            {
                cmf.showData(ds);
                richTextBox1.Text += "计算结果显示……\r\n";
                richTextBox1.Text += "结束时间：" + DateTime.Now.ToLocalTime().ToString() + "\r\n"; ;
                mlcalTimer.Stop();
            }
            barButtonItemCalc.Enabled = true;
        }

        private void backgroundWorkerMLCalc_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch(e.ProgressPercentage )
            {
                case 1:
                    richTextBox1.Text += "计算开始……\r\n依据关键字搜索找矿模型\r\n";
                    break;
                case 2:
                    richTextBox1.Text += "模型搜索结束\r\n模型统计创建\r\n";
                    break;
                case 3:
                    richTextBox1.Text += "模型统计结束\r\n找矿要素统计计算\r\n";
                    break;
                case 4:
                    richTextBox1.Text += "找矿要素统计计算结束\r\n模型要素关联\r\n";
                    break;
                case 5:
                    richTextBox1.Text += "计算结束……\r\n";
                    break;
                default :
                    break;

            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (pmdic == null || pmdic.Count < 1)
            {
                XtraMessageBox.Show("请先计算模型！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (initFactor == null || initFactor.Count < 1)
            {
                XtraMessageBox.Show("研究区控矿要素为空，请选择研究区控矿要素！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

            if (this.MdiChildren != null)
            {
                foreach (var item in this.MdiChildren)
                {
                    if (item is CalcFactorForm)
                    {
                        XtraMessageBox.Show("计算控矿要素窗体已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        item.Activate();
                        return;
                    }
                }
            }
            CalcFactorForm cff = new CalcFactorForm(pmdic,initFactor);
            cff.MdiParent = this;
            cff.Show();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InitParaForm ipf = new InitParaForm( initFactor,initkeywords);
            ipf.ShowDialog();
        }

        private void keywordbarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.MdiChildren != null)
            {
                foreach (var item in this.MdiChildren)
                {
                    if (item is KeywodsForm)
                    {
                        XtraMessageBox.Show("关键词窗体已存在！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        item.Activate();
                        return;
                    }
                }
            }
            KeywodsForm kwf = new KeywodsForm();
            kwf.MdiParent = this;
            kwf.keywodsOnReportProgress = new KeywodsForm.KeywodsDoReportProgress(keywodsOnReportProgress);
            kwf.Show();
        }

        public void keywodsOnReportProgress(List<String> strs)
        {
            initkeywords = strs;
            barStaticItem1.Caption = String.Format("关键字：{0}；控矿因子：{1}", initkeywords != null ? initkeywords.Count : 0, initFactor != null ? initFactor.Count : 0);
        }

        private void factorbarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.MdiChildren != null)
            {
                foreach (var item in this.MdiChildren)
                {
                    if (item is SelectFactorForm)
                    {
                        XtraMessageBox.Show("选择控矿要素窗体已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        item.Activate();
                        return;
                    }
                }
            }
            SelectFactorForm sff = new SelectFactorForm();
            sff.MdiParent = this;
            sff.selectFactorDoReportProgress = new SelectFactorForm.SelectFactorDoReportProgress(SelectFactorDoReportProgress);         
            sff.Show();
        }

        private void SelectFactorDoReportProgress(List<Factor> fs)
        {
            initFactor = fs;
            barStaticItem1.Caption = String.Format("关键字：{0}；控矿因子：{1}", initkeywords != null ? initkeywords.Count : 0, initFactor != null ? initFactor.Count : 0);   
        }

        private void modelTreeControl1_updateModelTree(object sender, EventArgs e)
        {
            if (modelTreeControl1.treeView1.SelectedNode.Tag is ProspectingModel)
            {
                ProspectingModel pm = modelTreeControl1.treeView1.SelectedNode.Tag as ProspectingModel;

                AddModelForm adf = new AddModelForm(pm);
                adf.ShowDialog();
            }
        }

        private void barButtonItemSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.MdiChildren != null)
            {
                foreach (var item in this.MdiChildren)
                {
                    if (item is FactorForm)
                    {
                        XtraMessageBox.Show("控矿要素窗体已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        item.Activate();
                        return;
                    }
                }
            }
            FactorForm ff = new FactorForm();
            ff.MdiParent = this;
            ff.Show();
        }

        private void barButtonItemTitle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item is BarButtonItem)
            {
                if (e.Item.Name.EndsWith("Cascade"))
                {
                    this.LayoutMdi(MdiLayout.Cascade);
                }
                else  if (e.Item.Name.EndsWith("Horizontal"))
                {
                    this.LayoutMdi(MdiLayout.TileHorizontal);
                }
                else  if (e.Item.Name.EndsWith("Vertical"))
                {
                    this.LayoutMdi(MdiLayout.TileVertical);
                }
            }
        }

        private void barCheckItemTabbedMDI_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xtraTabbedMdiManager1.MdiParent = barCheckItemTabbedMDI.Down ? this : null;
            barButtonItemCascade.Enabled = barButtonItemHorizontal.Enabled = barButtonItemVertical.Enabled = !barCheckItemTabbedMDI.Down;
        }

        private void barButtonItemDock_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.Item is BarButtonItem)
            {
                if (e.Item.Name.EndsWith("Catalog"))
                {
                    dockPanelTOC.Show();
                }
                else if (e.Item.Name.EndsWith("Result"))
                {
                    dockPanelRes.Show();
                }
            }
        }

    }
}

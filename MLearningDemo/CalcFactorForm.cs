using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MLCommon.Entities;
using System.Linq;
using MLCommon.DAO;
using MLCommon;
using DevExpress.Utils;
using userControls;

namespace MLearningDemo
{
    public partial class CalcFactorForm : DevExpress.XtraEditors.XtraForm
    {
        private WaitDialogForm waitDialogForm = null; 
        Dictionary<ProspectingModel, HashSet<String>> pmdic;
        List<Factor> resConfirmFactors = new List<Factor>();
        List<Factor> resNoConfirmFactors = new List<Factor>();
        List<Factor> mresNoConfirmFactors = new List<Factor>();
        List<Factor> initFactor = new List<Factor>();
        List<ProspectingModel> pppms;
        public CalcFactorForm(List<Factor> resConfirmFactors, List<Factor> resNoConfirmFactors, List<Factor> mresNoConfirmFactors)
        {
            InitializeComponent();
            this.resConfirmFactors = resConfirmFactors;
            this.resNoConfirmFactors = resNoConfirmFactors;
            this.mresNoConfirmFactors = mresNoConfirmFactors;
        }

        public CalcFactorForm(Dictionary<ProspectingModel, HashSet<String>> pmdic, List<Factor> initFactor)
        {
            InitializeComponent();
            this.pmdic = pmdic;
            this.initFactor = initFactor;
            calc(false);
        }

        private void calc(bool isChecked)
        {
            waitDialogForm = new WaitDialogForm("", "正在加载数据.....");
            List<ProspectingModel> pms = pmdic.Keys.ToList();
            List<Factor> dset;
            if (pppms == null)
                pppms = new List<ProspectingModel>();
            else pppms.Clear();
            dset = isChecked?
                FactorDB.GetFactorByProspectingModels(pms) :
                FactorDB.GetFactorByProspectingModelsAndFactors(pms, initFactor,out pppms);
            calcMethod(dset);
            LoadData();
            if (waitDialogForm != null)
            {
                waitDialogForm.Close();
                waitDialogForm.Dispose();
            }
        }

        private void calcMethod(List<Factor> dset)
        {
            resConfirmFactors = new List<Factor>();
            resNoConfirmFactors = new List<Factor>();
            mresNoConfirmFactors = new List<Factor>();
            List<Factor> modelfactors = dset != null ? dset: new List<Factor>();
            var mclassList = modelfactors.GroupBy(f => new { f.classification, f.category }).Select(k => k.Key).ToList();
            var classList = initFactor.GroupBy(f => new { f.classification, f.category }).Select(k => k.Key).ToList();

            initFactor.ForEach(i => resNoConfirmFactors.Add(i));
            modelfactors.ForEach(i => mresNoConfirmFactors.Add(i));
            foreach (var f in initFactor)
            {
                if (modelfactors.Find(fa => fa.factorId.Equals(f.factorId)) != null)
                {
                    resConfirmFactors.Add(f);
                }
            }

            var resclassList = resConfirmFactors.GroupBy(f => new { f.classification, f.category }).Select(k => k.Key).ToList();
            foreach (var item in resclassList)
            {
                resNoConfirmFactors = resNoConfirmFactors.Where(t => !t.category.Equals(item.category) || !t.classification.Equals(item.classification)).ToList();
                mresNoConfirmFactors = mresNoConfirmFactors.Where(t => !t.category.Equals(item.category) || !t.classification.Equals(item.classification)).ToList();
            }
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            listBox1.DataSource = modelName();
        }

        private void LoadData()
        {
            gridControl1.DataSource = null;
            gridControl1.DataSource = resConfirmFactors;

            var resclassList = resConfirmFactors.GroupBy(f => new { f.classification, f.category }).Select(k => k.Key).ToList();

            List<checkFactor> fs = new List<checkFactor>();

            foreach (var item in resNoConfirmFactors)
            {
                item.significance = 99999999;
                item.frequency = 99999999;
                fs.Add(new checkFactor(item));
            }
            foreach (var item in mresNoConfirmFactors)
            {
                fs.Add(new checkFactor(item));
            }

            List<checkFactor> fsr = new List<checkFactor>();
            var t = fs.OrderBy(f => f.classification).ThenBy(f => f.category).ThenByDescending(f => f.frequency).ThenByDescending(f => f.significance).ThenBy(f => f.name);
            foreach (var item in t)
            {
                fsr.Add(item);
            }
            gridControl2.DataSource = null;
            gridControl2.DataSource = fsr;
        }

        private void gridView2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

            object obj = gridView2.GetRow(e.RowHandle);
            if (obj is checkFactor)
            {
                if ("isChecked".Equals(e.Column.Name))
                {
                    checkFactor cf = obj as checkFactor;
                    if (Convert.ToBoolean(e.Value))
                    {
                        if (!resConfirmFactors.Contains(cf))
                            resConfirmFactors.Add(cf);
                    }
                    else
                    {
                        if (resConfirmFactors.Contains(cf))
                            resConfirmFactors.Remove(cf);
                    }
                }

            }
            gridControl1.DataSource = resConfirmFactors;
            gridControl1.RefreshDataSource();
        }

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {

            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString().Trim();
            }
        }

        private void gridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.Name.Equals("gridColumn8") || e.Column.Name.Equals("gridColumn9"))
             {
                 if (Convert.ToInt32(e.CellValue) == 99999999)
                     e.DisplayText = "研究区资料";
             }
        }

        private void checkEdit1_CheckStateChanged(object sender, EventArgs e)
        {
            calc(checkEdit1.Checked);
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkEdit1_Properties_CheckStateChanged(object sender, EventArgs e)
        {
            
        }

        public List<String>  modelName()
        {
            String tips;
            Dictionary<String, int> res = new Dictionary<string, int>();
            int val;
            foreach (var item in pppms)
            {
                List<String> list = createKeywords.DisplaySegment(item.name, out tips);
                foreach (var li in list)
                {
                    if (res.TryGetValue(li, out val))
                    {
                        res.Remove(li);
                        res.Add(li, val + 1);
                    }
                    else res.Add(li, 1);
                }
            }
            var t =res.OrderByDescending(r => r.Value);
            List<String> resstr = new List<string>();
            foreach (var item in t)
            {
                resstr.Add(item.Key + "(" + item.Value + ")");
            }
            return resstr;
        }
    }


    class checkFactor : Factor
    {
        public bool isChecked { get; set; }
        
        public checkFactor(Factor f)
        {
            this.author = f.author;
            this.category = f.category;
            this.classification = f.classification;
            this.createTime = f.createTime;
            this.detail = f.detail;
            this.factorId = f.factorId;
            this.frequency = f.frequency;
            this.name = f.name;
            this.prospectingModels = f.prospectingModels;
            this.referenceMatch = f.referenceMatch;
            this.significance = f.significance;
            this.updateTime = f.updateTime;
            this.isChecked = false;
        }

    }
}
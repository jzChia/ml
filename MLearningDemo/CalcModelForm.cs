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
    public partial class CalcModelForm : Form
    {
        public CalcModelForm()
        {
            InitializeComponent();
        }

        public void showData(DataSet ds)
        {
            gridControl1.DataSource = ds.Tables[0];
            gridView1.BestFitColumns();
            FactorTable.ExpandAllGroups();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString().Trim();
            }
        }
    }
}

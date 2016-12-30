using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MLCommon.Entities;

namespace MLearningDemo
{
    public partial class FactorForm : Form
    {
        public FactorForm()
        {
            InitializeComponent();
        }

        private void factorControl1_factorRowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            object obj = factorControl1.gridViewFactor.GetRow(e.RowHandle);
            if (obj is Factor)
            {
                Factor f = obj as Factor;
                if ("updatef".Equals(e.Column.Name))
                {
                    AddFactorForm ff = new AddFactorForm(f);
                    ff.ShowDialog();
                }
                else if ("deletef".Equals(e.Column.Name))
                {

                }
            }
        }

        private void factorControl1_addFactorClick(object sender, EventArgs e)
        {
            AddFactorForm ff = new AddFactorForm();
            ff.ShowDialog();
        }
    }
}

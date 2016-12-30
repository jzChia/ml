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

namespace userControls
{
    public partial class AddModelControl : UserControl
    {
        private ProspectingModel pm;
        public event EventHandler addFactorButtonClick;
        public AddModelControl()
        {
            InitializeComponent();
        }
        private void AddModelControl_Load(object sender, EventArgs e)
        {
            button1.Click += button1_Click;
        }

        void button1_Click(object sender, EventArgs e)
        {
            if (addFactorButtonClick != null)
                addFactorButtonClick(sender, e);
        }

        public void showModel(ProspectingModel p)
        {
            textBoxName.Text = p.name;
            textBoxAuthor.Text = p.author;
            richTextBoxDetail.Text = p.detail;
            gridControlFactor.DataSource = p.factors;
            pm = p;
        }

        public ProspectingModel addModel()
        {
            if (pm == null)
            {
                pm = new ProspectingModel();
                pm.createTime = DateTime.Now;
                pm.factors = new List<Factor>();
            }

            pm.name = textBoxName.Text;
            pm.author = textBoxAuthor.Text;
            pm.detail = richTextBoxDetail.Text;
            pm.updateTime = DateTime.Now;

            return pm;
        }

        public void showFactors(List<Factor> fs)
        {
            gridControlFactor.DataSource = fs;
            if (pm == null)
            {
                pm = new ProspectingModel();
                pm.createTime = DateTime.Now;
                pm.factors = new List<Factor>();
            }
            if (pm.factors == null)
                pm.factors = new List<Factor>();
            fs.AddRange(pm.factors);
            pm.factors = fs;
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            int index = e.RowHandle;
            object obj = gridView1.GetRow(index);
            if (obj is Factor)
            {
                Factor f = obj as Factor;
                if ("remove".Equals(e.Column.Name))
                {
                    pm.factors.Remove(f);
                    ProspectingModelDB.removeFacor(pm.modelId, f);
                    gridView1.DeleteRow(e.RowHandle);
                    gridView1.RefreshData();

                }

            }
            
        }
    }
}

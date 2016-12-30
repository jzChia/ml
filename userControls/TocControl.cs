using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace userControls
{
    public partial class TocControl : UserControl
    {
        public TocControl()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {

            if (sender is ToolStripMenuItem)
            {
                if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
                    modelTreeControl1.buttonExpandorCollapse_Click((sender as ToolStripMenuItem).Name);
                else if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
                    factorTreeControl1.buttonExpandorCollapse_Click((sender as ToolStripMenuItem).Name);
            }
        }
    }
}

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
    public partial class SelectFactorForm : Form
    {
        public delegate void SelectFactorDoReportProgress(List<Factor> initFactor);
        public SelectFactorDoReportProgress selectFactorDoReportProgress;
        public SelectFactorForm()
        {
            InitializeComponent();
        }

        private void selectFactorControl21_savebtnClick(object sender, EventArgs e)
        {
            if (selectFactorDoReportProgress != null)
                selectFactorDoReportProgress(selectFactorControl21.getFactors());

        }
    }
}

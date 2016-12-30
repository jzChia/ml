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
    public partial class AddFactorControl : UserControl
    {
        private Factor factor;
        public AddFactorControl()
        {
            InitializeComponent();

        }

        public void initCombox()
        {
            comboBoxClassification.DataSource = FactorDB.getAllClassification();
        }

        public void showfactor(Factor f)
        {
            comboBoxClassification.SelectedItem = f.classification;
            comboBoxCategory.SelectedItem = f.category;
            textBoxName.Text = f.name;
            textBoxAuthor.Text = f.author;
            richTextBoxDetail.Text = f.detail;
            factor = f;
        }

        public Factor addFactor()
        {
            if (factor == null)
            {
                factor = new Factor();
                factor.createTime=DateTime.Now;
            }             
                factor.classification = comboBoxClassification.Text;
                factor.category=comboBoxCategory.Text;
                factor.name=textBoxName.Text;
                factor.author=textBoxAuthor.Text;
                factor.detail=richTextBoxDetail.Text;
                factor.updateTime=DateTime.Now;
                return factor;
        }

        private void comboBoxClassification_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(comboBoxClassification.Text))
                comboBoxCategory.DataSource = FactorDB.getAllCategoryByClassification(comboBoxClassification.Text);
        }

    }
}

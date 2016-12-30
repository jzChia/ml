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
    public partial class SelectFactorControl1 : UserControl
    {
        private  List<Factor> Initfactors = new List<Factor>();
        private HashSet<Factor> _resultFactors = new HashSet<Factor>();
        public HashSet<Factor> resultFactors = null;
        public SelectFactorControl1()
        {
            InitializeComponent();
        }

        private void SelectFactorControl1_Load(object sender, EventArgs e)
        {
            Initfactors = FactorDB.GetFactors().ToList();
            listBoxControl1.DataSource = Initfactors;
            listBoxControl1.DisplayMember = "displayName";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            listBoxControl1.DataSource = Initfactors.Where(f => f.name.Contains(textEdit1.Text)).ToList();
        }

        private void simpleButtonadd_Click(object sender, EventArgs e)
        {
            if (_resultFactors == null)
                _resultFactors = new HashSet<Factor>();

            if (_resultFactors.Add((Factor)listBoxControl1.SelectedItem))
            {
                listBoxControl2.Items.Add(((Factor)listBoxControl1.SelectedItem).displayName); ;
            }
        }

        private void simpleButtonremove_Click(object sender, EventArgs e)
        {
            if (_resultFactors == null)
                _resultFactors = new HashSet<Factor>();

            if (_resultFactors.ToList().Remove((Factor)Initfactors.Find(f => f.displayName.Equals(listBoxControl2.SelectedItem))))
            {
                listBoxControl2.Items.Remove(listBoxControl2.SelectedItem);
            }
        }

        private void simpleButtonremoveAll_Click(object sender, EventArgs e)
        {
            listBoxControl2.Items.Clear();
            _resultFactors = new HashSet<Factor>();
        }

        private void simpleButtonsave_Click(object sender, EventArgs e)
        {
            resultFactors = _resultFactors;
        }

        private void listBoxControl1_DoubleClick(object sender, EventArgs e)
        {
            if (_resultFactors == null)
                _resultFactors = new HashSet<Factor>();

            if (_resultFactors.Add((Factor)listBoxControl1.SelectedItem))
            {
                listBoxControl2.Items.Add(((Factor)listBoxControl1.SelectedItem).displayName); ;
            }
        }

        private void listBoxControl2_DoubleClick(object sender, EventArgs e)
        {
            if (_resultFactors == null)
                _resultFactors = new HashSet<Factor>();

            if (_resultFactors.ToList().Remove((Factor)Initfactors.Find(f => f.displayName.Equals(listBoxControl2.SelectedItem))))
            {
                listBoxControl2.Items.Remove(listBoxControl2.SelectedItem);
            }

        }
    }
}

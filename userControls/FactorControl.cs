using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MLCommon;
using MLCommon.DAO;
using MLCommon.Entities;
using DevExpress.XtraGrid.Views.Grid;

namespace userControls
{
    public partial class FactorControl : UserControl
    {
        //页行数
        public int PageSize = 20;
        //当前页
        public int PageIndex = 1;
        //总页数
        public int PageCount;
        public event RowCellClickEventHandler factorRowCellClick;
        public event EventHandler addFactorClick;

        private String searchstr = String.Empty;
        public FactorControl()
        {
            InitializeComponent();
            //gridColumn7.GroupIndex = 0;
            //gridColumn8.GroupIndex = 1;
        }

        private void gridControl1_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

            //string type = button.ButtonType.ToString();
            string type = e.Button.Tag.ToString();
            if (type == "首页")
            {
                PageIndex = 1;
            }

            if (type == "下一页")
            {
                PageIndex++;
            }

            if (type == "末页")
            {
                PageIndex = PageCount;
            }

            if (type == "上一页")
            {
                PageIndex--;
            }


            //绑定分页控件和GridControl数据
            BindPageGridList();
        }

        private void BindPageGridList()
        {
            searchstr = String.IsNullOrEmpty(textEdit1.Text) ? "" : textEdit1.Text;
            OpaqueCommand cmd = new OpaqueCommand();
            cmd.ShowOpaqueLayer(gridControlFactor, 125, true);
            //记录获取开始数
            int startIndex = (PageIndex - 1) * PageSize + 1;
            //结束数
            int endIndex = PageIndex * PageSize;

            gridControlFactor.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = true;
            gridControlFactor.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = true;
            gridControlFactor.EmbeddedNavigator.Buttons.CustomButtons[2].Enabled = true;
            gridControlFactor.EmbeddedNavigator.Buttons.CustomButtons[3].Enabled = true;
            //总行数
            int row = FactorDB.GetFactorsCount(searchstr);

            //获取总页数  
            if (row % PageSize > 0)
            {
                PageCount = row / PageSize + 1;
            }
            else
            {
                PageCount = row / PageSize;
            }

            if (PageIndex == 1)
            {
                gridControlFactor.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;
                gridControlFactor.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = false;
            }

            //最后页时获取真实记录数
            if (PageCount == PageIndex)
            {
                endIndex = row;
                gridControlFactor.EmbeddedNavigator.Buttons.CustomButtons[2].Enabled = false;
                gridControlFactor.EmbeddedNavigator.Buttons.CustomButtons[3].Enabled = false;
            }

            //分页获取数据列表
            IList<Factor> als = FactorDB.GetFactors(searchstr, PageIndex, PageSize);
            gridControlFactor.DataSource = als;
            gridViewFactor.BestFitColumns();

            gridControlFactor.EmbeddedNavigator.TextStringFormat = string.Format("第 {0}页, 共 {1}页", PageIndex, PageCount);
            cmd.HideOpaqueLayer();

            gridViewFactor.ExpandAllGroups();
        }

        public void refreshData()
        {
            BindPageGridList();
        }

        private void FactorControl_Load(object sender, EventArgs e)
        {
            //BindPageGridList();
        }

        public void initControl()
        {

            BindPageGridList();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            BindPageGridList();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (addFactorClick != null)
                addFactorClick(sender, e);
        }

        private void gridViewFactor_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString().Trim();
            }

        }

        private void gridViewFactor_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (factorRowCellClick != null)
                factorRowCellClick(sender, e);
        }
    }
}

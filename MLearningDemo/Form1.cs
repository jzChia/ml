using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MLCommon;
using MLCommon.Entities;
using MLCommon.DAO;
using Aspose.Cells;

namespace MLearningDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Factor f1 = new Factor();
            f1.classification = "地层条件";
            f1.category = "成矿时代";
            f1.name = "三叠纪";

            Factor f2= new Factor();
            f2.classification = "地层条件";
            f2.category = "大地构造位置";
            f2.name = "大洋中脊";
            Factor f3 = new Factor();
            f3.classification = "构造条件";
            f3.category = "构造发育特征";
            f3.name = "深大断裂";
            Factor f4 = new Factor();
            f4.classification = "岩体条件";
            f4.category = "岩体顶面形态";
            f4.name = "岩体凹凸度";

            ProspectingModel pm = new ProspectingModel { name = "模型1" };
            pm.factors = new List<Factor>();

            pm.factors.Add(f1);
            pm.factors.Add(f2);
            pm.factors.Add(f3);
            pm.factors.Add(f4);

            f1.prospectingModels = new List<ProspectingModel>();
            f1.prospectingModels.Add(pm);
            f2.prospectingModels = new List<ProspectingModel>();
            f2.prospectingModels.Add(pm);
            f3.prospectingModels = new List<ProspectingModel>();
            f3.prospectingModels.Add(pm);
            f4.prospectingModels = new List<ProspectingModel>();
            f4.prospectingModels.Add(pm);

            //ProspectingModelDB.insertProspectingModel(pm);


            #region
            //Workbook CurrentWorkbook = new Workbook(@"E:\Desktop\zkmx - 副本.xlsx");
            //Factor factor;
            //for (int i = 0; i < CurrentWorkbook.Worksheets.Count; i++)
            //{
            //    string sheetname = CurrentWorkbook.Worksheets[i].Name;
            //    if (sheetname.Equals("Sheet1"))
            //    {
            //        Cells SelectedCells = CurrentWorkbook.Worksheets[sheetname].Cells;
            //        int col = SelectedCells.MaxColumn + 1;
            //        int row = SelectedCells.MaxRow + 1;
            //        for (int r = 2; r < row; r++)
            //        {
            //            for (int c = 0; c < col; c++)
            //            {
            //                Cell cellclassification = SelectedCells.CheckCell(0, c);
            //                int t = c;
            //                while (cellclassification == null || cellclassification.Value == null)
            //                {
            //                    cellclassification = SelectedCells.CheckCell(0, t--);
            //                }
            //                Cell cellcategory = SelectedCells.CheckCell(1, c);
            //                Cell cell = SelectedCells.CheckCell(r, c);
            //                if (cell != null && cellcategory != null && cellclassification != null)
            //                {
            //                    if (cell.Value == null || cellcategory.Value == null)
            //                    {

            //                    }
            //                    else
            //                    {
            //                        factor = new Factor
            //                        {
            //                            classification = cellclassification.Value.ToString(),
            //                            category = cellcategory.Value.ToString(),
            //                            name = cell.Value.ToString(),
            //                            author = "3s",
            //                            createTime = DateTime.Now,
            //                            updateTime = DateTime.Now

            //                        };
            //                        FactorDB.insertFactor(factor);


            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            #region
            Workbook CurrentWorkbook = new Workbook(@"E:\Desktop\zkmx - 副本.xlsx");
            Factor factor = new Factor();
            for (int i = 0; i < CurrentWorkbook.Worksheets.Count; i++)
            {
                string sheetname = CurrentWorkbook.Worksheets[i].Name;
                if (!sheetname.Equals("Sheet1"))
                {
                    Cells SelectedCells = CurrentWorkbook.Worksheets[sheetname].Cells;
                    int col = SelectedCells.MaxColumn + 1;
                    int row = SelectedCells.MaxRow + 1;
                    ProspectingModel pmo = new ProspectingModel();
                    Cell cellmodel = SelectedCells.CheckCell(0, 0);
                    if (cellmodel != null && cellmodel.Value != null)
                    {
                        pmo.name = cellmodel.Value.ToString();
                        pmo.author = "3s";
                        pmo.createTime = DateTime.Now;
                        pmo.updateTime = DateTime.Now;
                    }
                    else { continue; }

                    List<Factor> fss = new List<Factor>();
                    for (int r = 3; r < row; r++)
                    {
                        for (int c = 0; c < col; c++)
                        {
                            Cell cellclassification = SelectedCells.CheckCell(1, c);
                            int t = c;
                            while (cellclassification == null || cellclassification.Value == null)
                            {
                                cellclassification = SelectedCells.CheckCell(1, t--);
                            }
                            Cell cellcategory = SelectedCells.CheckCell(2, c);
                            Cell cell = SelectedCells.CheckCell(r, c);
                            if (cell != null && cellcategory != null && cellclassification != null)
                            {
                                if (cell.Value == null || cellcategory.Value == null)
                                {
                                    factor = new Factor();
                                }
                                //else if (cell.Value == null && !cellcategory.Value.ToString().Equals("构造"))
                                //{
                                //}
                                //else if (cell.Value == null && cellcategory.Value.ToString().Equals("构造"))
                                //{
                                //    if (cellcategory.GetDisplayStyle().BackgroundColor != Color.Transparent)
                                //    {
                                //        factor = FactorDB.GetFactorByClassandCategoryandName("受构造控制", cellclassification.Value.ToString(), cellcategory.Value.ToString());
                                //        if (factor != null && factor.prospectingModels != null)
                                //        {
                                //            factor.prospectingModels.Add(pmo);
                                //        }
                                //    }
                                //}
                                else
                                {
                                    factor = FactorDB.GetFactorByClassandCategoryandName(cell.Value.ToString(), cellclassification.Value.ToString(), cellcategory.Value.ToString());
                                    if (factor != null && factor.prospectingModels != null)
                                    {
                                        factor.prospectingModels.Add(pmo);
                                    }
                                }

                                if (factor != null && !String.IsNullOrEmpty(factor.factorId))
                                {
                                    fss.Add(factor);
                                }
                            }
                        }
                    }

                    pmo.factors = fss;

                    ProspectingModelDB.insertProspectingModel(pmo);
                }
            }
            #endregion
        }
    }
}

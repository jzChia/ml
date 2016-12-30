using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MLCommon.Entities;
using System.Data;

namespace MLCommon.DAO
{
	public class MLCalc
	{
		public DataSet calcDisplay( String[] names)
		{
			Dictionary<ProspectingModel, HashSet<String>> pmdic = ProspectingModelDB.GetProspectingModelByName(names);
			using (DataTable mdoeldt = calcModelDisplays(pmdic))
			{
				using (DataTable factordt = calcFactorDisplays(pmdic))
				{
					DataSet ds = new DataSet();
					ds.Tables.AddRange(new DataTable[] { mdoeldt.Copy(), factordt.Copy() });
					DataRelation relation = new DataRelation("FactorTable", ds.Tables[0].Columns[0], ds.Tables[1].Columns[0], false);
					ds.Relations.Add(relation);
					// 添加  
					return ds;
				}
			}
		}

		public void calcModel(Dictionary<ProspectingModel, HashSet<String>> pmdic, List<Factor> fs)
		{
			if (pmdic == null || fs==null)
				return;

			List<ProspectingModel> pms = pmdic.Keys.ToList();
			List<Factor> dset = FactorDB.GetFactorByProspectingModels(pms);

			List<Factor> modelfactors = dset!=null?dset:new List<Factor>();
			var mclassList = modelfactors.GroupBy(f => new { f.classification, f.category }).Select(k => k.Key).ToList();
			var classList = fs.GroupBy(f => new { f.classification, f.category }).Select(k => k.Key).ToList();

			List<Factor> resConfirmFactors = new List<Factor>();
			List<Factor> resNoConfirmFactors = new List<Factor>();
			List<Factor> mresNoConfirmFactors = new List<Factor>();
			fs.ForEach(i => resNoConfirmFactors.Add(i));
			modelfactors.ForEach(i => mresNoConfirmFactors.Add(i));
			foreach (var f in fs)
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

		public static DataTable calcModelDisplays(Dictionary<ProspectingModel, HashSet<String>> pmdic)
		{
			DataTable dt = initModelTable();
			DataRow dr;
			StringBuilder sb;
			foreach (var item in pmdic)
			{
				dr = dt.NewRow();
				sb = new StringBuilder();
				foreach (var v in item.Value)
				{
					sb.Append(v + ",");
				}
				dr["关键词"] = sb.ToString().TrimEnd(',');
				dr["关键词数"] = item.Value.Count;
				calcModelDisplay(item.Key, ref dr);
				dt.Rows.Add(dr);
			}
			dt.Columns[0].ColumnMapping = MappingType.Hidden;
			dt.DefaultView.Sort = "模型名称";
			return dt;
		}

		public static DataTable calcFactorDisplays(Dictionary<ProspectingModel, HashSet<String>> pmdic)
		{
			List<ProspectingModel> pms = pmdic.Keys.ToList();
            List<Factor> dset = FactorDB.GetFactorByProspectingModels(pms);

			DataTable factorTable = new DataTable("FactorTable");
			factorTable.Columns.Add(new DataColumn("模型编号", typeof(String)));
			factorTable.Columns.Add(new DataColumn("模型名称", typeof(String)));
			factorTable.Columns.Add(new DataColumn("要素大类", typeof(String)));
			factorTable.Columns.Add(new DataColumn("要素类型", typeof(String)));
			factorTable.Columns.Add(new DataColumn("要素名称", typeof(string)));
			factorTable.Columns.Add(new DataColumn("要素使用率", typeof(double)));
			factorTable.Columns.Add(new DataColumn("重要性", typeof(double)));
			DataRow dr;
			List<Factor> fs = new List<Factor>();
			List<Factor> setfs =dset!=null? dset.ToList():new List<Factor>();
			foreach (var item in pms)
			{
				if (item.factors != null)
					fs = item.factors.ToList();

				foreach (var f in fs)
				{
					dr = factorTable.NewRow();
					dr["模型编号"] = item.modelId;
					dr["模型名称"] = item.name;
					dr["要素大类"] = f.classification;
					dr["要素类型"] = f.category;
					dr["要素名称"] = f.name;
					//dicf.Contains(f)
					Factor fi = setfs.Find(p=>p.Equals(f));
					if (fi!=null)
					{
						dr["要素使用率"] = fi.frequency;
						dr["重要性"] = fi.sig;

					}
					factorTable.Rows.Add(dr);
				}

			}
			factorTable.Columns[0].ColumnMapping = MappingType.Hidden;
			factorTable.DefaultView.Sort = "要素大类,要素类型,要素名称";
			return factorTable;
		}


		private static void calcModelDisplay(ProspectingModel pm,ref DataRow dr)
		{
			dr["模型编号"] = pm.modelId;
			dr["模型名称"] = pm.name;
			dr["控矿要素数"] = pm.factors != null ? pm.factors.Count : 0;

		}

		private static DataTable initModelTable()
		{
			DataTable modelTable = new DataTable("找矿模型");
			modelTable.Columns.Add(new DataColumn("模型编号", typeof(String)));
			modelTable.Columns.Add(new DataColumn("模型名称", typeof(String)));
			modelTable.Columns.Add(new DataColumn("关键词", typeof(String)));
			modelTable.Columns.Add(new DataColumn("关键词数", typeof(int)));
			modelTable.Columns.Add(new DataColumn("控矿要素数", typeof(int)));
			return modelTable;
		}
	}
}

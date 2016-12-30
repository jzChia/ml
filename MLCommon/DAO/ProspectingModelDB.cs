using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MLCommon.Entities;
using NHibernate;
using NHibernate.Criterion;
using System.IO;
using System.Windows.Forms;

namespace MLCommon.DAO
{
	public class ProspectingModelDB
	{
		public static int insertProspectingModel(ProspectingModel p)
		{
			if (p == null)
				return 0;

			return EntityDB<ProspectingModel>.saveOrUpdateT(p);
		}
		

		public static int UpdateProspectingModel(ProspectingModel p)
		{
			if (p == null)
				return 0;
			return EntityDB<ProspectingModel>.updateT(p);
		}

        public static int removeFacor(String modelid)
        {
            if (String.IsNullOrEmpty(modelid)) return 0;
            return EntityDB<int>.executeBySQL("delete from ProspectingModelFactor where modelId=\"" + modelid + "\"");
        }

		public static int removeFacor(String modelid,Factor f)
		{
			return EntityDB<int>.executeBySQL("delete from ProspectingModelFactor where modelId=\"" + modelid + "\" and factorId = \"" + f.factorId + "\"");
		}

		public static int removeFacor(String modelid, String classification,String category)
		{
			ProspectingModel pm = GetProspectingModelById(modelid);
			if(pm.factors==null || (String.IsNullOrWhiteSpace(category) && String.IsNullOrWhiteSpace(classification)))
				return -1;
			List<Factor> fs =null;
			if(String.IsNullOrWhiteSpace(classification))
				fs =pm.factors.ToList().FindAll(p => p.classification.Equals(classification));
			else   if (String.IsNullOrWhiteSpace(category))
				fs = pm.factors.ToList().FindAll(p => p.category.Equals(category));
			if(fs==null)
				return -1;
			foreach (var f in fs)
			{
				EntityDB<int>.executeBySQL("delete from ProspectingModelFactor where modelId=\"" + modelid + "\" and factorId = \"" + f.factorId + "\"");;
			}
			return 1;
		}


		public static int DeleteProspectingModel(ProspectingModel p)
		{
			if (p == null)
				return 0;
			return EntityDB<ProspectingModel>.deleteT(p);
		}

		public static ProspectingModel GetProspectingModelById(string modelid)
		{
			if (String.IsNullOrEmpty(modelid))
				return null; 
			using (ISession session = MYNHibernate.GetSession())
			{
				using (ITransaction trans = MYNHibernate.GeTransaction(session))
				{
					ProspectingModel pm = session.Get<ProspectingModel>(modelid);
					if (pm != null && pm.factors!=null && pm.factors.Count>0 )
                    {
                        foreach (var item in pm.factors)
                        {
                            if (item != null && item.prospectingModels != null && item.prospectingModels.Count > 0)
                            { }
                        }
					}

					return pm;
				}
			}
		}

		public static IList<ProspectingModel> GetProspectingModelByName(string name)
		{
			IList<ProspectingModel> pms = new List<ProspectingModel>();
			using (ISession session = MYNHibernate.GetSession())
			{
				using (ITransaction trans = MYNHibernate.GeTransaction(session))
				{

					ICriteria crit = getCrit(session, name);
					pms = crit.List<ProspectingModel>();
					foreach (var pm in pms)
					{
						if (pm != null && pm.factors != null && pm.factors.Count > 0)
						{
							foreach (var item in pm.factors)
							{
								if (item != null && item.prospectingModels != null && item.prospectingModels.Count > 0)
								{ }
							}
						}
					}
				}
			}
			return pms;
		
		}

		private static DateTime getLastUpdateTime()
		{
			return EntityDB<DateTime>.getTBySQL("select max(updateTime) from ProspectingModel");
		}

		public static List<ProspectingModel> GetProspectingModels()
		{
			IList<ProspectingModel> pms = new List<ProspectingModel>();
			using (ISession session = MYNHibernate.GetSession())
			{
				using (ITransaction trans = MYNHibernate.GeTransaction(session))
				{

					ICriteria crit = getCrit(session, null);
					pms = crit.List<ProspectingModel>();
				}
			}
			return pms!=null?pms.ToList():null;
		
		}

		private static ICriteria getCrit(ISession session, String name)
		{

			ICriteria crit = session.CreateCriteria<ProspectingModel>();

			if (!String.IsNullOrEmpty(name))
				crit.Add(Restrictions.Like("name", name, MatchMode.Anywhere));

			crit.AddOrder(Order.Asc("name"));

			return crit;
		}

		public static Dictionary<ProspectingModel, HashSet<String>> GetProspectingModelByName(string[] names)
		{
			IList<ProspectingModel> lists = new List<ProspectingModel>();
			Dictionary<ProspectingModel, HashSet<String>> pmdic = new Dictionary<ProspectingModel, HashSet<String>>();
			HashSet<String> obj;
			foreach (var name in names)
			{
				obj = new HashSet<string>();
				lists = GetProspectingModelByName(name);
				foreach (var pm  in lists)
				{
					pmdic.TryGetValue(pm, out obj);
					if (obj == null)
						obj = new HashSet<string>();
					obj.Add(name);          
					pmdic.Add(pm, obj);
				}
			}
			return pmdic;
		}   

	}
}

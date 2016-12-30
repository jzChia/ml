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
    public class FactorDB
    {
        public static int insertFactor(Factor f)
        {
            if (f == null)
                return 0;
            int res = EntityDB<Factor>.saveOrUpdateT(f);
            return res;
        }

        public static int UpdateFactor(Factor f)
        {
            if (f == null)
                return 0;
            int res = EntityDB<Factor>.updateT(f);
            return res;
        }

        public static int DeleteFactor(Factor f)
        {
            if (f == null)
                return 0;
            int res = EntityDB<Factor>.deleteT(f);
            return res;
        }

        public static Factor GetFactorById(string factorid)
        {
            if (String.IsNullOrEmpty(factorid))
                return null;
            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {
                    Factor f = new Factor();
                    f = session.Get<Factor>(factorid);
                    if (f != null && f.prospectingModels != null && f.prospectingModels.Count > 0)
                    { }
                    trans.Commit();
                    return f;
                }
            }
        }


        public static List<Factor> GetFactorsByFactors(List<Factor> fs)
        {

            if (fs==null)
                return null;
            List<Factor> factors = new List<Factor>();
            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {
                    ICriteria crit = session.CreateCriteria<Factor>();

                    crit.Add(Restrictions.In("factorId", fs.GroupBy(f=>f.factorId).Select(s=>s.Key).ToList()));
                    factors = crit.List<Factor>().ToList();

                    foreach (var item in factors)
                    {
                        if (item != null && item.prospectingModels != null && item.prospectingModels.Count > 0)
                        { }
                    }

                }
            }
            return factors;
        }

        public static IList<Factor> GetFactorByName(string name)
        {
            IList<Factor> factors = EntityDB<Factor>.getTListByHQL("from Factor where name=?", new object[] { name });
            return factors;
        }

        public static Factor GetFactorByClassandCategoryandName(string name, String classification, string category)
        {
            Factor factor = null; 
            ISession session = MYNHibernate.GetSession();
            ITransaction trans = MYNHibernate.GeTransaction(session);
            try
            {
                IQuery query = session.CreateQuery("from Factor where name=? and classification=? and category=?")
                    .SetString(0,name)
                    .SetString(1,classification)
                    .SetString(2,category);

                factor = query.UniqueResult<Factor>();
                trans.Commit(); 
                    if (factor != null && factor.prospectingModels != null && factor.prospectingModels.Count > 0)
                    { }
            }
            catch { }
            finally
            {
                MYNHibernate.Closed(session, trans);
            }

            return factor;
        }


        private static DateTime getLastUpdateTime()
        {
            return EntityDB<DateTime>.getTBySQL("select max(updateTime) from Factor");
        }

        public static List<Factor> GetFactors()
        {
            List<Factor> factor = null;
            factor = EntityDB<Factor>.getAllTList().ToList();
            return factor; 
        }

        public static List<Factor> GetFactorByProspectingModels(List<ProspectingModel> lists)
        {
            if (lists == null) return null;
            List<Factor> fset = new List<Factor>();
            List<Factor> fs = new List<Factor>();
            List<double> l = new List<double>();
            Factor fac;
            foreach (var li in lists)
            {
                fs = li.factors != null ?new List<Factor>( li.factors) : new List<Factor>();
                foreach (var f in fs)
                {
                    if (fset.Contains(f))
                    {
                        fac = ((Factor)fset.Find(g => g.Equals(f))).clone();
                        fset.Remove(f);
                    }
                    else
                    fac = f.clone();
                    int sum = fs.Count(p => p.classification.Equals(fac.classification) && p.category.Equals(fac.category));
                    fac.frequency = fac.frequency + 1;
                    fac.significance = fac.significance + 1.0 / sum;
                    fset.Add(fac);
                }
            }
            return fset;
        }

        public static List<Factor> GetFactorByProspectingModelsAndFactors(List<ProspectingModel> lists, List<Factor> initFactor, out List<ProspectingModel> outpms)
        {
            outpms = new List<ProspectingModel>();
            if (lists == null) return null;
            List<Factor> fset = new List<Factor>();
            List<Factor> fs = new List<Factor>();
            List<double> l = new List<double>();
            Factor fac;
            foreach (var li in lists)
            {
                fs = li.factors != null ? new List<Factor>(li.factors) : new List<Factor>();
                if (initFactor.Intersect(fs).Count() > 0)
                {
                    outpms.Add(li);
                    foreach (var f in fs)
                    {
                        if (fset.Contains(f))
                        {
                            fac = ((Factor)fset.Find(g => g.Equals(f))).clone();
                            fset.Remove(f);
                        }

                        else
                            fac = f.clone();
                        int sum = fs.Count(p => p.classification.Equals(fac.classification) && p.category.Equals(fac.category));
                        fac.frequency = fac.frequency + 1;
                        fac.significance = fac.significance + 1.0 / sum;
                        fset.Add(fac);
                    }
                }
            }
            return fset;
        }

        public static int removeModel(String factorid)
        {
            if (String.IsNullOrEmpty(factorid)) return 0;
            return EntityDB<int>.executeBySQL("delete from ProspectingModelFactor where factorId=\"" + factorid + "\"");
        }

        public static IList<Factor> GetFactors(string str, int pageIndex, int pageSize)
        {
            IList<Factor> factors = new List<Factor>();
            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {

                    ICriteria crit = getCrit(session, str);
                    crit.SetFirstResult((pageIndex - 1) * pageSize);
                    crit.SetMaxResults(pageSize);
                    factors = crit.List<Factor>();
                }
            }
            return factors;
        }

        public static List<String> getAllClassification()
        {
            IList<String> list = EntityDB<String>.getTListBySQL("select classification from factor group by classification");

            return list != null ? list.ToList() : new List<String>();
        }

        public static List<String> getAllCategoryByClassification(string classification)
        {
            if (String.IsNullOrWhiteSpace(classification))
                return new List<String>();
            IList<String> list = EntityDB<String>.getTListBySQL("select category from factor where classification = \"" + classification + "\" group by category");

            return list != null ? list.ToList() : new List<String>();
        }

        private static ICriteria getCrit(ISession session, String name)
        {
            ICriteria crit = session.CreateCriteria<Factor>();
            if (!String.IsNullOrEmpty(name))
                crit.Add(Restrictions.Like("name", name,MatchMode.Anywhere));

            crit.AddOrder(Order.Asc("classification")).AddOrder(Order.Asc("category")).AddOrder(Order.Asc("name"));

            return crit;
        }

        public static int GetFactorsCount(string str)
        {
            int rowcount = 0;
            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {
                    
                    ICriteria crit = getCrit(session, str);
                    
                    rowcount = (int)crit.SetProjection(Projections.RowCount()).UniqueResult();
                    
                }
            }
            return rowcount;

        }

        public static List<String> getClassification(List<Factor> factor)
        {
            if (factor == null)
                return null;
            List<String> cls = factor.Select(f => f.classification).Distinct().ToList();

            return cls;
        }

        public static List<String> getCategory(List<Factor> factor, string cls)
        {
            List<String> ctr = factor.FindAll(f => f.classification.Equals(cls)).Select(f => f.category).Distinct().ToList();

            return ctr;
        }


        public static List<Factor> getFactorNames(List<Factor> factor, string cls,string ctr)
        {
            List<Factor> fs = factor.FindAll(f => f.classification.Equals(cls) && f.category.Equals(ctr));

            return fs;
        }
       
    }
}

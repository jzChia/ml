using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace MLCommon.DAO
{
    public class EntityDB<T>
    {
        public static int importT(T t)
        {
            int res = 0;
            if (t == null)
                return 0;
            ISession session = MYNHibernate.GetSession();
            ITransaction trans = MYNHibernate.GeTransaction(session);
            try
            {
                session.Save(t);
                trans.Commit();
                res = 1;
            }
            catch { res = -1; }
            finally { MYNHibernate.Closed(session, trans); }

            return res;
        }

        public static int deleteT(T t)
        {
            int res = 0;
            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {
                        session.Delete(t);
                        trans.Commit();
                        res = 1;
                }
            }
            return res;
        
        }

        public static int updateT(T t)
        {
            int res = 0;

            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {
                        session.Update(t);
                        trans.Commit();
                        res = 1;
                }
            }
            return res;
        }

        public static int executeByHQL(string hql ,params object[] values)
        {
            int res = 0;

            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {
                        IQuery query = session.CreateQuery(hql);
                        getQueryStr(ref query, values);
                        res = query.ExecuteUpdate();
                        trans.Commit();
                }
            }
            return res;
        }

        public static int executeBySQL(string sql, params object[] values)
        {
            int res = 0;

            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {
                        ISQLQuery query = session.CreateSQLQuery(sql);
                        getSQLQueryStr(ref query, values);
                        res = query.ExecuteUpdate();
                        trans.Commit();
                }
            }
            return res;
        }
        public static int saveOrUpdateT(T t)
        {
            int res = 0;
            if (t == null)
                return 0;
            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {
                        session.SaveOrUpdate(t);
                        trans.Commit();
                        res = 1;
                }
            }

            return res;
        }

        public static T getTById(string id)
        {
            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {
                    
                    T t = default(T);
                        t = session.Get<T>(id);
                        trans.Commit();
                    return t;
                }
            }
        }


        public static T loadTById(string id)
        {
            using (ISession session = MYNHibernate.GetSession())
            {
                using (ITransaction trans = MYNHibernate.GeTransaction(session))
                {

                    T t = default(T);
                    t = session.Load<T>(id);
                    trans.Commit();
                    return t;
                }
            }
        }

        public static IList<T> getAllTList()
        {
            ISession session = MYNHibernate.GetSession();
            ITransaction trans = MYNHibernate.GeTransaction(session);


            IList<T> pis = new List<T>();
            try
            {

                string sql = "from "+typeof(T).Name;
                pis = session.CreateQuery(sql).List<T>();

                trans.Commit();
            }
            catch { }
            finally
            {
                MYNHibernate.Closed(session, trans);
            }
            return pis;
        }

        public static T getTByHQL(string sql,params object[] values)
        {
            ISession session = MYNHibernate.GetSession();
            ITransaction trans = MYNHibernate.GeTransaction(session);
            T t = default(T);
            try
            {
                IQuery query = session.CreateQuery(sql);

                getQueryStr(ref query, values);
                
                t = query.UniqueResult<T>();

                trans.Commit();
            }
            catch { }
            finally
            {
                MYNHibernate.Closed(session, trans);
            }
            return t;

        }

        public static IList<T> getTListByHQL(string sql, params object[] values)
        {
            ISession session = MYNHibernate.GetSession();
            ITransaction trans = MYNHibernate.GeTransaction(session);
            IList<T> ts = new List<T>();
            try
            {
                IQuery query = session.CreateQuery(sql);

                getQueryStr(ref query, values);

                ts = query.List<T>();

                trans.Commit();
            }
            catch { }
            finally
            {
                MYNHibernate.Closed(session, trans);
            }
            return ts;

        }

        public static T getTBySQL(string sql, params object[] values)
        {
            ISession session = MYNHibernate.GetSession();
            ITransaction trans = MYNHibernate.GeTransaction(session);
            T t = default(T);
            try
            {
                ISQLQuery query = session.CreateSQLQuery(sql);

                getSQLQueryStr(ref query, values);
                t = query.UniqueResult<T>();
                trans.Commit();
            }
            catch { }
            finally
            {
                MYNHibernate.Closed(session, trans);
            }
            return t;

        }

        public static IList<T> getTListBySQL(string sql, params object[] values)
        {
            ISession session = MYNHibernate.GetSession();
            ITransaction trans = MYNHibernate.GeTransaction(session);
            IList<T> t = new List<T>();
            try
            {
                ISQLQuery query = session.CreateSQLQuery(sql);

                getSQLQueryStr(ref query, values);
                t = query.List<T>();
                trans.Commit();
            }
            catch { }
            finally
            {
                MYNHibernate.Closed(session, trans);
            }
            return t;

        }

        private static void getSQLQueryStr(ref ISQLQuery query, params object[] values)
        {
            if (values == null) return;
            for (int i = 0; i < values.Length; i++)
            {
                Type type = values[i].GetType();
                switch (type.Name.ToLower())
                {
                    case "int32":
                        query = (ISQLQuery)query.SetInt32(i, Convert.ToInt32(values[i]));
                        break;
                    case "double":
                        query = (ISQLQuery)query.SetDouble(i, Convert.ToDouble(values[i]));
                        break;
                    case "datetime":
                        query = (ISQLQuery)query.SetDateTime(i, Convert.ToDateTime(values[i]));
                        break;
                    case "string":
                        query = (ISQLQuery)query.SetString(i, Convert.ToString(values[i]));
                        break;
                    case "boolean":
                        query = (ISQLQuery)query.SetBoolean(i, Convert.ToBoolean(values[i]));
                        break;
                    case "decimal":
                        query = (ISQLQuery)query.SetDecimal(i, Convert.ToDecimal(values[i]));
                        break;
                    case "char":
                        query = (ISQLQuery)query.SetCharacter(i, Convert.ToChar(values[i]));
                        break;
                    default:
                        query = null;
                        break;
                }
            }

        }
    
        private static void getQueryStr(ref IQuery query, params object[] values)
        {
            if (values == null)
            {
                return;
            }

            for (int i = 0; i < values.Length; i++)
            {
                Type type = values[i].GetType();
                switch (type.Name.ToLower())
                {
                    case "int32":
                        query = query.SetInt32(i, Convert.ToInt32(values[i]));
                        break;
                    case "double":
                        query = query.SetDouble(i, Convert.ToDouble(values[i]));
                        break;
                    case "datetime":
                        query = query.SetDateTime(i, Convert.ToDateTime(values[i]));
                        break;
                    case "string":
                        query = query.SetString(i, Convert.ToString(values[i]));
                        break;
                    case "boolean":
                        query = query.SetBoolean(i, Convert.ToBoolean(values[i]));
                        break;
                    case "decimal":
                        query = query.SetDecimal(i, Convert.ToDecimal(values[i]));
                        break;
                    case "char":
                        query = query.SetCharacter(i, Convert.ToChar(values[i]));
                        break;
                    default:
                        query =null;
                        break;
                }
            }
        }

        
    }
}

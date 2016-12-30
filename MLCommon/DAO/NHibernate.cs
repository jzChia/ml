using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using System.Reflection;
using NHibernate.Mapping.Attributes;
using MLCommon.Entities;

namespace MLCommon.DAO
{
    public class MYNHibernate
    {
        public static ISession GetSession()
        {
            try
            {
                Configuration cfg = new Configuration().Configure();
                Assembly assembly = typeof(UserInfo).Assembly;

                cfg.AddInputStream(HbmSerializer.Default.Serialize(assembly));
                ISessionFactory factory = cfg.BuildSessionFactory();

                ISession session = factory.OpenSession();
                return session;
            }
            catch
            {
                return null;
            }

        }

        public static ITransaction GeTransaction(ISession session)
        {
            try
            {
                ITransaction transaction = session.BeginTransaction();
                return transaction;
            }
            catch
            {
                return null;
            }
        }

        public static void Closed(ISession session, ITransaction transaction)
        {
            if (session == null || transaction == null)
                return;
            try
            {
                session.Close();
                transaction.Dispose();
            }
            catch
            {

            }
        }


        public static IStatelessSession GetStatelessSession()
        {
            try
            {
                Configuration cfg = new Configuration().Configure();

                Assembly assembly = typeof(UserInfo).Assembly;

                cfg.AddInputStream(HbmSerializer.Default.Serialize(assembly));
                ISessionFactory factory = cfg.BuildSessionFactory();

                IStatelessSession session = factory.OpenStatelessSession();
                return session;
            }
            catch
            {
                return null;
            }

        }

        public static ITransaction GetStatelessTransaction(IStatelessSession session)
        {
            try
            {
                ITransaction transaction = session.BeginTransaction();
                return transaction;
            }
            catch
            {
                return null;
            }
        }

        public static void StatelessClosed(IStatelessSession session, ITransaction transaction)
        {
            if (session == null || transaction == null)
                return;
            try
            {
                session.Close();
                transaction.Dispose();
            }
            catch
            {

            }
        }


    }
}

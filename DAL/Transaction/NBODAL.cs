using DAL.Common;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Transaction
{
    public class NBODAL : Common.CommonDAL<INBO>
    {
        public IList<INBO> GetClientFile(int id)
        {
            IList<INBO> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(INBO))
               .Add(NHibernate.Criterion.Restrictions.Eq("Client.Id", id))
               .List<INBO>();
            return obj;
        }

        public void AddDocument(INboDocument obj, int id)
        {
            INBO bl = GetById(id);
            bl.DocumentList.Add(obj);
            InsertOrUpdate(bl);
        }

        public IList<INBO> GetCompltedTask()
        {
            IList<INBO> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(INBO))
               .Add(NHibernate.Criterion.Restrictions.IsNotNull("Completed"))
               .Add(!NHibernate.Criterion.Restrictions.Eq("Status",4))
               .List<INBO>();
            return obj;
        }

        public IList<INBO> GetPendingTask()
        {
            IList<INBO> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(INBO))
               .Add(NHibernate.Criterion.Restrictions.IsNull("Completed"))
               .List<INBO>();
            return obj;
        }

        public static string GetMaxTaskNo()
        {
            var obj = NHibernateHelper.OpenSession()
             .CreateCriteria(typeof(INBO))
             .SetProjection(Projections.Max("FileNumber"))
             .UniqueResult();
            if (obj != null)
                return obj.ToString();
            else
                return "";
        }

        public static int GetLatestTaskId()
        {
            var obj = NHibernateHelper.OpenSession()
             .CreateCriteria(typeof(INBO))
             .SetProjection(Projections.Max("Id"))
             .UniqueResult();
            return (int)obj;
        }
    }
}

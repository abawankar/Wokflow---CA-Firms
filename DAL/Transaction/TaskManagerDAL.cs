using DAL.Common;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Transaction
{
    public class TaskManagerDAL : Common.CommonDAL<ITaskManager>
    {
        public IList<ITaskManager> GetByAssigner(int empid)
        {
            IList<ITaskManager> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ITaskManager))
               .CreateAlias("Contacts", "emp")
               .Add(NHibernate.Criterion.Restrictions.Eq("emp.Id", empid))
               .List<ITaskManager>();
            return obj;
        }

        public IList<ITaskManager> GetByAssignee(int empid)
        {
            IList<ITaskManager> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ITaskManager))
               .Add(NHibernate.Criterion.Restrictions.Eq("Assigneer.Id", empid))
               .List<ITaskManager>();
            return obj;
        }

        public IList<ITaskManager> GetTaskByFileId(int fileId)
        {
            IList<ITaskManager> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ITaskManager))
               .Add(NHibernate.Criterion.Restrictions.Eq("FileNumber.Id", fileId))
               .List<ITaskManager>();
            return obj;
        }

        public IList<ITaskManager> GetTodayTask()
        {
            DateTime initDate = System.DateTime.Now.Date;
            DateTime endDate = System.DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            IList<ITaskManager> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ITaskManager))
               .Add(NHibernate.Criterion.Expression.Eq("Start",initDate))
               .List<ITaskManager>();
            return obj;
        }

        public IList<ITaskManager> GetPendingTask()
        {
            DateTime initDate = System.DateTime.Now.Date;
            DateTime endDate = System.DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            int status = 3;
            IList<ITaskManager> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ITaskManager))
               .Add(NHibernate.Criterion.Restrictions.Not(NHibernate.Criterion.Expression.Eq("Start", initDate)))
               .Add(NHibernate.Criterion.Restrictions.Not(NHibernate.Criterion.Expression.Eq("Status", status)))
               .List<ITaskManager>();
            return obj;
        }

        public IList<ITaskManager> GetByAssignerPending(int empid)
        {
            DateTime initDate = System.DateTime.Now.Date;
            DateTime endDate = System.DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            int status = 3;
            IList<ITaskManager> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ITaskManager))
               .CreateAlias("Contacts", "emp")
               .Add(NHibernate.Criterion.Restrictions.Eq("emp.Id", empid))
               .Add(NHibernate.Criterion.Restrictions.Not(NHibernate.Criterion.Expression.Eq("Status", status)))
               .Add(NHibernate.Criterion.Restrictions.Not(NHibernate.Criterion.Expression.Eq("Start", initDate)))
               .List<ITaskManager>();
            return obj;
        }

        public void AddComments(ITaskComments obj, int id)
        {
            ITaskManager bl = GetById(id);
            bl.Comments.Add(obj);
            InsertOrUpdate(bl);
        }

        public IList<ITaskManager> GetByConsultant(int consltId)
        {
            IList<ITaskManager> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ITaskManager))
               .Add(NHibernate.Criterion.Restrictions.Eq("Consultant.Id", consltId))
               .List<ITaskManager>();
            return obj;
        }

        public IList<ITaskManager> GetByConsultantPending(int consltId)
        {
            DateTime initDate = System.DateTime.Now.Date;
            DateTime endDate = System.DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            int status = 3;
            IList<ITaskManager> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ITaskManager))
               .Add(NHibernate.Criterion.Restrictions.Eq("Consultant.Id", consltId))
               .Add(NHibernate.Criterion.Restrictions.Not(NHibernate.Criterion.Expression.Eq("Status", status)))
               .Add(NHibernate.Criterion.Restrictions.Not(NHibernate.Criterion.Expression.Eq("Start", initDate)))
               .List<ITaskManager>();
            return obj;
        }
    }
}

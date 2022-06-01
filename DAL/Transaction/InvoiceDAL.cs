using DAL.Common;
using Domain.Implementation.Transaction;
using Domain.Interface.Transaction;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Transaction
{
    public class InvoiceDAL : Common.CommonDAL<IInvoice>
    {
        public static string GetMaxInvoice(int compid)
        {
            var obj = NHibernateHelper.OpenSession()
             .CreateCriteria(typeof(IInvoice))
             .Add(NHibernate.Criterion.Restrictions.Eq("Company.Id",compid ))
             .SetProjection(Projections.Max("Id"))
             .UniqueResult();
            if (obj != null )
            {
                IInvoice obj1 = NHibernateHelper
               .OpenSession()
               .CreateCriteria(typeof(IInvoice))
               .Add(NHibernate.Criterion.Restrictions.Eq("Id", (int)obj))
               .SetFirstResult(0)
               .UniqueResult<IInvoice>();

                return obj1.InvoiceNo;
            }
            else
                return "";
        }

        public void AddTask(int invid, string tasklist)
        {
            IInvoice bl = GetById(invid);
            string[] taskid = tasklist.Split(',');
            for (int i = 1; i < taskid.Length; i++)
            {
                int id = Convert.ToInt32(taskid[i]);
                NBODAL dal = new NBODAL();
                INBO task = dal.GetById(id);
                task.BillStatus = 2;
                dal.InsertOrUpdate(task);

                InvoiceTrn tr = new InvoiceTrn();
                tr.Particulars = task.TaskTypes.Particulars;
                tr.Amount = task.Cost;
                tr.Task = task;

                bl.InvTrn.Add(tr);

            }
            InsertOrUpdate(bl);
        }
    }
}

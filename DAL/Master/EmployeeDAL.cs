using DAL.Common;
using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Master
{
    public class EmployeeDAL : Common.CommonDAL<IEmployee>
    {
        public static IEmployee GetByAppLogin(string Emailid)
        {
            IEmployee obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IEmployee))
            .Add(NHibernate.Criterion.Restrictions.Eq("Emailid", Emailid))
            .SetFirstResult(0)
            .UniqueResult<IEmployee>();

            return obj;
        }

        public static string GetName(string Emailid)
        {
            IEmployee obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IEmployee))
            .Add(NHibernate.Criterion.Restrictions.Eq("Emailid", Emailid))
            .SetFirstResult(0)
            .UniqueResult<IEmployee>();

            return obj.Name;
        }

        public void AddRights(int empId, string rightsList)
        {
            IEmployee bl = GetById(empId);
            EmpRightsDAL dal = new EmpRightsDAL();
            string[] rightsid = rightsList.Split(',');
            List<IEmpRights> list = new List<IEmpRights>();
            for (int i = 1; i < rightsid.Length; i++)
            {
                int id = Convert.ToInt32(rightsid[i]);
                IEmpRights empRights = dal.GetById(id);
                bl.EmpRight.Add(empRights);
            }
            InsertOrUpdate(bl);
        }

        public void AddClients(int empid, string clientlist)
        {
            IEmployee bl = GetById(empid);
            ClientsDAL dal = new ClientsDAL();
            string[] clientid = clientlist.Split(',');
            for (int i = 1; i < clientid.Length; i++)
            {
                int id = Convert.ToInt32(clientid[i]);
                IClients client = dal.GetById(id);
                bl.Client.Add(client);
            }
            InsertOrUpdate(bl);
        }
    }
}

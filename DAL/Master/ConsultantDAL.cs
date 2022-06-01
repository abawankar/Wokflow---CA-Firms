using DAL.Common;
using Domain.Implementation.Master;
using Domain.Interface.Master;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Master
{
    public class ConsultantDAL : Common.CommonDAL<IConsultant>
    {
        public void AddClients(int ConsltId, string clientlist)
        {
            IConsultant bl = GetById(ConsltId);
            ClientsDAL dal = new ClientsDAL();
            string[] clientid = clientlist.Split(',');
            for (int i = 1; i < clientid.Length; i++)
            {
                int id = Convert.ToInt32(clientid[i]);
                IClients client = dal.GetById(id);
                bl.Clients.Add(client);
            }
            InsertOrUpdate(bl);
        }

        public static IList<Consultant> ConsultantOptions()
        {
            IList<Consultant> obj = NHibernateHelper.OpenSession()
             .CreateCriteria(typeof(Consultant))
              .SetProjection(Projections.ProjectionList()
              .Add(Projections.Property("Id"), "Id")
              .Add(Projections.Property("Name"), "Name")).List<IList>()
              .Select(l => new Consultant() { Id = (int)l[0], Name = (string)l[1] })
              .ToList();
            return obj;
        }

        public static IConsultant GetByAppLogin(string Emailid)
        {
            IConsultant obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IConsultant))
            .Add(NHibernate.Criterion.Restrictions.Eq("MailId", Emailid))
            .SetFirstResult(0)
            .UniqueResult<IConsultant>();

            return obj;
        }
    }
}

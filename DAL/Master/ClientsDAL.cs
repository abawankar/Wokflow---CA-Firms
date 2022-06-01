using DAL.Common;
using Domain.Interface.Master;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Loader;
using System.Collections;
using Domain.Implementation.Master;

namespace DAL.Master
{
    public class ClientsDAL : Common.CommonDAL<IClients>
    {
        public static IList<Clients>ClientOptions()
        {
            IList<Clients> obj = NHibernateHelper.OpenSession()
             .CreateCriteria(typeof(Clients))
              .SetProjection(Projections.ProjectionList()
              .Add(Projections.Property("Id"), "Id")
              .Add(Projections.Property("Name"), "Name")).List<IList>()
              .Select(l => new Clients() { Id = (int)l[0], Name = (string)l[1] })
              .ToList();
            return obj;          
        }
        public static int GetByAppLogin1(string EmailId)
        {
                IClients obj = NHibernateHelper
                .OpenSession()
                .CreateCriteria(typeof(IClients))
                .Add(NHibernate.Criterion.Restrictions.Eq("EmailId", EmailId))
                .SetFirstResult(0)
                .UniqueResult<IClients>();

                return obj.Id;
        }

        public static IList<IClients> GetByAppLogin(string EmailId)
        {
            IList<IClients> obj = NHibernateHelper.OpenSession()
            .CreateCriteria(typeof(IClients))
            .Add(NHibernate.Criterion.Restrictions.Eq("EmailId", EmailId))
            .List<IClients>();

            return obj;
        }

        public void AddContact(IClientContact obj, int id)
        {
            IClients bl = GetById(id);
            bl.Contacts.Add(obj);
            InsertOrUpdate(bl);
        }
    }
}

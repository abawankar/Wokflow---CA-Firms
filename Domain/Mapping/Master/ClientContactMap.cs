using Domain.Implementation.Master;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public class ClientContactMap : ClassMap<ClientContact>
    {
        public ClientContactMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.MobileNo);
            Map(x => x.Mailid);
            Map(x => x.Comments);
            Map(x => x.DIN);
            Map(x => x.PAN);
            Map(x => x.DOB); 
        }
    }
}

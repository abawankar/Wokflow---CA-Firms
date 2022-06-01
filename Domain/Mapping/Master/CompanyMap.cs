using Domain.Implementation.Master;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public sealed class CompanyMap: ClassMap<Company>
    {
        public CompanyMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.PAN);
            Map(x => x.ServiceTax);
            Map(x => x.Emailid);
            Map(x => x.PhoneNo);
            Map(x => x.CIN);
            Map(x => x.RegDate);
            Map(x => x.RegNumber);
            Map(x => x.WebSite);

    }
    }
}

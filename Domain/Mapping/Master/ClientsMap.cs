using Domain.Implementation.Master;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public sealed class ClientsMap : ClassMap<Clients>
    {
        public ClientsMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.MobileNo);
            Map(x => x.EmailId);
            Map(x => x.Address);
            Map(x => x.PAN);
            Map(x => x.CIN);
            Map(x => x.DateOfIncorpration);
            Map(x => x.TAN);
            Map(x => x.ServiceTaxNumber);
            Map(x => x.TIN);
            Map(x => x.BankName);
            Map(x => x.BankAccountNumber);
            Map(x => x.AccountType);
            Map(x => x.IFSCCode);
            HasMany<ClientContact>(x => x.Contacts).Cascade.All().LazyLoad();
        }
    }
}

using Domain.Implementation.Master;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public sealed class ConsultantMap : ClassMap<Consultant>
    {
        public ConsultantMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(X => X.Name);
            Map(X => X.Address);
            Map(X => X.MobileNo);
            Map(X => X.MailId);
            Map(x=>x.PAN);
            Map(x=>x.BankName);
            Map(x=>x.BankAccountNumber );
            Map(x => x.AccountType);
            Map(x=>x.IFSCCode );
            HasManyToMany<Clients>(x => x.Clients).Table("ClientToConsultant").ParentKeyColumn("ConsultId").ChildKeyColumn("ClientId").LazyLoad();

        }
    }
}

using Domain.Implementation.Master;
using Domain.Implementation.Transaction;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Transaction
{
    public sealed class InvoiceMap : ClassMap<Invoice>
    {
        public InvoiceMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.InvoiceNo);
            Map(x => x.SerTax).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.TDS).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Status);
            Map(x => x.Received);
            References<Clients>(x => x.Client).Column("ClientId").ForeignKey("fk_invoice_client").LazyLoad();
            HasMany<InvoiceTrn>(x => x.InvTrn).KeyColumn("InvId").Cascade.All();
            References<Company>(x => x.Company).Column("CompId").ForeignKey("fk_invoice_company").LazyLoad();
        }
    }
}

using Domain.Implementation.Transaction;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Transaction
{
    public sealed class InvoiceTrnMap : ClassMap<InvoiceTrn>
    {
        public InvoiceTrnMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Particulars);
            Map(x => x.Amount).CustomSqlType("numeric(12,2)").Not.Nullable();
            References<NBO>(x => x.Task).Column("TaskId").ForeignKey("fk_invtrn_task").LazyLoad();
        }
    }
}

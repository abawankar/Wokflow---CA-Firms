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
    public sealed class NBOMap: ClassMap<NBO>
    {
        public NBOMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FileNumber);
            Map(x => x.FY);
            Map(x => x.Received);
            Map(x => x.DueDate);
            Map(x => x.Completed);
            Map(x => x.Cost).CustomSqlType("numeric(12,2)").Not.Nullable();
            Map(x => x.Status);

            Map(x => x.PaymentStatus);
            Map(x => x.FilingStatus);
            Map(x => x.BillStatus);
            Map(x => x.Comments);
            Map(x => x.OfficeFileNo);

            References<TaskTypes>(x => x.TaskTypes).Column("TaskType").ForeignKey("fk_nbo_tasktype").LazyLoad();
            References<Clients>(x => x.Client).Column("ClientId").ForeignKey("fk_nbo_client").LazyLoad();

            HasMany<NboDocument>(x => x.DocumentList).Cascade.All().LazyLoad();

        }
    }
}

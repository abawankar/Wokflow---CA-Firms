using Domain.Implementation.Transaction;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Transaction
{
    public class NboDocumentMap : ClassMap<NboDocument>
    {
        public NboDocumentMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FileName);
            Map(x => x.FileOnServer);
            Map(x => x.FileSize);
        }
    }
}

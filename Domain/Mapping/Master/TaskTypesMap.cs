using Domain.Implementation.Master;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public sealed class TaskTypesMap : ClassMap<TaskTypes>
    {
        public TaskTypesMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Particulars);
        }
    }
}

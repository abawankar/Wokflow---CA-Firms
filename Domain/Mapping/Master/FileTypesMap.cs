using Domain.Implementation.Master;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public sealed class FileTypesMap : ClassMap<FileTypes>
    {
        public FileTypesMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
        }
    }
}

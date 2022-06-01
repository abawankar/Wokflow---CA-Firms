using Domain.Implementation.Transaction;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Transaction
{
    public sealed class TaskCommentsMap: ClassMap<TaskComments>
    {
        public TaskCommentsMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.Comments);
            Map(x => x.UserName);
        }
    }
}

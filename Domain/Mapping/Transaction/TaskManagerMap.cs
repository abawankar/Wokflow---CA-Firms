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
    public sealed class TaskManagerMap : ClassMap<TaskManager>
    {
        public TaskManagerMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.ContactType).CustomSqlType("nvarchar(2)");
            Map(x => x.Date);
            Map(x => x.Task);
            Map(x => x.Notes);
            Map(x => x.Status);
            Map(x => x.Start);
            Map(x => x.Due);
            Map(x => x.Compl);
            Map(x => x.StatusPercentage);

            References<Consultant>(x => x.Consultant).Column("ConsltId").ForeignKey("fk_task_conslt").LazyLoad();
            References<NBO>(x => x.FileNumber).Column("NboId").ForeignKey("fk_task_nbo").LazyLoad();
            References<Employee>(x => x.Assigneer).Column("EmpId").ForeignKey("fk_task_employee").LazyLoad();
            HasManyToMany<Employee>(x => x.Contacts).ParentKeyColumn("TaskId").ChildKeyColumn("EmpId").LazyLoad();

            HasMany<TaskComments>(x => x.Comments).Cascade.All().LazyLoad();

        }
    }
}

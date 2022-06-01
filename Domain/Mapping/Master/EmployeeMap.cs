using Domain.Implementation.Master;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping.Master
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Address);
            Map(x => x.PAN);
            Map(x => x.DOB);
            Map(x => x.Emailid);
            Map(x => x.MobileNo);
            Map(x => x.Status);

            References<Company>(x => x.Company).Column("CompanyId").ForeignKey("fk_employee_company").LazyLoad();
            References<Positions>(x => x.Position).Column("PositionId").ForeignKey("fk_position_company").LazyLoad();
            HasManyToMany<EmpRights>(x => x.EmpRight).ParentKeyColumn("EmpId").ChildKeyColumn("RightsId").LazyLoad();
            HasManyToMany<Clients>(x => x.Client).Table("ClientToEmployee").ParentKeyColumn("EmpId").ChildKeyColumn("ClientId").LazyLoad();
        }
    }
}

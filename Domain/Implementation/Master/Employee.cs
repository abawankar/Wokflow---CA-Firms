using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Master
{
    public class Employee : IEmployee
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual ICompany Company { get; set; }
        public virtual IPositions Position { get; set; }
        public virtual string PAN { get; set; }
        public virtual DateTime? DOB { get; set; }
        public virtual string Emailid { get; set; }
        public virtual string MobileNo { get; set; }
        public virtual IList<IEmpRights> EmpRight { get; set; }
        public virtual IList<IClients> Client { get; set; }
        public virtual int Status { get; set; }
    }
}

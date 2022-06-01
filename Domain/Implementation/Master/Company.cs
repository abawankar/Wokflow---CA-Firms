using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Master
{
    public class Company : ICompany
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string PAN { get; set; }
        public virtual string ServiceTax { get; set; }
        public virtual string Emailid { get; set; }
        public virtual string PhoneNo { get; set; }
        public virtual string CIN { get; set; }
        public virtual DateTime? RegDate { get; set; }
        public virtual string RegNumber { get; set; }
        public virtual string WebSite { get; set; }
    }
}

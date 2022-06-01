using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Master
{
    public class Clients : IClients
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string MobileNo { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string Address { get; set; }
        public virtual string PAN { get; set; }
        public virtual string CIN { get; set; }
        public virtual DateTime? DateOfIncorpration { get; set; }
        public virtual string TAN { get; set; }
        public virtual string ServiceTaxNumber { get; set; }
        public virtual string TIN { get; set; }
        public virtual string BankName { get; set; }
        public virtual int BankAccountNumber { get; set; }
        public virtual string AccountType { get; set; }
        public virtual string IFSCCode { get; set; }
        public virtual IList<IClientContact> Contacts { get; set; }
    }
}

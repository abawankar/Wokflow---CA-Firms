using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Master
{
   public class Consultant : IConsultant
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string MobileNo { get; set; }
        public virtual string MailId { get; set; }
        public virtual string PAN { get; set; }
        public virtual string BankName { get; set; }
        public virtual string BankAccountNumber { get; set; }
        public virtual string AccountType { get; set; }
        public virtual string IFSCCode { get; set; }
        public virtual IList<IClients> Clients { get; set; }
    }
}

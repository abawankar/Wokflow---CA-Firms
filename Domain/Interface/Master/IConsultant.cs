using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Master
{
    public interface IConsultant
    {
        int Id { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        string MobileNo { get; set; }
        string MailId { get; set; }
        string PAN { get; set; }
        string BankName { get; set; }
        string BankAccountNumber { get; set; }
        string AccountType { get; set; }
        string IFSCCode { get; set; }
        IList<IClients> Clients { get; set; }
    }
}

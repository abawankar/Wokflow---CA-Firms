using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Master
{
    public interface IClients
    {
        int Id { get; set; }
        string Name { get; set; }
        string MobileNo { get; set; }
        string EmailId { get; set; }
        string Address { get; set; }
        string PAN { get; set; }
        string CIN { get; set; }
        DateTime? DateOfIncorpration { get; set; }
        string TAN { get; set; }
        string ServiceTaxNumber { get; set; }
        string TIN { get; set; }
        string BankName { get; set; }
        int BankAccountNumber { get; set; }
        string AccountType { get; set; }
        string IFSCCode { get; set; }
        IList<IClientContact> Contacts { get; set; }
    }
}

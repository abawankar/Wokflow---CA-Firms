using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IInvoice
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        string InvoiceNo { get; set; }
        double SerTax { get; set; }
        double TDS { get; set; }
        IList<IInvoiceTrn> InvTrn { get; set; }
        IClients Client { get; set; }
        int Status { get; set; }
        DateTime? Received { get; set; }
        ICompany Company { get; set; }
    }
}

using Domain.Interface.Master;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
   public class Invoice : IInvoice
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual double SerTax { get; set; }
        public virtual double TDS { get; set; }
        public virtual IList<IInvoiceTrn> InvTrn { get; set; }
        public virtual IClients Client { get; set; }
        public virtual int Status { get; set; }
        public virtual DateTime? Received { get; set; }
        public virtual ICompany Company { get; set; }
    }
}

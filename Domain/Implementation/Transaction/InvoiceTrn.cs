using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class InvoiceTrn : IInvoiceTrn
    {
        public virtual int Id { get; set; }
        public virtual INBO Task { get; set; }
        public virtual string Particulars { get; set; }
        public virtual double? Amount { get; set; }
    }
}

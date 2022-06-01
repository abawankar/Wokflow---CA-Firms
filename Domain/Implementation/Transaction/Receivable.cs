using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class Receivable : IReceivable
    {
        public virtual int Id { get; set; }
        public virtual DateTime? DueDate { get; set; }
        public virtual double Amount { get; set; }
        public virtual string DepositType { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime? DateReceived { get; set; }
        public virtual double AmountReceived { get; set; }
        public virtual double? Balance
        {
            get
            {
                return Amount - AmountReceived;
            }
        }
        public virtual string PaymentMode { get; set; }
        public virtual int Status { get; set; }
        public virtual INBO NBO { get; set; }
    }
}

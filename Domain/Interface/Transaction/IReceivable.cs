using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IReceivable
    {
        int Id { get; set; }
        DateTime? DueDate { get; set; }
        double Amount { get; set; }
        string DepositType { get; set; }
        string Description { get; set; }
        DateTime? DateReceived { get; set; }
        double AmountReceived { get; set; }
        string PaymentMode { get; set; }
        int Status { get; set; }
        INBO NBO { get; set; }
    }
}

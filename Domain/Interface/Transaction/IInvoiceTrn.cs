using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface IInvoiceTrn
    {
        int Id { get; set; }
        INBO Task { get; set; }
        string Particulars { get; set; }
        double? Amount { get; set; }
    }
}

using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface INBO
    {
        int Id { get; set; }
        int FileNumber { get; set; }
        ITaskTypes TaskTypes { get; set; }
        IClients Client { get; set; }
        string FY { get; set; }
        DateTime? Received { get; set; }
        DateTime? DueDate { get; set; }
        DateTime? Completed { get; set; }
        double Cost { get; set; }
        int Status { get; set; }
        int FilingStatus { get; set; }
        int PaymentStatus { get; set; }
        int BillStatus { get; set; }
        string Comments { get; set; }
        string OfficeFileNo { get; set; }
        IList<INboDocument> DocumentList { get; set; }
    }
}

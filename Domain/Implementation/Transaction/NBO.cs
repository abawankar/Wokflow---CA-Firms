using Domain.Interface.Master;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class NBO : INBO
    {
        public virtual int Id { get; set; }
        public virtual int FileNumber { get; set; }
        public virtual ITaskTypes TaskTypes { get; set; }
        public virtual IClients Client { get; set; }
        public virtual string FY { get; set; }
        public virtual DateTime? Received { get; set; }
        public virtual DateTime? DueDate { get; set; }
        public virtual DateTime? Completed { get; set; }
        public virtual double Cost { get; set; }
        public virtual int Status { get; set; }
        public virtual int FilingStatus { get; set; }
        public virtual int PaymentStatus { get; set; }
        public virtual int BillStatus { get; set; }
        public virtual string Comments { get; set; }
        public virtual string OfficeFileNo { get; set; }
        public virtual IList<INboDocument> DocumentList { get; set; }
    }
}

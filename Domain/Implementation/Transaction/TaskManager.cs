using Domain.Interface.Master;
using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class TaskManager : ITaskManager
    {
        public virtual int Id { get; set; }
        public virtual string ContactType { get; set; }
        public virtual IConsultant Consultant { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Task { get; set; }
        public virtual IEmployee Assigneer { get; set; }
        public virtual string Notes { get; set; }
        public virtual int Status { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime? Due { get; set; }
        public virtual DateTime? Compl { get; set; }
        public virtual int TaskRepeat { get; set; }
        public virtual IList<IEmployee> Contacts { get; set; }
        public virtual INBO FileNumber { get; set; }
        public virtual int StatusPercentage { get; set; }
        public virtual IList<ITaskComments> Comments { get; set; }
    }
}

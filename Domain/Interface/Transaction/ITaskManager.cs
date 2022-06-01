using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface ITaskManager
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        string ContactType { get; set; }
        IConsultant Consultant { get; set; }
        string Task { get; set; }
        IEmployee Assigneer { get; set; }
        string Notes { get; set; }
        int Status { get; set; }
        DateTime Start { get; set; }
        DateTime? Due { get; set; }
        DateTime? Compl { get; set; }
        IList<IEmployee> Contacts { get; set; }
        INBO FileNumber { get; set; }
        int StatusPercentage { get; set; }
        IList<ITaskComments> Comments { get; set; }
    }
}

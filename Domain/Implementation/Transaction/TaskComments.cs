using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class TaskComments : ITaskComments
    {
        public virtual int Id { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual string Comments { get; set; }
        public virtual string UserName { get; set; }
    }
}

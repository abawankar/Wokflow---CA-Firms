using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface ITaskComments
    {
        int Id { get; set; }
        DateTime? Date { get; set; }
        string Comments { get; set; }
        string UserName { get; set; }
    }
}

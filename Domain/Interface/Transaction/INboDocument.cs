using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Transaction
{
    public interface INboDocument
    {
        int Id { get; set; }
        string FileName { get; set; }
        string FileOnServer { get; set; }
        string FileSize { get; set; }
    }
}

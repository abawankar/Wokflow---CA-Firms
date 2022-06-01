using Domain.Interface.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Transaction
{
    public class NboDocument: INboDocument
    {
        public virtual int Id { get; set; }
        public virtual string FileName { get; set; }
        public virtual string FileOnServer { get; set; }
        public virtual string FileSize { get; set; }
    }
}

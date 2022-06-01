using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Master
{
    public class ClientContact :IClientContact
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string MobileNo { get; set; }
        public virtual string Mailid { get; set; }
        public virtual string Comments { get; set; }
        public virtual string DIN { get; set; }
        public virtual string PAN { get; set; }
        public virtual DateTime? DOB { get; set; }
    }
}

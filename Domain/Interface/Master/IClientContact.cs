using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Master
{
    public interface IClientContact
    {
        int Id { get; set; }
        string Name { get; set; }
        string MobileNo { get; set; }
        string Mailid { get; set; }
        string Comments { get; set; }
        string DIN { get; set; }
        string PAN { get; set; }
        DateTime? DOB { get; set; }
    }
}

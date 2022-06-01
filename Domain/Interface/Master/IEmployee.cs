using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Master
{
    public interface IEmployee
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        ICompany Company { get; set; }
        IPositions Position { get; set; }
        string PAN { get; set; }
        DateTime? DOB { get; set; }
        string Emailid { get; set; }
        string MobileNo { get; set; }
        IList<IEmpRights> EmpRight { get; set; }
        IList<IClients> Client { get; set; }
        int Status { get; set; }
    }
}

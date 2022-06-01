using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserInterface.Models.Master
{
    public class ClientToEmployee
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string EmpName { get; set; }
        public int EmpId { get; set; }
    }
}
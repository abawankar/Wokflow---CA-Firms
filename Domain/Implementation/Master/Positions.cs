using Domain.Interface.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation.Master
{
    public class Positions:IPositions
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }
}

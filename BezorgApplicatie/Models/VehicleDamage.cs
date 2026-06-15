using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class VehicleDamage
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int VehicleId { get; set; }
        public string Location { get; set; } 

    }
}

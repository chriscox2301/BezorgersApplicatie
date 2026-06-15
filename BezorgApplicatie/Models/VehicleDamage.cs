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
        public Vehicle Vehicle { get; set; }
        public Shift Shift { get; set; }
        public string Location { get; set; } 

    }
}

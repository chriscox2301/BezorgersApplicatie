using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string VehicleZone { get; set; }
        public IEnumerable<Package> Packages { get; set; }
    }
}

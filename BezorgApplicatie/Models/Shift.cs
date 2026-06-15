using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
        public IEnumerable<Order> Orders { get; set; }

        public int WarehouseId { get; set; }
        public int LoadId { get; set; }
    }
}

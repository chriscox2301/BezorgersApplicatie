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
        public Driver Driver { get; set; }
        public Vehicle Vehicle { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public Warehouse Warehouse { get; set; }
        public Load Load { get; set; }
    }
}

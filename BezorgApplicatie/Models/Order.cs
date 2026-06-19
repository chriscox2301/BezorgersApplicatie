using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<Package> Packages { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string? Location { get; set; }
    }
}

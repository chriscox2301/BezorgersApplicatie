using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class Package
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public string Barcode { get; set; }
        public int OrderId { get; set; }
        public int CartId { get; set; }
    }
}

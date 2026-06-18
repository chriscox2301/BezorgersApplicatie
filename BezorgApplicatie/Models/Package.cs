using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class Package
    {
        public int Id { get; set; }
        [MinLength(3), MaxLength(3)]
        public int Number { get; set; }
        public double Weight { get; set; }
        public string Barcode { get; set; }
        public string? Status { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}

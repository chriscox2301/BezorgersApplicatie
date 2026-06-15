using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public IEnumerable<Load> Loads { get; set; }
    }
}

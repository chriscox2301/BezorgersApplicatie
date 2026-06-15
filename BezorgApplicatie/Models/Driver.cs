using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Shift> Shifts { get; set; }
    }
}

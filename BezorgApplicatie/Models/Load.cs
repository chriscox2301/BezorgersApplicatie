using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class Load
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int ShiftId { get; set; }
        public Shift Shift { get; set; }
        public IEnumerable<Cart> Carts { get; set; }
    }
}

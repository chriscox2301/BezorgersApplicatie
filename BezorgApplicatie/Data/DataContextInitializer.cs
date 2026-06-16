using BezorgApplicatie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Data
{
    public class DataContextInitializer
    {
        public static void Initialize(DataContext context)
        {
            //Zorgt ervoor dat initializer NIET gedaan wordt wanneer er al data in zit.
            if (context.Drivers.Any())
                return;

            var drivers = new Driver[]
            {
                new Driver {Name = "Piet"},
              
            };
            context.Drivers.AddRange(drivers);

            var vehicles = new Vehicle[] {
                new Vehicle {Id = 1, LicensePlate = "AA-392-B" },
                new Vehicle {Id = 2, LicensePlate = "BB-333-C" },
                new Vehicle {Id = 3, LicensePlate = "DD-322-D" },
                new Vehicle {Id = 4, LicensePlate = "VD-421-E" }
            };
            context.Vehicles.AddRange(vehicles);

            var warehouses = new Warehouse[] {
                new Warehouse {Location = "Maastricht" }, 
                new Warehouse {Location = "Sittard" }
            };

            context.Warehouses.AddRange(warehouses); 
            var loads = new Load[]
                {
                   new Load { Warehouse = warehouses[0] }  
                };

            context.Loads.AddRange(loads);

        


            var shift = new Shift[]
            {
                new Shift {StartTime = DateTime.Parse("2026-08-16 9:30:00"), EndTime = DateTime.Parse("2026-08-16 16:30:00"), Driver = drivers[0], Vehicle = vehicles[1],  Warehouse = warehouses[1],  Load = loads[0] },
                new Shift {StartTime = DateTime.Parse("2026-08-18 9:30:00"), EndTime = DateTime.Parse("2026-08-18 16:30:00"), Driver = drivers[0], Vehicle = vehicles[3], Warehouse = warehouses[0],  Load = loads[0] }
            };
            context.Shifts.AddRange(shift);

            var orders = new Order[]
            {
                 new Order {Date = DateTime.Now,  Status = "Nieuw", Address = "Maastricht", Shift = shift[0]  }
            };

            context.Orders.AddRange(orders);

            context.Database.EnsureCreated();

            context.SaveChanges(); 


        }
    }
}

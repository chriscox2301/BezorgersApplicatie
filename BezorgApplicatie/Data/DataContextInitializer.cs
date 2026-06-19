using BezorgApplicatie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BezorgApplicatie.Data
{
    public class DataContextInitializer
    {
        public static void Initialize(DataContext context)
        {   
            if (context == null)
            {
                context.Database.Migrate();
            }

            context.Database.EnsureCreated();

            var vehicles = new Vehicle[]
            {
                new Vehicle {LicensePlate = "TL-123-R"},
                new Vehicle {LicensePlate = "PB-234-Z"}
            };
            context.Vehicles.AddRange(vehicles);

            var drivers = new Driver[]
            {
                new Driver {Name = "Piet"},
              
            };
            context.Drivers.AddRange(drivers);

            var warehouse = new Warehouse { Location = "Amsterdam" };
            context.Warehouses.Add(warehouse);
            var cart = new Cart { Warehouse = warehouse, VehicleZone = "A1" };
            context.Carts.Add(cart);

            var loads = new Load[]
            {
                new Load { Warehouse = warehouse}
            };
            context.Loads.AddRange(loads);

            var orders = new Order[]
            {
                new Order { Date = DateTime.Now, Status = "Onderweg", Address = "Damrak 1, Amsterdam" },
                new Order { Date = DateTime.Now, Status = "Onderweg", Address = "Kalverstraat 5, Amsterdam" },
                new Order { Date = DateTime.Now, Status = "Onderweg", Address = "Nieuwendijk 10, Amsterdam" }
            };
            context.Orders.AddRange(orders);

            var packages = new Package[]
            {
                new Package { Weight = 1.5, Barcode = "PKG001", Order = orders[0], Cart = cart, HasIssue = false },
                new Package { Weight = 2.3, Barcode = "PKG002", Order = orders[1], Cart = cart, HasIssue = false },
                new Package { Weight = 0.8, Barcode = "PKG003", Order = orders[2], Cart = cart, HasIssue = false }
            };
            context.Packages.AddRange(packages);

            var shift = new Shift[]
            {
                new Shift { Orders = orders, Driver = drivers[0], StartTime = DateTime.Parse("2026-06-25 08:00:00"), EndTime = DateTime.Parse("2026-06-25 16:00:00"), Warehouse = warehouse, Vehicle = vehicles[0], Load = loads[0]}
            };
            context.Shifts.AddRange(shift);

            context.SaveChanges();

            context.Database.EnsureCreated();

            context.SaveChanges(); 


        }
    }
}

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
                new Driver {Name = "Henk"}
            };
            context.Drivers.AddRange(drivers);

            var warehouse = new Warehouse { Location = "Amsterdam" };
            context.Warehouses.Add(warehouse);
            var cart = new Cart { Warehouse = warehouse, VehicleZone = "A1" };
            context.Carts.Add(cart);

            var orders = new Order[]
            {
                new Order { Date = DateTime.Now, Status = "Open", Address = "Damrak 1, Amsterdam" },
                new Order { Date = DateTime.Now, Status = "Open", Address = "Kalverstraat 5, Amsterdam" },
                new Order { Date = DateTime.Now, Status = "Open", Address = "Nieuwendijk 10, Amsterdam" }
            };
            context.Orders.AddRange(orders);

            var packages = new Package[]
            {
                new Package { Weight = 1.5, Barcode = "PKG001", Order = orders[0], Cart = cart, HasIssue = false },
                new Package { Weight = 2.3, Barcode = "PKG002", Order = orders[1], Cart = cart, HasIssue = false },
                new Package { Weight = 0.8, Barcode = "PKG003", Order = orders[2], Cart = cart, HasIssue = false }
            };
            context.SaveChanges();

            var cart = new Cart { WarehouseId = warehouse.Id, VehicleZone = "Zone A" };
            context.Carts.Add(cart);
            context.SaveChanges();

            var orders = new Order[]
            {
                new Order
                {
                    Date = DateTime.Now,
                    Status = "Pending",
                    Address = "123 Main Street, Amsterdam",
                    Location = "A"
                },
                new Order
                {
                    Date = DateTime.Now.AddDays(1),
                    Status = "Pending",
                    Address = "456 Oak Avenue, Rotterdam",
                    Location = "B"
                }
            };
            context.Orders.AddRange(orders);
            context.SaveChanges();

            var packages = new Package[]
            {
                new Package { Weight = 2.5, Barcode = "PKG001", OrderId = orders[0].Id, CartId = cart.Id, Number = 012 },
                new Package { Weight = 1.8, Barcode = "PKG002", OrderId = orders[0].Id, CartId = cart.Id, Number = 013 },
                new Package { Weight = 3.2, Barcode = "PKG003", OrderId = orders[0].Id, CartId = cart.Id, Number = 014 },
                new Package { Weight = 1.5, Barcode = "PKG004", OrderId = orders[1].Id, CartId = cart.Id, Number = 015 },
                new Package { Weight = 2.1, Barcode = "PKG005", OrderId = orders[1].Id, CartId = cart.Id, Number = 016 },
                new Package { Weight = 2.7, Barcode = "PKG006", OrderId = orders[1].Id, CartId = cart.Id, Number = 017 }
            };
            context.Packages.AddRange(packages);
            context.SaveChanges();

            context.Database.EnsureCreated();
        }
    }
}

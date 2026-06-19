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

            // Add test data if database is empty
            if (!context.Packages.Any())
            {
                // First create a warehouse
                var warehouse = new Warehouse 
                { 
                    Location = "Test Warehouse"
                };
                context.Warehouses.Add(warehouse);
                context.SaveChanges();

                // Then create a cart
                var cart = new Cart
                {
                    WarehouseId = warehouse.Id,
                    VehicleZone = "D"
                };
                context.Carts.Add(cart);
                context.SaveChanges();

                // Then create an order
                var order = new Order
                {
                    Date = DateTime.Now,
                    Status = "In Transit",
                    Address = "Test Address"
                };
                context.Orders.Add(order);
                context.SaveChanges();

                // Finally create packages with valid foreign keys
                var testPackages = new List<Package>
                {
                    new Package { Barcode = "PKG001", Weight = 2.5, OrderId = order.Id, CartId = cart.Id },
                    new Package { Barcode = "PKG002", Weight = 1.2, OrderId = order.Id, CartId = cart.Id },
                    new Package { Barcode = "PKG003", Weight = 3.8, OrderId = order.Id, CartId = cart.Id },
                    new Package { Barcode = "PKG004", Weight = 0.9, OrderId = order.Id, CartId = cart.Id },
                    new Package { Barcode = "PKG005", Weight = 4.2, OrderId = order.Id, CartId = cart.Id }
                };

                context.Packages.AddRange(testPackages);
                context.SaveChanges();
            }
        }
    }
}

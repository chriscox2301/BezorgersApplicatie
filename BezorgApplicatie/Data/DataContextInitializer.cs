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

            var orders = new Order[]
            {
                new Order
                {
                    Date = DateTime.Parse("2026-06-16"),
                    Address = "Stationsstraat 10",
                    Status = "Onderweg",
                    Packages = new List<Package>
                    {
                        new Package
                        {
                            Barcode = "PKG001"

                        },
                        new Package
                        {
                            Barcode = "PKG002"
                        }
                    }
                },
                new Order
                {
                    Date = DateTime.Parse("2026-06-16"),
                    Address = "Dorpsplein 25",
                    Status = "Onderweg",
                    Packages = new List<Package>
                    {
                        new Package
                        {
                            Barcode = "PKG003"
                        }
                    }
                },
                new Order
                {
                    Date = DateTime.Parse("2026-06-16"),
                    Address = "Kerklaan 7",
                    Status = "Onderweg",
                    Packages = new List<Package>
                    {
                        new Package
                        {
                            Barcode = "PKG004"
                        }
                    }
                }
            };

            context.Orders.AddRange(orders);

            var shifts = new Shift[]
            {
                new Shift
                {
                    StartTime = DateTime.Parse("2026-06-16 08:00:00"),
                    EndTime = DateTime.Parse("2026-06-16 16:00:00"),
                    Driver = drivers[0],
                    Vehicle = new Vehicle
                    {
                        LicensePlate = "TL-123-R"
                    },
                    Orders = orders.ToList()
                }
            };

            context.Shifts.AddRange(shifts);


            context.SaveChanges();

            context.Database.EnsureCreated();
        }
    }
}

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
         
            var drivers = new Driver[]
            {
                new Driver {Name = "Piet"},
                new Driver {Name = "Henk"}
            };
            context.Drivers.AddRange(drivers);

            context.SaveChanges();

            context.Database.EnsureCreated();
        }
    }
}

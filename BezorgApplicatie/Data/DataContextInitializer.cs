using BezorgApplicatie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

            context.SaveChanges();

            context.Database.EnsureCreated();
        }
    }
}

using BezorgApplicatie.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Data
{
    public class DataContext : DbContext
    {
        public DbSet<VehicleDamage> VehicleDamages { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Load> Loads { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageIssue> PackageIssues { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}

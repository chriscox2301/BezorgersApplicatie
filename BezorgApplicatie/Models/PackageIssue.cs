using Android.App.AppSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class PackageIssue
    {
        public  int id  { get; set; }
        public string description { get; set; }
        public DateTime Date { get; set; }
        public int PackageId { get; set; }
    }
}

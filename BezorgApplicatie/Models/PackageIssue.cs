using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezorgApplicatie.Models
{
    public class PackageIssue
    {
        public  int Id  { get; set; }
        public string IssueType { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
    }
}

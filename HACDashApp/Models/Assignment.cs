using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSWebApiEmpty.Models
{
    public class Assignment
    {
        public string Description { get; set; }
        public DateTime DateDue { get; set; }
        public string Course { get; set; }
        public string Category { get; set; }
        public string Points { get; set; }
        public string Score { get; set; }   // "L" Is bad, loser.
    }
}
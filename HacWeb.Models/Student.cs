using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacWeb.Models
{
    public class Student
    {
        public string DisplayName { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<Course> Courses { get; set; }
    }       

    public class Students : List<Student>
    {

    }
}
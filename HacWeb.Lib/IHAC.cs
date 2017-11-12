using HacWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HacWeb.Lib
{
    public interface IHAC
    {
        Students GetStudents();

        List<Course> GetCurrentCourseStatus(string studentId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;


namespace RRISD_HAC_Access
{
    class Program
    {
        static void Main(string[] args)
        {
            //HACSMSGateway
            HAC hac = new HAC();
            CookieContainer container;
            HttpWebResponse response = hac.login("kobrien", "omit", out container);
            List<Student> students = hac.getStudents(container, response.ResponseUri);
            //Tuple<String,String> emailInfo = new Tuple<String,String>("HACSMSGateway@gmail.com","omit");
            hac.changeStudent("124936", container, response.ResponseUri);
            List<Assignment> assignments = hac.getAssignments(container, response.ResponseUri);
            Dictionary<String, List<Assignment>> courses = AssignmentUtils.organizeAssignments(assignments);
            SMS sms = new SMS();
            foreach(KeyValuePair<String,List<Assignment>> course in courses) {
                Console.WriteLine(course.Key);
                foreach(Assignment assignment in course.Value) {
                    Console.WriteLine("\t" + assignment);
                }
            }
            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}

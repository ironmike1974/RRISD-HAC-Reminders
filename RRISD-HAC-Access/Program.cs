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
            HttpWebResponse response = hac.login("omit", "omit", out container);
            List<Student> students = hac.getStudents(container, response.ResponseUri);
            Tuple<String, String> emailInfo = new Tuple<String, String>("omit", "omit");
            hac.changeStudent("omit", container, response.ResponseUri);
            List<Assignment> assignments = hac.getAssignments(container, response.ResponseUri);
            Dictionary<String, List<Assignment>> courses = AssignmentUtils.organizeAssignments(assignments);
            foreach (KeyValuePair<String,List<Assignment>> pair in courses)
            {
                Console.WriteLine(pair.Key+"\n");
                double avg = 0;
                foreach (Assignment assignment in pair.Value)
                {
                    avg = assignment.courseAverage; //eww
                    Console.WriteLine(assignment);
                }
                Console.WriteLine("CLASS AVERAGE: " + avg);
                Console.WriteLine();
            }
            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}

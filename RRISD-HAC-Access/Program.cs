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
            HttpWebResponse response = hac.login("omitted", "omitted", out container);
            List<Student> students = hac.getStudents(container, response.ResponseUri);
            Tuple<String, String> emailInfo = new Tuple<String, String>("omitted", "omitted");
            foreach (Student student in students)
            {
                hac.changeStudent(student.id, container, response.ResponseUri);
                List<Assignment> assignments = hac.getAssignments(container, response.ResponseUri);
                SMS sms = new SMS();
                foreach (Assignment assignment in assignments)
                {
                    if ((assignment.points!=-1)&& (assignment.points < 100))
                    {
                        sms.sendSMS("omitted",student.name+" got a " + assignment.points + " on his assignment " + assignment.classwork + " which was due on " + assignment.dueDate + " and " + (assignment.canBeDropped ? "can" : "cannot") + " be dropped!", SMSCarrier.ATT,credentials);
                    }
                }
            }
            Console.ReadKey();
        }
    }
}

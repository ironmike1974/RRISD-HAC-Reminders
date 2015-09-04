using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RRISD_HAC_Access
{
    class Program
    {
        static void Main(string[] args)
        {
            HAC hac = new HAC();
            CookieContainer container;
            HttpWebResponse response = hac.login("ommited :)", "omitted :)", out container);
            List<Assignment> assignments = hac.getAssignments(container, response.ResponseUri);
            foreach (Assignment assignment in assignments)
            {
                Console.WriteLine(assignment.classwork);
            }
            Console.ReadKey();
        }
    }
}

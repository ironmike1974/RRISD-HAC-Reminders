using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRISD_HAC_Access
{
    class AssignmentUtils
    {
        public static Dictionary<String, List<Assignment>> organizeAssignments(List<Assignment> assignments)
        {
            Dictionary<String, List<Assignment>> ret = new Dictionary<String, List<Assignment>>();
            foreach (Assignment assignment in assignments)
            {
                if (ret.ContainsKey(assignment.course))
                    ret[assignment.course].Add(assignment);
                else
                {
                    ret[assignment.course] = new List<Assignment>();
                    ret[assignment.course].Add(assignment);
                }
            }
            return ret;
        }
        public static double getListAverage(List<Assignment> assignments)
        {
            double a = 0;
            int b = 0;
            foreach (Assignment assignment in assignments)
            {
                if (assignment.points != -1)
                {
                    a += assignment.points;
                    b++;
                }
            }
            return (a / (double)b) * 100;
        }
        public static List<Assignment> getUpcomingAssignments(List<Assignment> assignments, TimeSpan timeSpan, bool includeAlreadyGradedAssignments)
        {
            DateTime now = DateTime.Now;
            List<Assignment> ret = new List<Assignment>();
            foreach (Assignment assignment in assignments)
            {
                TimeSpan x = assignment.dueDate.Subtract(now);
                if ((x < timeSpan) && (includeAlreadyGradedAssignments || assignment.points != -1))
                {
                    ret.Add(assignment);
                }
            }
            return ret;
        }
        public static List<Assignment> getGradesAboveValue(List<Assignment> assignments, double value)
        {
            List<Assignment> ret = new List<Assignment>();
            foreach (Assignment assignment in assignments)
            {
                if ((assignment.points > value) && (assignment.points != -1)) //second part kind of irrelevant
                {
                    ret.Add(assignment);
                }
            }
            return ret;
        }
        public static List<Assignment> getGradesBelowValue(List<Assignment> assignments, double value)
        {
            List<Assignment> ret = new List<Assignment>();
            foreach (Assignment assignment in assignments)
            {
                if ((assignment.points < value) && (assignment.points != -1))
                {
                    ret.Add(assignment);
                }
            }
            return ret;
        }
        public static List<Assignment> getExtraCreditOpportunities(List<Assignment> assignments, TimeSpan timeSpan, bool includeAlreadyGradedAssignments)
        {
            DateTime now = DateTime.Now;
            List<Assignment> ret = new List<Assignment>();
            foreach (Assignment assignment in assignments)
            {
                TimeSpan x = assignment.dueDate.Subtract(now);
                if ((x < timeSpan) && (includeAlreadyGradedAssignments || assignment.points != -1) && assignment.extraCredit)
                {
                    ret.Add(assignment);
                }
            }
            return ret;
        }
        public static List<Assignment> getDroppableAssignments(List<Assignment> assignments, bool includeAlreadyGradedAssignments)
        {
            List<Assignment> ret = new List<Assignment>();
            foreach (Assignment assignment in assignments)
            {
                if ((includeAlreadyGradedAssignments || assignment.points != -1) && assignment.canBeDropped)
                {
                    ret.Add(assignment);
                }
            }
            return ret;
        }
        public static List<Assignment> getGradesUnderValueAndDroppable(List<Assignment> assignments, double value)
        {
            return getDroppableAssignments(getGradesBelowValue(assignments, value), true);
        }
    }
}

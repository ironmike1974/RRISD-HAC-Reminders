# RRISD-HAC-Reminders
<h1><b>Features</b></h1>
<ul>
<li>Access <a href="https://accesscenter.roundrockisd.org/HomeAccess/Account/LogOn">Home Access Center for RRISD</a> via a simple C# library</li>
<li>Monitor grades for multiple students under one account</li>
<li>Easy organization of upcoming assignments</li>

<h1><b>Examples</b></h1>

<h3>Login and fetch all grades for the default student</h3>
```
HAC hac = new HAC();
CookieContainer container;
HttpWebResponse response = hac.login("hacUsernameHere", "hacPasswordHere", out container);  
List<Assignment> assignments = hac.getAssignments(container, response.ResponseUri);
foreach (Assignment assignment in assignments){
  Console.WriteLine(assignment);
}
```
<h3>Get all students under a HAC account</h3>
```
HAC hac = new HAC();
CookieContainer container;
HttpWebResponse response = hac.login("hacUsernameHere", "hacPasswordHere", out container);  
List<Student> students = hac.getStudents(container, response.ResponseUri);
foreach(Student student in students){
  Console.WriteLine(student.name+" ("+student.id+")");
}
```
<h3>Organize all assignments into a Dictionary</h3>
```
HAC hac = new HAC();
CookieContainer container;
HttpWebResponse response = hac.login("hacUsernameHere", "hacPasswordHere", out container);  
Dictionary<String, List<Assignment>> courses = AssignmentUtils.organizeAssignments(assignments);
foreach(KeyValuePair<String,List<Assignment>> course in courses) {
  Console.WriteLine(course.Key);
  foreach(Assignment assignment in course.Value) {
    Console.WriteLine("\t" + assignment);
  }
}
```
<h3>Send an SMS to a number regarding grades below a threshold</h3>
```
HAC hac = new HAC();
CookieContainer container;
HttpWebResponse response = hac.login("hacUsernameHere", "hacPasswordHere", out container); 
Tuple<String,String> emailInfo = new Tuple<String,String>("example@gmail.com","hunter2");
List<Assignment> assignments = hac.getAssignments(container, response.ResponseUri);
SMS sms = new SMS();
foreach (Assignment assignment in assignments){
  if (((assignment.points/assignment.maxPoints)*100.00)<70){
    sms.sendSMS("5125555555","This is an alert that your assignment ("+assignment+") has a failing grade! Your current average in this class is "+assignment.courseAverage+".",SMSCarrier.ATT,emailInfo);
  }
}
```
<h3>Get grades for a non-default student</h3>
```
HAC hac = new HAC();
CookieContainer container;
HttpWebResponse response = hac.login("hacUsernameHere", "hacPasswordHere", out container);  
List<Student> students = hac.getStudents(container, response.ResponseUri);
foreach(Student student in students){
  hac.changeStudent(student.id, container, response.ResponseUri);
  List<Assignment> assignments = hac.getAssignments(container, response.ResponseUri);
  foreach (Assignment assignment in assignments){
    Console.WriteLine(assignment);
  }
}
```

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RRISD_HAC_Access
{
    class HAC
    {
        public HttpWebResponse login(String username, String password, out CookieContainer container)
        {
            container = new CookieContainer();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://accesscenter.roundrockisd.org/HomeAccess/Account/LogOn?ReturnUrl=%2fhomeaccess%2f");

                request.KeepAlive = true;
                request.Headers.Set(HttpRequestHeader.CacheControl, "max-age=0");
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Headers.Add("Origin", @"https://accesscenter.roundrockisd.org");
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = "https://accesscenter.roundrockisd.org/HomeAccess/Account/LogOn?ReturnUrl=%2fhomeaccess%2f";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
                request.CookieContainer = container;
                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                string body = @"Database=10&LogOnDetails.UserName="+username+"&LogOnDetails.Password="+password;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                return (HttpWebResponse)request.GetResponse();
            }catch
            {
                return null;
            }
        }
        public List<Assignment> getAssignments(CookieContainer cookies, Uri requestUri)
        {
            String data = getRawGradeData(cookies, requestUri);
            List<String> raw = new List<String>();
            List<Assignment> ret = new List<Assignment>();
            //past this line is absolutely horrid
            //you can thank my lack of regex knowledge for this
            int x = data.IndexOf(">", data.IndexOf("<a class=\"sg-header-heading\"")) + 1;
            int y = data.IndexOf("</a>", x);
            String course = data.Substring(x, y - x).Trim();
            data = data.Substring(y);
            while (data.IndexOf("title=\"Title") != -1)
            {
                int a = data.IndexOf("title=\"Title");
                x = data.IndexOf(">", data.IndexOf("<a class=\"sg-header-heading\"")) + 1;
                if (a < x)
                {
                    int b = data.IndexOf("\"", a + "title=".Length + 1);
                    String s = data.Substring(a + "title=".Length + 1, b - a - 7); //7 is the magic number
                    data = data.Substring(b);
                    int c = data.IndexOf("</td><td>");
                    data = data.Substring(c + 7);
                    c = data.IndexOf("</td><td>") + 10; //10 is also magic
                    int d = data.IndexOf("</td>", c + 5); //as is 5

                    String temp = data.Substring(c, d - c).Trim();
                    double points = -1;
                    if (temp.Length > 0)
                    {
                        points = double.Parse(temp);
                    }
                    //its about to get ugly
                    int e = s.IndexOf("Title:") + 6;
                    String title = s.Substring(e, s.IndexOf("\n", e) - e).Trim();
                    e = s.IndexOf("Classwork:") + 10;
                    String classwork = s.Substring(e, s.IndexOf("\n", e) - e).Trim();
                    e = s.IndexOf("Category:") + 9;
                    String category = s.Substring(e, s.IndexOf("\n", e) - e).Trim();
                    e = s.IndexOf("Due Date:") + 9;
                    DateTime date = DateTime.Parse(s.Substring(e, s.IndexOf("\n", e) - e).Trim());
                    e = s.IndexOf("Max Points:") + 11;
                    double maxPoints = double.Parse(s.Substring(e, s.IndexOf("\n", e) - e).Trim());
                    e = s.IndexOf("Can Be Dropped:") + 16;
                    bool droppable = s.Substring(e, s.IndexOf("\n", e) - e).Trim().Contains("Y");
                    e = s.IndexOf("Extra Credit:") + 14;
                    bool extraCredit = s.Substring(e, s.IndexOf("\n", e) - e).Trim().Contains("Y");
                    e = s.IndexOf("Has Attachments:") + 16;
                    bool attachments = s.Substring(e).Trim().Contains("Y");
                    //you can open your eyes now
                    ret.Add(new Assignment
                    {
                        title = title,
                        course = course,
                        classwork = classwork,
                        category = category,
                        dueDate = date,
                        maxPoints = maxPoints,
                        points = points,
                        canBeDropped = droppable,
                        extraCredit = extraCredit,
                        hasAttachment = attachments
                    });
                }
                else
                {
                    x = data.IndexOf(">", data.IndexOf("<a class=\"sg-header-heading\"")) + 1;
                    y = data.IndexOf("</a>", x);
                    course = data.Substring(x, y - x).Trim();
                    data = data.Substring(y);
                }
            }
            //end horrid part
            return ret;
        }
        private String getRawGradeData(CookieContainer cookies, Uri requestUri)
        {
            String s = String.Empty;
            foreach (Cookie cookie in cookies.GetCookies(requestUri))
            {
                s += (cookie.Name + "=" + cookie.Value + "; ");
            }
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://accesscenter.roundrockisd.org/HomeAccess/Content/Student/Assignments.aspx");

                request.KeepAlive = true;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                request.Headers.Add("Upgrade-Insecure-Requests", @"1");
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.85 Safari/537.36";
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch");
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
                request.Headers.Set(HttpRequestHeader.Cookie, s);

                return readResponse((HttpWebResponse)request.GetResponse());
            }
            catch
            {
                return null;
            }
        }
        private string readResponse(HttpWebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                Stream streamToRead = responseStream;
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    streamToRead = new GZipStream(streamToRead, CompressionMode.Decompress);
                }
                else if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    streamToRead = new DeflateStream(streamToRead, CompressionMode.Decompress);
                }

                using (StreamReader streamReader = new StreamReader(streamToRead, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
    class Assignment
    {
        public String title { get; set; }
        public String course { get; set; }
        public String classwork { get; set; }
        public String category { get; set; }
        public DateTime dueDate { get; set; }
        public double maxPoints { get; set; }
        public double points { get; set; }
        public bool canBeDropped { get; set; }
        public bool extraCredit { get; set; }
        public bool hasAttachment { get; set; }
    }
}

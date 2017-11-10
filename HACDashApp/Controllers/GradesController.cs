using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularJSWebApiEmpty.Controllers
{
    public class GradesController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetGrades(string userId, string password)
        {
            dynamic grade = new
            {
                G1 = userId,
                G2 = password
            };

            return Json(grade);
        }
    }
}

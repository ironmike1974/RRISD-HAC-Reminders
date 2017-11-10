using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace AngularJSWebApiEmpty.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Run() ///api/Test/Run
        {
            dynamic doobie = new
            {
                Test = "one",
                Hank = "no"
            };

            return Json(doobie);
        }
    }
}

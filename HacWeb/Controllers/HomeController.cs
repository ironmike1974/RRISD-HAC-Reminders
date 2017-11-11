using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HacWeb.Models;
using Microsoft.Extensions.Options;

namespace HacWeb.Controllers
{
    public class HomeController : Controller
    {
        private string loginUri;
        private Students _students;

        public HomeController(IOptions<HacInterfaceUris> uriOptions,
            IOptions<Students> studentList)
        {
            loginUri = uriOptions.Value.Login;
            _students = studentList.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [HttpGet("YourMom")]
        public IActionResult Test()
        {
            dynamic foobie = new
            {
                anger = "max",
                level = 21
            };

            return Json(foobie);
        }

    }
}

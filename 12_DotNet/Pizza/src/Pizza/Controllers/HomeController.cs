using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pizza.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        
        public IActionResult Foo()
        {

            return Content("Foo");
        }

        public IActionResult Name(NameSettings settings)
        {
            string name = "Pizzza Shop - The Best!";
            
            return Content(settings.LetterCase == "upper" ? name.ToUpper() : name.ToLower());
        }
    }

    public class NameSettings
    {
        public string LetterCase { get; set; } = "upper";
    }
}

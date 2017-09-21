using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace time_display{
    public class TimeController : Controller{

        [Route("/")]
        public IActionResult Index() {
            var dt = new Dictionary<string, object>();
            dt["Time"] = DateTime.Now.ToString("hh:mm tt");
            dt["Date"] = DateTime.Now.ToString("MMM dd, yyyy");
            ViewBag.Time = dt;
            return View();
        }
    }
}
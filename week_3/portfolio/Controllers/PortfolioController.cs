using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace portfolio{
    public class PortfolioController : Controller{

        [HttpGet]
        [Route("/")]
        public IActionResult Home() {
            return View("home");
        }

        [HttpGet]
        [Route("/contact")]
        public IActionResult Contact() {
            return View("contact");
        }

        [HttpGet]
        [Route("/projects")]
        public IActionResult Projects() {
            return View("projects");
        }

    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using woods.Factory;
using woods.Models;

namespace woods.Controllers
{
    public class HomeController : Controller
    {
        private readonly TrailFactory TrailFactory;
        public HomeController()
        {
            TrailFactory = new TrailFactory();
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Trails = TrailFactory.FindAll();
            return View("Index");
        }

        [HttpPost]
        [Route("")]
        public IActionResult Sort(string sort)
        {
            if(sort == "Length")
            {
                ViewBag.Trails = TrailFactory.FindAllSortLength();
                return View("Index");
            }
            if(sort == "Elevation")
            {
                ViewBag.Trails = TrailFactory.FindAllSortElevation();
                return View("Index");
            }
            return Redirect("Index");
        }

        [HttpGet]
        [Route("NewTrail")]
        public IActionResult NewTrail()
        {
            return View("NewTrail");
        }

        [HttpPost]
        [Route("AddTrail")]
        public IActionResult AddTrail(Trail trail)
        {
            if(ModelState.IsValid)
            {
                TrailFactory.Add(trail);
                return Redirect("/");
            }
            return View("NewTrail");
        }

        [HttpGet]
        [Route("/trails/{trailID}")]
        public IActionResult Trail(int trailID)
        {
            ViewBag.Trail = TrailFactory.FindByID(trailID);
            return View("Trail");
        }

    }
}

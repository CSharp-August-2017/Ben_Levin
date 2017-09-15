using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using dojo_league.Models;
using dojo_league.Factory;

namespace dojo_league.Controllers
{
    public class HomeController : Controller
    {
        private readonly DojFactory DojFactory;
        private readonly NinFactory NinFactory;
        public HomeController()
        {
            DojFactory = new DojFactory();
            NinFactory = new NinFactory();
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        [Route("Dojos")]
        public IActionResult AddDojo()
        {
            @ViewBag.Dojos = DojFactory.FindAll();
            return View("Dojo");
        }

        [HttpPost]
        [Route("processdojo")]
        public IActionResult ProcessDojo(Dojo dojo)
        {
            if(ModelState.IsValid)
            {
                DojFactory.Register(dojo);
                return Redirect("/Dojos");
            }
            return View("Dojo");
        }

        [HttpGet]
        [Route("/Dojo/{dojoID}")]
        public IActionResult Dojo(int dojoID)
        {
            if(dojoID == 1) //cannot visit "Rogue Dojo, ID=1", How to I make Rogue default for NULL value, instead of making a record?
            {
                return Redirect("/Dojos");
            }
            @ViewBag.Dojo = DojFactory.GetDojoByID(dojoID);
            @ViewBag.Rogue = DojFactory.GetDojoByID(1); 
            return View("DojoDetail");
        }

        [HttpGet]
        [Route("Ninjas")]
        public IActionResult AddNinja()
        {
            @ViewBag.Ninjas = NinFactory.FindAll();
            @ViewBag.Dojos = DojFactory.FindAll();
            return View("Ninja");
        }

        [HttpPost]
        [Route("processninja")]
        public IActionResult ProcessNinja(Ninja ninja)
        {
            if(ModelState.IsValid)
            {
                NinFactory.Register(ninja);
                return Redirect("/Ninjas");
            }
            return View("Ninja");
        }

        [HttpGet]
        [Route("/Ninja/{ninjaID}")]
        public IActionResult Ninja(int ninjaID)
        {
            ViewBag.Ninja = NinFactory.GetNinjaByID(ninjaID);
            return View("NinjaDetail");
        }

        [HttpGet]
        [Route("/Ninja/Banish/{dojoID}/{ninjaID}")]
        public IActionResult BanishNinja(int dojoID, int ninjaID)
        {
            NinFactory.Banish(ninjaID);
            return Redirect("/Dojo/"+dojoID);
        }

        [HttpGet]
        [Route("/Ninja/Recruit/{dojoID}/{ninjaID}")]
        public IActionResult RecruitNinja(int dojoID, int ninjaID)
        {
            NinFactory.Recruit(ninjaID, dojoID);
            return Redirect("/Dojo/"+dojoID);
        }
    }
}

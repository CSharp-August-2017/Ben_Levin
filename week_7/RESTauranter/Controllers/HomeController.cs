using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RESTauranter.Models;

namespace RESTauranter.Controllers
{
    public class HomeController : Controller
    {
        private RESTauranterContext _context;
    
        public HomeController(RESTauranterContext context)
        {
            _context = context;
        }
    
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd"); //default value for date field
            return View("Index");
        }

        [HttpGet]
        [Route("reviews")]
        public IActionResult Reviews()
        {
            List<string> FormattedVisit = new List<string>();
            List<int> YesHelp = new List<int>();
            List<int> UnHelp = new List<int>();
            List<Review> AllReviews = _context.Reviews.OrderByDescending(review => review.Visit).ToList();
            foreach(var review in AllReviews)
            {
                string FormDate = review.Visit.ToString("MMMM dd, yyyy");
                FormattedVisit.Add(FormDate);
                YesHelp.Add(_context.Helps.Where(help => help.Review_ID == review.ID).Where(help => help.Value == "yes").Count());
                UnHelp.Add(_context.Helps.Where(help => help.Review_ID == review.ID).Where(help => help.Value == "no").Count());
            }
            ViewBag.YesHelp = YesHelp;
            ViewBag.UnHelp = UnHelp;
            ViewBag.FormDate = FormattedVisit;
            ViewBag.AllReviews = AllReviews;
            return View("Reviews");
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(Review review, string Visit)
        {
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
            if(ModelState.IsValid)
            {
                if(Convert.ToDateTime(Visit) > Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    ViewBag.Error = "date of visit may not occur in the future";
                    return View("Index");
                };
                review.Visit = Convert.ToDateTime(Visit);
                review.Created_At = DateTime.Now;
                review.Updated_At = DateTime.Now;
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return Redirect("reviews");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("helpful")]
        public IActionResult Helpful(int id, string value)
        {
            if(value == "yes")
            {
                Console.WriteLine("YESSSSSSS");
                Help NewHelp = new Help
                {
                    Review_ID = id,
                    Value = value,
                    Created_At = DateTime.Now,
                    Updated_At = DateTime.Now,
                };
                _context.Helps.Add(NewHelp);
                _context.SaveChanges();
            }
            else if(value == "no")
            {
                Console.WriteLine("NOOOOOOOOOOO");
                Help NewHelp = new Help
                {
                    Review_ID = id,
                    Value = value,
                    Created_At = DateTime.Now,
                    Updated_At = DateTime.Now,
                };
                _context.Helps.Add(NewHelp);
                _context.SaveChanges();
            }
            return Redirect("/reviews");
        }
    }
}
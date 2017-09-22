using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using wedding_planner.Models;

using Microsoft.Extensions.Options;

namespace wedding_planner.Controllers
{
    public class WeddingController : Controller
    {

        private WeddingContext _context;
        private readonly GoogleMap _GoogleMap;
        public WeddingController(WeddingContext context, IOptions<GoogleMap> GoogleMap)
        {
            _context = context;
            _GoogleMap = GoogleMap.Value;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("ID") != null) //redirect if user already signed in
            {
                return Redirect("/dashboard");
            }
            ViewBag.RegistrationError = null;
            ViewBag.LoginError = null;
            return View("Index");
        }

        [HttpPost]
        [Route("processregistration")]
        public IActionResult ProcessRegistration(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = _context.Users.SingleOrDefault(User => User.Email == model.Email); //checking db for e-mail address to prevent duplicates
                if(user == null) //if e-mail not already in use
                {
                    PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                    string hashed = Hasher.HashPassword(model, model.Password);
                    User newUser = new User()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = hashed,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    LoginViewModel login = new LoginViewModel() //creating login model for automatic login process
                    {
                        LogEmail = newUser.Email,
                        LogPassword = model.Password,
                    };
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    return ProcessLogin(login);
                }
                ViewBag.RegistrationError = "e-mail address is already in use";
            }
            return View("Index");
        }

        [HttpPost]
        [Route("processlogin")]
        public IActionResult ProcessLogin(LoginViewModel login)
        {
            if(ModelState.IsValid)
            {
                User user = _context.Users.SingleOrDefault(User => User.Email == login.LogEmail); //searching for e-mail in db
                if(user != null) //if e-mail is in db
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    var Authentication = Hasher.VerifyHashedPassword(user, user.Password, login.LogPassword); //confirm password
                    if(Authentication == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetInt32("ID", user.ID); //logging in via session variables
                        HttpContext.Session.SetString("Name", user.FirstName);
                        return Redirect("/");
                    }
                }
                ViewBag.LoginError = "invalid password"; //intentionally ambiguous for null e-mail (security reasons)
            }
            return View("Index");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("ID") == null) //redirect if user not signed in
            {
                return Redirect("/");
            }
            List<Wedding> WeddingDateCheck = _context.Weddings.OrderByDescending(wedding => wedding.WeddingDate).ToList();
            foreach(var weddingevent in WeddingDateCheck) //deleting wedding if date has passed
            {
                DateTime DateCheck = (DateTime)weddingevent.WeddingDate;
                if(Convert.ToDateTime(DateCheck.ToString("yyyy-MM-dd")) < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    _context.Weddings.Remove(weddingevent);
                    _context.SaveChanges();
                }
            }
            List<int> GuestCount = new List<int>(); //creating and gathering dashboard view data
            List<int> RSVP = new List<int>();
            List<string> FormattedDates = new List<string>();
            List<Wedding> Weddings = _context.Weddings.OrderByDescending(wedding => wedding.WeddingDate).ToList();
            foreach(var weddingevent in Weddings)
            {
                DateTime WeddingDate = (DateTime)weddingevent.WeddingDate;
                FormattedDates.Add(WeddingDate.ToString("MMMM dd, yyyy"));
                List<RSVP> WeddingRSVPS = new List<RSVP>();
                WeddingRSVPS = _context.RSVPS.Where(rsvp => rsvp.WeddingID == weddingevent.ID).ToList();
                GuestCount.Add(WeddingRSVPS.Count());
                RSVP RSVPcheck = _context.RSVPS.Where(rsvp => rsvp.WeddingID == weddingevent.ID).Where(rsvp => rsvp.UserID == HttpContext.Session.GetInt32("ID")).SingleOrDefault();
                if(RSVPcheck == null)
                {
                    RSVP.Add(0); //represents no RSVP
                }
                else if(RSVPcheck != null)
                {
                    RSVP.Add(1); //represents existing RSVP
                }
            }
            ViewBag.ID = HttpContext.Session.GetInt32("ID");
            ViewBag.Weddings = Weddings;
            ViewBag.GuestCount = GuestCount;
            ViewBag.WeddingDates = FormattedDates;
            ViewBag.RSVP = RSVP;
            return View("Dashboard");
        }

        [HttpGet]
        [Route("planwedding")]
        public IActionResult PlanWedding()
        {
            if(HttpContext.Session.GetInt32("ID") == null) //redirect if user not signed in
            {
                return Redirect("/");
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.WeddingError = null;
            return View("PlanWedding");
        }

        [HttpPost]
        [Route("processwedding")]
        public IActionResult ProcessWedding(Wedding NewWedding)
        {
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
            if(ModelState.IsValid)
            {
                DateTime WeddingDate = (DateTime)NewWedding.WeddingDate;
                if(Convert.ToDateTime(WeddingDate.ToString("yyyy-MM-dd")) < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    ViewBag.WeddingError = "wedding date may not occur in the past";
                    return View("PlanWedding");
                };
                NewWedding.UserID = (int)HttpContext.Session.GetInt32("ID");
                NewWedding.CreatedAt = DateTime.Now;
                NewWedding.UpdatedAt = DateTime.Now;
                _context.Weddings.Add(NewWedding);
                _context.SaveChanges();
                return Redirect("/dashboard");
            }
            return View("PlanWedding");
        }

        [HttpPost]
        [Route("deletewedding")]
        public IActionResult DeleteWedding(int weddingID)
        {
            Wedding WeddingDelete = _context.Weddings.Where(wedding => wedding.ID == weddingID).SingleOrDefault();
            _context.Weddings.Remove(WeddingDelete);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }

        [HttpPost]
        [Route("rsvp")]
        public IActionResult RSVP(int weddingID)
        {
            RSVP NewRSVP = new RSVP
            {
                UserID = (int)HttpContext.Session.GetInt32("ID"),
                WeddingID = weddingID,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            _context.RSVPS.Add(NewRSVP);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }

        [HttpPost]
        [Route("unrsvp")]
        public IActionResult UnRSVP(int weddingID)
        {
            RSVP RSVPdelete = _context.RSVPS.Where(rsvp => rsvp.WeddingID == weddingID).Where(rsvp => rsvp.UserID == HttpContext.Session.GetInt32("ID")).SingleOrDefault();
            _context.RSVPS.Remove(RSVPdelete);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }

        [HttpGet]
        [Route("/wedding/{weddingID}")]
        public IActionResult Wedding(int weddingID)
        {
            if(HttpContext.Session.GetInt32("ID") == null) //redirect if user not signed in
            {
                return Redirect("/");
            }
            List<RSVP> WeddingRSVPS = new List<RSVP>();
            Wedding WeddingDetail = _context.Weddings.Where(wedding => wedding.ID == weddingID).SingleOrDefault();
            WeddingRSVPS = _context.RSVPS.Where(rsvp => rsvp.WeddingID == weddingID).ToList();
            foreach(var rsvp in WeddingRSVPS)
            {
                WeddingDetail.Guests.Add(_context.Users.Where(user => user.ID == rsvp.UserID).SingleOrDefault());
            }
            DateTime WeddingDate = (DateTime)WeddingDetail.WeddingDate;
            ViewBag.GoogleMap = _GoogleMap.URLString;
            ViewBag.WeddingDate = WeddingDate.ToString("MMMM dd, yyyy");
            ViewBag.Wedding = WeddingDetail;
            return View("ViewWedding");
        }
    }    
}

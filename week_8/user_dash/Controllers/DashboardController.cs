using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
//ADDED BELOW
using Microsoft.AspNetCore.Identity; //for password hashing
using System.Linq; //for queries

using user_dash.Models;

namespace user_dash.Controllers
{
    public class DashboardController : Controller
    {
        private UDContext _context;
    
        public DashboardController(UDContext context)
        {
            _context = context;
        }

        public void DefaultViewBag() //SHOULD CREATE MORE PUBLIC METHODS TO REDUCE LENGTH OF CONTROLLER
        {
            ViewBag.ID = HttpContext.Session.GetInt32("ID");
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.AdminLevel = HttpContext.Session.GetInt32("AdminLevel");
        }

//GET URLS

        [HttpGet]
        [Route("/dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("ID") == null) //redirect if user not signed in
            {
                return Redirect("/");
            }
            if(HttpContext.Session.GetInt32("AdminLevel") == 9) //redirect if user is an admin
            {
                return Redirect("/dashboard/admin");
            }
            List<User> AllUsers = _context.Users.OrderBy(user => user.ID).ToList();
            List<UserRenderModel> FormattedUsers = new List<UserRenderModel>();
            foreach(var user in AllUsers)
            {
                UserRenderModel FormattedUser = new UserRenderModel()
                {
                    ID = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FirstName+" "+user.LastName,
                    Email = user.Email,
                    CreatedDate = ((DateTime)user.Created_At).ToString("MMMM dd, yyyy"),
                };
                if(user.AdminLevel == 9)
                {
                    FormattedUser.AdminLevel = "admin";
                }
                else if(user.AdminLevel == 5)
                {
                    FormattedUser.AdminLevel = "normal";
                }
                FormattedUsers.Add(FormattedUser);
            }
            DefaultViewBag();
            ViewBag.Users = FormattedUsers;
            return View("Dashboard");
        }

        [HttpGet]
        [Route("/dashboard/admin")]
        public IActionResult AdminDashboard()
        {
            if(HttpContext.Session.GetInt32("ID") == null) //redirect if user not signed in
            {
                return Redirect("/");
            }
            if(HttpContext.Session.GetInt32("AdminLevel") != 9) //redirect if user is not an admin
            {
                return Redirect("/dashboard/admin");
            }
            List<User> AllUsers = _context.Users.OrderBy(user => user.ID).ToList();
            List<UserRenderModel> FormattedUsers = new List<UserRenderModel>();
            foreach(var user in AllUsers)
            {
                UserRenderModel FormattedUser = new UserRenderModel()
                {
                    ID = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FirstName+" "+user.LastName,
                    Email = user.Email,
                    CreatedDate = ((DateTime)user.Created_At).ToString("MMMM dd, yyyy"),
                };
                if(user.AdminLevel == 9)
                {
                    FormattedUser.AdminLevel = "admin";
                }
                else if(user.AdminLevel == 5)
                {
                    FormattedUser.AdminLevel = "normal";
                }
                FormattedUsers.Add(FormattedUser);
            }
            DefaultViewBag();
            ViewBag.Users = FormattedUsers;
            return View("Dashboard");
        }
    }
}
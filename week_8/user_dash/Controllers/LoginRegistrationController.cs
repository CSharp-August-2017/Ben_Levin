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
    public class LoginRegistrationController : Controller
    {
        private UDContext _context;
    
        public LoginRegistrationController(UDContext context)
        {
            _context = context;
        }

//**LOGIN + LOGOUT + REGISTER

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("ID") != null) //redirect if user already signed in
            {
                return Redirect("/dashboard");
            }
            return View("Index");
        }
        
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginSendModel Login)
        {
            if(ModelState.IsValid)
            {
                User user = _context.Users.SingleOrDefault(User => User.Email == Login.LogEmail); //searching for e-mail in db
                if(user != null) //if e-mail is in db
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    var Authentication = Hasher.VerifyHashedPassword(user, user.Password, Login.LogPassword); //check hashed password match
                    if(Authentication == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetInt32("ID", user.ID); //logging in via session variables
                        HttpContext.Session.SetString("Name", user.FirstName);
                        HttpContext.Session.SetInt32("AdminLevel", user.AdminLevel);
                        return Redirect("/");
                    }
                }
                ViewBag.LoginError = "invalid password"; //intentionally ambiguous for null e-mail (security reasons)
            }
            return View("Login");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterSendModel Register)
        {
            if(ModelState.IsValid)
            {
                List<User> firstusercheck = _context.Users.ToList();
                User user = _context.Users.SingleOrDefault(User => User.Email == Register.RegEmail); //checking db for e-mail address to prevent duplicates
                if(user == null) //if e-mail not already in use
                {
                    PasswordHasher<RegisterSendModel> Hasher = new PasswordHasher<RegisterSendModel>();
                    string HashedPassword = Hasher.HashPassword(Register, Register.RegPassword); //hashing password
                    User newUser = new User() //creating user
                    {
                        FirstName = Register.RegFirstName,
                        LastName = Register.RegLastName,
                        Email = Register.RegEmail,
                        Password = HashedPassword,
                        Created_At = DateTime.Now,
                        Updated_At = DateTime.Now,
                    };
                    if(firstusercheck.Count() == 0)
                    {
                        newUser.AdminLevel = 9; //first registered user receives highest admin level                    
                    }
                    else if(firstusercheck.Count() != 0)
                    {
                        newUser.AdminLevel = 5; //default user level is 5 (normal)
                    }
                    LoginSendModel UserLogin = new LoginSendModel() //creating login model for automatic login
                    {
                        LogEmail = newUser.Email,
                        LogPassword = Register.RegPassword,
                    };
                    _context.Users.Add(newUser); //adding user to database
                    _context.SaveChanges();
                    return Login(UserLogin);
                }
                ViewBag.RegistrationError = "e-mail address is already in use";
            }
            return View("Register");
        }

//**GET REQUESTS SENT TO POST URLS

        [HttpGet]
        [Route("login")]
        public IActionResult LoginPage()
        {
            if(HttpContext.Session.GetInt32("ID") != null) //redirect if user already signed in
            {
                return Redirect("/dashboard");
            }
            return View("Login");
        }

        [HttpGet]
        [Route("register")]
        public IActionResult RegisterPage()
        {
            if(HttpContext.Session.GetInt32("ID") != null) //redirect if user already signed in
            {
                return Redirect("/dashboard");
            }
            return View("Register");
        }
    }
}
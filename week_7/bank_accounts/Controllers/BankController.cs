using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using bank_accounts.Models;

namespace bank_accounts.Controllers
{
    public class BankController : Controller
    {
        private BankContext _context;
    
        public BankController(BankContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Registration()
        {
            if(HttpContext.Session.GetInt32("ID") != null) //redirect if user is already signed in
            {
                return Redirect("/account/"+HttpContext.Session.GetInt32("ID"));
            }
            ViewBag.Error = null;
            return View("Registration");
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
                        Email = newUser.Email,
                        Password = model.Password,
                    };
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    return ProcessLogin(login);
                }
                ViewBag.Error = "e-mail address already in use";
            }
            return View("Registration");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            if(HttpContext.Session.GetInt32("ID") != null)
            {
                return Redirect("/account/"+HttpContext.Session.GetInt32("ID"));
            }
            return View("Login");
        }

        [HttpPost]
        [Route("processlogin")]
        public IActionResult ProcessLogin(LoginViewModel login)
        {
            if(ModelState.IsValid)
            {
                User user = _context.Users.SingleOrDefault(User => User.Email == login.Email); //searching for e-mail in db
                if(user != null) //if e-mail is in db
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    var Authentication = Hasher.VerifyHashedPassword(user, user.Password, login.Password); //confirm password
                    if(Authentication == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetInt32("ID", user.ID); //logging in via session variables
                        HttpContext.Session.SetString("Name", user.FirstName);
                        return Redirect("/account/"+HttpContext.Session.GetInt32("ID"));
                    }
                }
                ViewBag.Error = "invalid password"; //intentionally ambiguous for null e-mail (security reasons)
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

        [HttpGet]
        [Route("account/{ID}")]
        public IActionResult Account(int ID, string error = null)
        {
            if(ID == HttpContext.Session.GetInt32("ID"))
            {
                List<double> Amounts = new List<double>();
                List<string> FormattedAmounts = new List<string>();
                List<string> FormattedDates = new List<string>();
                List<Transaction> Transactions = _context.Transactions.Where(transaction => transaction.UserID == ID).ToList();
                foreach(var transaction in Transactions)
                {
                    Amounts.Add(transaction.Amount);
                    FormattedAmounts.Add(transaction.Amount.ToString("C"));
                    FormattedDates.Add(transaction.CreatedAt.ToString("MMMM dd, yyyy"));
                }
                ViewBag.TransactionAmounts = FormattedAmounts;
                ViewBag.FormattedAccountSum = Amounts.Sum().ToString("C");
                ViewBag.TransactionDates = FormattedDates;
                ViewBag.Error = error; //from process transaction redirect
                ViewBag.Name = HttpContext.Session.GetString("Name");
                return View("Account");
            }
            else if(HttpContext.Session.GetInt32("ID") == null)
            {
                return Redirect("/login"); 
            }
            return Redirect("/account/"+HttpContext.Session.GetInt32("ID"));
        }

        [HttpPost]
        [Route("processtransaction")]
        public IActionResult ProcessTransaction(Transaction newtransaction)
        {
            if(ModelState.IsValid)
            {
                List<double> Amounts = new List<double>(); //fetching account balance
                List<Transaction> Transactions = _context.Transactions.Where(transaction => transaction.UserID == HttpContext.Session.GetInt32("ID")).ToList();
                foreach(var transaction in Transactions)
                {
                    Amounts.Add(transaction.Amount);
                }
                double AccountBalance = Amounts.Sum();
                if((newtransaction.Amount < 0.01) && (newtransaction.Amount > -0.01))
                {
                    string error1 = "transaction amount may not be $0.00";
                    return Account((int)HttpContext.Session.GetInt32("ID"), error1);
                }
                else if((newtransaction.Amount + AccountBalance) < 0)
                {
                    string error2 = "your current account balance does not allow that transaction";
                    return Account((int)HttpContext.Session.GetInt32("ID"), error2);
                }
                else if((newtransaction.Amount > 0) || (newtransaction.Amount < 0))
                {
                    newtransaction.UserID = HttpContext.Session.GetInt32("ID");
                    newtransaction.CreatedAt = DateTime.Now;
                    newtransaction.UpdatedAt = DateTime.Now;
                    _context.Transactions.Add(newtransaction);
                    _context.SaveChanges();
                    return Redirect("/account/"+HttpContext.Session.GetInt32("ID"));
                }
            }
            string error3 = "invalid transaction entry";
            return Account((int)HttpContext.Session.GetInt32("ID"), error3);
        }
    }
}

//this code is not clean
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using wall.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace wall.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbConnector _dbConnector;
        public HomeController(DbConnector connect)
        {
            _dbConnector = connect;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            //redirect if user is already signed in
            if((int?)HttpContext.Session.GetInt32("ID") != null)
            {
                return Redirect("wall");
            }
            ViewBag.RegErrors = new List<string>();
            ViewBag.LogErrors = new List<string>();
            return View("Index");
        }

        [HttpGet]
        [Route("wall")]
        public IActionResult Wall()
        //redirect if user is not signed in
        {
            if((int?)HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("Index");
            }
            //creating page render timestamp
            DateTime currentTime = DateTime.Now;
            //grabbing all messages
            string AllMessages = $"SELECT * FROM messages ORDER BY created_at DESC ;";
            List<Dictionary<string, object>> Messages = _dbConnector.Query(AllMessages);
            //adding message fields for rendered view
            foreach(var message in Messages)
            {
                string messageUser = $"SELECT * FROM users WHERE id='{message["user_id"]}';";
                List<Dictionary<string, object>> User = _dbConnector.Query(messageUser);
                message.Add("Name", (string)User[0]["first_name"]+' '+(string)User[0]["last_name"]);
                message.Add("FormDate", ((string)((DateTime)message["created_at"]).ToString("MMMM dd, yyyy"))+" | "+(string)((DateTime)message["created_at"]).ToString("h:mm tt")); //formatted date for page display
                int time = DateTime.Compare(currentTime, (DateTime)message["created_at"]);
                if((currentTime.Subtract((DateTime)message["created_at"])) <= TimeSpan.FromMinutes(30)) 
                {
                    message.Add("TimeDelete", "yes");
                }
                else
                {
                    message.Add("TimeDelete", "no");
                }
                //grabbing comments per message
                string AllComments = $"SELECT * FROM comments WHERE message_id='{message["id"]}' ORDER BY created_at ASC;";
                List<Dictionary<string, object>> MessageComments = _dbConnector.Query(AllComments);
                foreach(var comment in MessageComments)
                {
                    string commentUser = $"SELECT * FROM users WHERE id='{comment["user_id"]}';";
                    List<Dictionary<string, object>> CUser = _dbConnector.Query(commentUser);
                    comment.Add("Name", (string)CUser[0]["first_name"]+' '+(string)CUser[0]["last_name"]);
                    comment.Add("FormDate", ((string)((DateTime)comment["created_at"]).ToString("MMMM dd, yyyy"))+" | "+(string)((DateTime)comment["created_at"]).ToString("h:mm tt")); 
                }
                message.Add("Comments", MessageComments);
            }
            //passing session variables for form functionality
            ViewBag.ID = HttpContext.Session.GetInt32("ID");
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.Messages = Messages;
            return View("Wall");
        }

        [HttpPost]
        [Route("createMessage")]
        public IActionResult CreateMessage(string message)
        {
            //preventing empty messages (no error message)
            if(message == null)
            {
                return RedirectToAction("Wall");
            }
            string Insert = $"INSERT INTO messages (user_id, message, created_at, updated_at) VALUES ('{HttpContext.Session.GetInt32("ID")}', '{message}', NOW(), NOW());";
            _dbConnector.Execute(Insert);
            return RedirectToAction("Wall");
        }

        [HttpPost]
        [Route("/deleteMessage/{mID}")]
        public IActionResult DeleteMessage(int mID)
        {
            string deleteMessage = $"DELETE FROM messages WHERE id='{mID}';"; //logging new user in
            _dbConnector.Execute(deleteMessage);
            return RedirectToAction("Wall");
        }

        [HttpPost]
        [Route("/createComment/{mID}")]
        public IActionResult CreateComment(int mID, string comment)
        {
            //preventing empty comments (no error message)
            if(comment == null)
            {
                return RedirectToAction("Wall");
            }
            string Insert = $"INSERT INTO comments (user_id, message_id, comment, created_at, updated_at) VALUES ('{(int?)HttpContext.Session.GetInt32("ID")}', '{mID}', '{comment}', NOW(), NOW());";
            _dbConnector.Execute(Insert);
            return RedirectToAction("Wall");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User user)
        {
            ViewBag.LogErrors = new List<string>();
            if(ModelState.IsValid)
            {
                //checking if email is already registered
                string duplicateCheck = $"SELECT * FROM users WHERE email='{user.Email}';"; //checking for registered email
                List<Dictionary<string, object>> emailCheck = _dbConnector.Query(duplicateCheck);
                if(emailCheck.Count > 0)
                {
                    ViewBag.RegErrors = new List<string>{"e-mail address is already registered"};
                    return View("Index");
                }
                //hashing password
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                //adding user to DB
                string addUser = $"INSERT INTO users (first_name, last_name, email, password, created_at, updated_at)VALUES ('{user.FirstName}', '{user.LastName}', '{user.Email}', '{user.Password}', NOW(), NOW());";
                _dbConnector.Execute(addUser);
                //automatically logging user in
                string selectQuery = $"SELECT * FROM users WHERE email='{user.Email}';"; //logging new user in
                List<Dictionary<string, object>> UserQuery = _dbConnector.Query(selectQuery);
                HttpContext.Session.SetInt32("ID", (int)UserQuery[0]["id"]);
                HttpContext.Session.SetString("Name", user.FirstName);
                return RedirectToAction("Wall");
            }
            ViewBag.RegErrors = new List<string>();
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password)
        {
            ViewBag.RegErrors = new List<string>();
            //checking for contents in email field
            if(Email != null)
            {
                //checking DB for email
                string selectQuery = $"SELECT * FROM users WHERE email='{Email}';";
                List<Dictionary<string, object>> UserQuery = _dbConnector.Query(selectQuery);
                if(UserQuery.Count > 0)
                {
                    //grabbing user info for email
                    User checkUser = new User
                    {
                        FirstName = (string)UserQuery[0]["first_name"],
                        LastName = (string)UserQuery[0]["last_name"],
                        Email = (string)UserQuery[0]["email"],
                        Password = (string)UserQuery[0]["password"],
                    };
                    //checking for contents in password field
                    if(Password != null)
                    {
                        //verifying password matches user password from DB
                        PasswordHasher<User> Hasher = new PasswordHasher<User>();
                        var authen = Hasher.VerifyHashedPassword(checkUser, checkUser.Password, Password); //confirm password
                        if(authen == PasswordVerificationResult.Success) //if pw matches
                        {
                            HttpContext.Session.SetInt32("ID", (int)UserQuery[0]["id"]); //set session vars
                            HttpContext.Session.SetString("Name", checkUser.FirstName);
                            return Redirect("wall");
                        }
                    }
                    ViewBag.LogErrors = new List<string>{"invalid password entry"};
                    return View("Index");
                }
                ViewBag.LogErrors = new List<string>{"e-mail is not registered"};
                return View("Index");
            }
            ViewBag.LogErrors = new List<string>{"invalid e-mail entry"};
            return View("Index");
        }
    }
}
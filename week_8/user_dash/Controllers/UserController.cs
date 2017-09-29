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
    public class UserController : Controller
    {
        private UDContext _context;

        public void DefaultViewBag() //SHOULD CREATE MORE PUBLIC METHODS TO REDUCE LENGTH OF CONTROLLER
        {
            ViewBag.ID = HttpContext.Session.GetInt32("ID");
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.AdminLevel = HttpContext.Session.GetInt32("AdminLevel");
        }

        public UserController(UDContext context)
        {
            _context = context;
        }

//GET URLS

        [HttpGet]
        [Route("/users/new")]
        public IActionResult NewUser()
        {
            if(HttpContext.Session.GetInt32("ID") == null) //redirect if user not signed in
            {
                return Redirect("/");
            }
            DefaultViewBag();
            return View("UserAdd");
        }

        [HttpGet]
        [Route("/users/edit/{UID}")]
        public IActionResult EditUser(int UID)
        {
            if(HttpContext.Session.GetInt32("ID") == null) //redirect if user not signed in
            {
                return Redirect("/");
            }
            User User = _context.Users.SingleOrDefault(user => user.ID == UID);
            UserRenderModel RenderedUser = new UserRenderModel()
            {
                ID = User.ID,
                FirstName = User.FirstName,
                LastName = User.LastName,
                FullName = User.FirstName+" "+User.LastName,
                Email = User.Email,
                Description = User.Description,
                CreatedDate = ((DateTime)User.Created_At).ToString("MMMM dd, yyyy"),
            };
            if(User.AdminLevel == 9)
            {
                RenderedUser.AdminLevel = "admin";
            }
            else if(User.AdminLevel == 5)
            {
                RenderedUser.AdminLevel = "normal";
            }
            DefaultViewBag();
            ViewBag.User = RenderedUser;
            return View("UserEdit");
        }

        [HttpGet]
        [Route("/users/remove/{UID}")]
        public IActionResult RemoveUser(int UID)
        {
            if(HttpContext.Session.GetInt32("ID") == null) //redirect if user not signed in
            {
                return Redirect("/");
            }
            User User = _context.Users.SingleOrDefault(user => user.ID == UID);
            UserRenderModel RenderedUser = new UserRenderModel()
            {
                ID = User.ID,
                FullName = User.FirstName+" "+User.LastName,
                Email = User.Email,
                Description = User.Description,
                CreatedDate = ((DateTime)User.Created_At).ToString("MMMM dd, yyyy"),
            };
            if(User.AdminLevel == 9)
            {
                RenderedUser.AdminLevel = "admin";
            }
            else if(User.AdminLevel == 5)
            {
                RenderedUser.AdminLevel = "normal";
            }
            DefaultViewBag();
            ViewBag.User = RenderedUser;
            return View("UserRemove");
        }

//POST URLS

        [HttpPost]
        [Route("/users/remove")]
        public IActionResult ProcessRemoveUser(int UID)
        {
            User User = _context.Users.SingleOrDefault(user => user.ID == UID);
            List<Message> MessagesPosted = _context.Messages.Where(message => message.AuthorID == UID).ToList();
            List<Message> MessagesReceived = _context.Messages.Where(message => message.UserID == UID).ToList();
            List<Comment> UserComments = _context.Comments.Where(comment => comment.AuthorID == UID).ToList();
            foreach(var post in MessagesPosted)
            {
                List<Comment> Comments = _context.Comments.Where(comment => comment.MessageID == post.ID).ToList();
                foreach(var comment in Comments)
                {
                    _context.Comments.Remove(comment);
                }
                _context.Messages.Remove(post);
            }
            foreach(var receive in MessagesReceived)
            {
                List<Comment> Comments = _context.Comments.Where(comment => comment.MessageID == receive.ID).ToList();
                foreach(var comment in Comments)
                {
                    _context.Comments.Remove(comment);
                }
                _context.Messages.Remove(receive);
            }
            foreach(var comment in UserComments)
            {
                _context.Comments.Remove(comment);
            }
            _context.Users.Remove(User);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }

        [HttpPost]
        [Route("/updatedescription")]
        public IActionResult UpdateDescription (UserEditDescriptionSendModel UserUpdate)
        {
            if(ModelState.IsValid)
            {
                User User = _context.Users.SingleOrDefault(user => user.ID == UserUpdate.ID);
                User.Description = UserUpdate.EdDescription;
                User.Updated_At = DateTime.Now;
                _context.SaveChanges();
                return Redirect("/users/edit/"+UserUpdate.ID);
            }
            return EditUser(UserUpdate.ID);
        }

        [HttpPost]
        [Route("/updateinfo")]
        public IActionResult UpdateInfo (UserEditInfoSendModel UserUpdate)
        {
            if(ModelState.IsValid)
            {
                User User = _context.Users.SingleOrDefault(user => user.ID == UserUpdate.ID);
                User.FirstName = UserUpdate.EdFirstName;
                User.LastName = UserUpdate.EdLastName;
                User.Email = UserUpdate.EdEmail;
                User.Updated_At = DateTime.Now;
                _context.SaveChanges();
                return Redirect("/users/edit/"+UserUpdate.ID);
            }
            return EditUser(UserUpdate.ID);
        }

        [HttpPost]
        [Route("/updatepassword")]
        public IActionResult UpdatePassword (UserEditPasswordSendModel UserUpdate)
        {
            if(ModelState.IsValid)
            {
                User User = _context.Users.SingleOrDefault(user => user.ID == UserUpdate.ID);
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                string HashedPassword = Hasher.HashPassword(User, UserUpdate.EdPassword); //hashing password
                User.Password = HashedPassword;
                User.Updated_At = DateTime.Now;
                _context.SaveChanges();
                return Redirect("/users/edit/"+UserUpdate.ID);
            }
            return EditUser(UserUpdate.ID);
        }

        [HttpPost]
        [Route("/users/new")]
        public IActionResult CreateUser(RegisterSendModel Register)
        {
            DefaultViewBag();
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
                        AdminLevel = 5,
                    };
                    _context.Users.Add(newUser); //adding user to database
                    _context.SaveChanges();
                    return Redirect("/dashboard/admin");
                }
                ViewBag.RegistrationError = "e-mail address is already in use";
            }
            return View("UserAdd");
        }

// GET REDIRECTS FOR POST URLS

        [HttpGet]
        [Route("/users/remove")]
        public IActionResult RedirectRemoveUser()
        {
            return Redirect("/");
        }
        [HttpGet]
        [Route("/updatedescription")]
        public IActionResult RedirectUpdateUserDescription()
        {
            return Redirect("/");
        }
        [HttpGet]
        [Route("/updateinfo")]
        public IActionResult RedirectUpdateUserInfo()
        {
            return Redirect("/");
        }
        [HttpGet]
        [Route("/updatepassword")]
        public IActionResult RedirectUpdateUserPassword()
        {
            return Redirect("/");
        }
    }
}
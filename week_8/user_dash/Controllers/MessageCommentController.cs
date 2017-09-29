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
    public class MessageCommentController : Controller
    {
        private UDContext _context;
    
        public void DefaultViewBag() //SHOULD CREATE MORE PUBLIC METHODS TO REDUCE LENGTH OF CONTROLLER
        {
            ViewBag.ID = HttpContext.Session.GetInt32("ID");
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.AdminLevel = HttpContext.Session.GetInt32("AdminLevel");
        }

        public string TimePastStringFunction(DateTime? DateInput) //FOR MESSAGE AND COMMENT RENDERING
        {
            DateTime Date = (DateTime)DateInput;
            string TimePastString = Date.ToString("MMMM dd, yyyy"); //DEFAULT
            if((Date.AddMonths(1)) > DateTime.Now)
            {
                TimePastString = ((DateTime.Now - Date).Days).ToString()+" days ago";
            }
            if((Date.AddHours(24)) > DateTime.Now)
            {
                TimePastString = ((DateTime.Now - Date).Hours).ToString()+" hours ago";
            }
            if((Date.AddMinutes(60)) > DateTime.Now)
            {
                TimePastString = ((DateTime.Now - Date).Minutes).ToString()+" minutes ago";
            }
            return TimePastString;
        }

        public MessageCommentController(UDContext context)
        {
            _context = context;
        }

//GET URLS

        [HttpGet]
        [Route("/users/show/{UID}")]
        public IActionResult ShowUser(int UID)
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
            List<Message> Messages = _context.Messages.Where(message => message.UserID == RenderedUser.ID).OrderByDescending(message => message.Created_At).ToList();
            List<MessageRenderModel> RenderedMessages = new List<MessageRenderModel>();
            foreach(var result in Messages)
            {
                User Author = _context.Users.SingleOrDefault(user => user.ID == result.AuthorID);
                string AuthorName = Author.FirstName+" "+Author.LastName;
                string TimePastString = TimePastStringFunction(result.Created_At);
                MessageRenderModel RenderedMessage = new MessageRenderModel()
                {
                    ID = result.ID,
                    Author = AuthorName,
                    Content = result.Content,
                    Created_At = ((DateTime)result.Created_At).ToString("MMMM dd, yyyy"),
                    TimePast = TimePastString,
                };
                List<Comment> Comments = _context.Comments.Where(comment => comment.MessageID == result.ID).OrderBy(comment => comment.Created_At).ToList();
                List<CommentRenderModel> RenderedComments = new List<CommentRenderModel>();
                foreach(var comment in Comments)
                {
                    User CommentAuthor = _context.Users.SingleOrDefault(user => user.ID == comment.AuthorID);
                    string CommentAuthorName = CommentAuthor.FirstName+" "+CommentAuthor.LastName;
                    string CommentTimePastString = TimePastStringFunction(comment.Created_At);
                    CommentRenderModel RenderedComment = new CommentRenderModel()
                    {
                        ID = comment.ID,
                        Author = CommentAuthorName,
                        Content = comment.Content,
                        TimePast = CommentTimePastString,
                    };
                    RenderedComments.Add(RenderedComment);
                } 
                RenderedMessage.Comments = RenderedComments;
                RenderedMessages.Add(RenderedMessage);
            }
            DefaultViewBag();
            ViewBag.User = RenderedUser;
            ViewBag.Messages = RenderedMessages;
            return View("UserProfile");
        }

//POST URLS

        [HttpPost]
        [Route("/newcomment")]
        public IActionResult NewComment(CommentSendModel Comment)
        {
            if(ModelState.IsValid)
            {
                Comment NewComment = new Comment()
                {
                    Content = Comment.CContent,
                    MessageID = Comment.MessageID,
                    AuthorID = Comment.AuthorID,
                    Created_At = (DateTime?)DateTime.Now,
                    Updated_At = (DateTime?)DateTime.Now,
                };
                _context.Comments.Add(NewComment);
                _context.SaveChanges();
                return Redirect("/users/show/"+Comment.UserID);
            }
            return ShowUser(Comment.UserID);
        }

        [HttpPost]
        [Route("/newmessage")]
        public IActionResult NewMessage(Message Message)
        {
            if(ModelState.IsValid)
            {
                Message.AuthorID = (int)HttpContext.Session.GetInt32("ID");
                Message.Created_At = (DateTime?)DateTime.Now;
                Message.Updated_At = (DateTime?)DateTime.Now;
                _context.Messages.Add(Message);
                _context.SaveChanges();
                return Redirect("/users/show/"+Message.UserID);
            }
            return ShowUser(Message.UserID);
        }

//GET REDIRECTS FOR POST URLS

        [HttpGet]
        [Route("/newcomment")]
        public IActionResult NewCommentRedirect()
        {
            return Redirect("/");
        }

        [HttpGet]
        [Route("/newmessage")]
        public IActionResult NewMessageRedirect()
        {
            return Redirect("/");
        }
    }
}
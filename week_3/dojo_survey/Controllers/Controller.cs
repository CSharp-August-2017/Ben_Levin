using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace dojo_survey{
    public class SurveyController : Controller{

        [Route("/")]
        public IActionResult Index() {
            return View("survey");
        }

        [HttpPost]
        [Route("result")]
        public IActionResult Result(string Name, string Location, string Language, string Comment) {
            var survey = new Dictionary<string, object>();
            survey["Name"] = Name;
            survey["Location"] = Location;
            survey["Language"] = Language;
            survey["Comment"] =  Comment;
            ViewBag.Result =  survey;
            return View("result");
        }

    }
}


//from solution



//                 ViewBag.Errors.Add("Name cannot be empty");
//             }

//             if(Location == null)
//             {
//                 ViewBag.Errors.Add("Please select  a valid location");
//             }

//             if(Language == null)
//             {
//                 ViewBag.Errors.Add("Please select a valid language");
//             }

//             if(Comment == null)
//             {
//                 Comment = "";
//             }

//             if(ViewBag.Errors.Count > 0)
//             {
//                 return View("Index");
//             }

//             ViewBag.Name = Name;
//             ViewBag.Location = Location;
//             ViewBag.Language = Language;
//             ViewBag.Comment = Comment;
            
//             return View("Success");
//         }
//     }
// }
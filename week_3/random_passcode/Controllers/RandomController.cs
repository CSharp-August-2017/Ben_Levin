using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace dojo_survey{

using Newtonsoft.Json;
 
    // Somewhere in your namespace, outside other classes
    public static class SessionExtensions
    {
        // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            // This helper function simply serializes theobject to JSON and stores it as a string in session
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        
        // generic type T is a stand-in indicating that we need to specify the type on retrieval
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            // Upon retrieval the object is deserialized based on the type we specified
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    public class RandomController : Controller{

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [Route("/")]
        public IActionResult Index() {
            if(HttpContext.Session.GetInt32("Count") > 0){
                int counter = (int)HttpContext.Session.GetInt32("Count");
                counter += 1;
                HttpContext.Session.SetInt32("Count", counter);
                ViewBag.Counter = counter;
                string passcode = RandomString(14);
                HttpContext.Session.SetString("Pass", passcode);
                ViewBag.Result = passcode;
                return View("index");
            }
            else{
                HttpContext.Session.SetInt32("Count", 0);
                int counter = (int)HttpContext.Session.GetInt32("Count");
                counter += 1;
                HttpContext.Session.SetInt32("Count", counter);
                ViewBag.Counter = counter;
                string passcode = RandomString(14);
                HttpContext.Session.SetString("Pass", passcode);
                ViewBag.Result = passcode;
                return View("index");
            }


        }

        [Route("/generate")]
        public IActionResult Generate() {
            return Redirect("/");
        }

        [Route("/reset")]
        public IActionResult Reset() {
            HttpContext.Session.Clear();
            HttpContext.Session.SetInt32("Count", 0);
            int counter = (int)HttpContext.Session.GetInt32("Count");
            HttpContext.Session.SetInt32("Count", counter);
            ViewBag.Counter = counter;
            string passcode = HttpContext.Session.GetString("Pass");
            ViewBag.Result = passcode;
            return View("index");
        }
    }
}
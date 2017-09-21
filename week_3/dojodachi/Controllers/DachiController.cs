using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace dojodachi{

using Newtonsoft.Json;

    public static class SessionExtensions // Somewhere in your namespace, outside other classes
    {
        public static void SetObjectAsJson(this ISession session, string key, object value) // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
        {
            session.SetString(key, JsonConvert.SerializeObject(value)); // This helper function simply serializes theobject to JSON and stores it as a string in session
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)  // generic type T is a stand-in indicating that we need to specify the type on retrieval
        {
            string value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);   // Upon retrieval the object is deserialized based on the type we specified
        }
    }

    public class DachiController : Controller{

        private static Random random = new Random();

        [Route("/")]
        public IActionResult Index() {
            return Redirect("/dojodachi");
        }

        [Route("/dojodachi")]
        public IActionResult Main() {
            if (HttpContext.Session.GetInt32("Happiness") == null){
                HttpContext.Session.SetInt32("Happiness", 20);
                HttpContext.Session.SetInt32("Fullness", 20);
                HttpContext.Session.SetInt32("Energy", 50);
                HttpContext.Session.SetInt32("Meals", 3);
                ViewBag.Happiness = (int)HttpContext.Session.GetInt32("Happiness");
                ViewBag.Fullness = (int)HttpContext.Session.GetInt32("Fullness");
                ViewBag.Energy = (int)HttpContext.Session.GetInt32("Energy");
                ViewBag.Meals = (int)HttpContext.Session.GetInt32("Meals");
                ViewBag.Message = HttpContext.Session.GetString("Message");              
                return View("Index");
            }
            else if((HttpContext.Session.GetInt32("Happiness") <= 0)||((HttpContext.Session.GetInt32("Fullness") <= 0))){
                HttpContext.Session.SetString("Message", "Your Dojodachi is real dead. RIP.");
                ViewBag.Message = HttpContext.Session.GetString("Message");
                return View("Lost");
            }
            else if((HttpContext.Session.GetInt32("Happiness") >= 100)||((HttpContext.Session.GetInt32("Fullness") >= 100))){
                HttpContext.Session.SetString("Message", "Congratulations! You have won!");
                ViewBag.Message = HttpContext.Session.GetString("Message");
                return View("Won");
            }
            else{
                ViewBag.Happiness = (int)HttpContext.Session.GetInt32("Happiness");
                ViewBag.Fullness = (int)HttpContext.Session.GetInt32("Fullness");
                ViewBag.Energy = (int)HttpContext.Session.GetInt32("Energy");
                ViewBag.Meals = (int)HttpContext.Session.GetInt32("Meals");  
                ViewBag.Message = HttpContext.Session.GetString("Message");
                return View("Index");
            }

        }
        
        [Route("/reset")]
        public IActionResult Reset() {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [Route("/feed")]
        public IActionResult Feed() {
            int like = random.Next(1,4);
            int meal = (int)HttpContext.Session.GetInt32("Meals");
            if(meal <= 0){
                HttpContext.Session.SetString("Message", "Your Dojodachi doesn't have any meals.");

                return Redirect("/dojodachi");
            }    
            else{
                if(like==1){
                    meal -= 1;
                    HttpContext.Session.SetInt32("Meals", meal);
                    int full = (int)HttpContext.Session.GetInt32("Fullness");
                    HttpContext.Session.SetString("Message", "Your Dojodachi does not want to eat. Fullness +0, Meal -1");
                    return Redirect("/dojodachi");
                }
                else{
                    meal -= 1;
                    HttpContext.Session.SetInt32("Meals", meal);
                    int full = (int)HttpContext.Session.GetInt32("Fullness");
                    int fullAdd = random.Next(5,10);
                    full += fullAdd;
                    HttpContext.Session.SetInt32("Fullness", full);
                    HttpContext.Session.SetString("Message", "Your fed your Dojodachi! Fullness +" + fullAdd + ", Meal -1");
                    return Redirect("/dojodachi");
                }
            }    
        }

        [Route("/play")]
        public IActionResult Play() {
            int energy = (int)HttpContext.Session.GetInt32("Energy");
            int like = random.Next(1,4);
            if(energy <= 0){
                HttpContext.Session.SetString("Message", "Your Dojodachi doesn't have any energy.");
                return Redirect("/dojodachi");
            }    
            else{
                if(like == 1){
                    energy -= 5;
                    HttpContext.Session.SetInt32("Energy", energy);
                    int happy = (int)HttpContext.Session.GetInt32("Happiness");
                    HttpContext.Session.SetString("Message", "Your Dojodachi does not want to play. Happiness +0, Energy -5");
                    return Redirect("/dojodachi");
                }
                else{
                    energy -= 5;
                    HttpContext.Session.SetInt32("Energy", energy);
                    int happy = (int)HttpContext.Session.GetInt32("Happiness");
                    int happyAdd = random.Next(5,10);
                    happy += happyAdd;
                    HttpContext.Session.SetInt32("Happiness", happy);
                    HttpContext.Session.SetString("Message", "Your played with your Dojodachi! Happiness +" + happyAdd + ", Energy -5");
                    return Redirect("/dojodachi");
                }
            }    
        }

        [Route("/work")]
        public IActionResult Work() {
            int energy = (int)HttpContext.Session.GetInt32("Energy");
            if(energy <= 0){
                HttpContext.Session.SetString("Message", "Your Dojodachi doesn't have any energy.");
                return Redirect("/dojodachi");
            }
            else{
                energy -= 5;
                HttpContext.Session.SetInt32("Energy", energy);
                int meal = (int)HttpContext.Session.GetInt32("Meals");
                int mealAdd = random.Next(1,3);
                meal += mealAdd;
                HttpContext.Session.SetInt32("Meals", meal);
                HttpContext.Session.SetString("Message", "Your put your Dojodachi to work! Meals +" + mealAdd + ", Energy -5");
                return Redirect("/dojodachi");
            }    
        }

        [Route("/sleep")]
        public IActionResult Sleep() {
            int full = (int)HttpContext.Session.GetInt32("Fullness"); //redundant, game should be lost
            int happy = (int)HttpContext.Session.GetInt32("Happiness");
            if((full <= 0)||(happy <= 0)){
                return Redirect("/dojodachi");
            }
            else{
                full -= 5;
                happy -= 5;
                HttpContext.Session.SetInt32("Fullness", full);
                HttpContext.Session.SetInt32("Happiness", happy);
                int energy = (int)HttpContext.Session.GetInt32("Energy");
                energy += 15;
                HttpContext.Session.SetInt32("Energy", energy);
                HttpContext.Session.SetString("Message", "Your Dojodachi slept! Energy +15, Fullness -5, Happiness -5");
                return Redirect("/dojodachi");
            }    
        }

    }
}
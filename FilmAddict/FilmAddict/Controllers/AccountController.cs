using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FilmAddict.App_Start;
using FilmAddict.Models;
using System.Web.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FilmAddict.Controllers
{
    public class AccountController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<UserAccount> userCollection;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                dbcontext = new MongoDBContext();
                userCollection = dbcontext.mongoDatabase.GetCollection<UserAccount>("User");
                {
                    userCollection.InsertOne(account);
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login (UserAccount user)
        {

            dbcontext = new MongoDBContext();
            userCollection = dbcontext.mongoDatabase.GetCollection<UserAccount>("User");

            List<UserAccount> users = userCollection.AsQueryable<UserAccount>().ToList();
            
            if (users.Contains(user))
            {
                Session["UserID"] = user.UserId.ToString();
                Session["Username"] = user.Username.ToString();
                return RedirectToAction("LoggedIn");

            }
            else
            {
                ModelState.AddModelError("", "Username or password is wrong.");
            }
            return View();
        }
        public ActionResult LoggedIn()
        {
            if(Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}

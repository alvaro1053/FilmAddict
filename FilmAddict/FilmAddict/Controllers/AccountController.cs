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

                List<UserAccount> users = userCollection.AsQueryable<UserAccount>().ToList();

               var films = new List<FilmModel>();


                var check = true;
                foreach(UserAccount user in users)
                {
                    if (user.Username.Equals(account.Username))
                    {
                        check = false;
                        ViewBag.Used = "Username already used.";
                    }
                    else
                    {
                        check = true;
                        account.Films = films;
                    }
                }
                if (check == true)
                {
                    if (account.Films==null) {

                        account.Films = films;
                    }

                    userCollection.InsertOne(account);

                    ModelState.Clear();
                    ViewBag.Message = account.Username + " succesfully registered.";
                    return RedirectToAction("Index", "Home");
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

            foreach (UserAccount u in users)
            {
                if ((user.Username.Equals(u.Username)) && user.Password.Equals(u.Password))
                {
                    Session["UserID"] = user.UserId.ToString();
                    Session["Username"] = user.Username.ToString();
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                   
                    ViewBag.Fail = "Username or password is wrong.";
                }

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

        public ActionResult Logout()
        {
            Session.Clear();
            ViewBag.session = Session["Username"];
            return RedirectToAction("LoggedIn");
        }

      

    }
}

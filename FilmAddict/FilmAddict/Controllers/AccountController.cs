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
                    }
                }
                if (check == true)
                {
                    userCollection.InsertOne(account);

                    ModelState.Clear();
                    ViewBag.Message = account.Username + " succesfully registered.";
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
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    //ModelState.AddModelError("", "Username or password is wrong.");
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

        /*       
        public JsonResult checkUsernameAvailability(string UserName)
        {
            dbcontext = new MongoDBContext();
            userCollection = dbcontext.mongoDatabase.GetCollection<UserAccount>("User");

            List<UserAccount> users = userCollection.AsQueryable<UserAccount>().ToList();

            var searchData = users.Where(u => u.Username == UserName).SingleOrDefault();

            if(searchData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }



            
            dbcontext = new MongoDBContext();
            userCollection = dbcontext.mongoDatabase.GetCollection<UserAccount>("User");

            List<UserAccount> users = userCollection.AsQueryable<UserAccount>().ToList();

            bool existsUser = users.Where(u => u.Username.ToLowerInvariant().Equals(UserName.ToLower())) != null;

            return Json(!existsUser, JsonRequestBehavior.AllowGet);

            string y = null;
            var user = y;

            foreach (UserAccount u in users)
            {
                if (!(u.Username.Equals(UserName)))
                { 
                    user = UserName;
                }
            }

            return Json(user == null);


        }
        */

    }
}

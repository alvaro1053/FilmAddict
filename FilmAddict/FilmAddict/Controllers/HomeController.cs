using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MongoDB.Bson;
using MongoDB.Driver;
using FilmAddict.App_Start;
using FilmAddict.Models;

namespace FilmAddict.Controllers
{
    public class HomeController : Controller
    {

       

        public ActionResult Index()
        {
            return View();
        }

    

    }


}
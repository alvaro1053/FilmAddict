using FilmAddict.App_Start;
using FilmAddict.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FilmAddict.Controllers
{
    public class FilmController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<FilmModel> filmCollection;


        public FilmController() {
            dbcontext = new MongoDBContext();
            filmCollection = dbcontext.mongoDatabase.GetCollection<FilmModel>("Film");
        }

        // GET: Film
        public ActionResult Index()
        {
            List<FilmModel> films = filmCollection.AsQueryable<FilmModel>().ToList();

            return View(films);
        }

        public ActionResult Display(String id) {
            var filmId = new ObjectId(id);
            var film = filmCollection.AsQueryable<FilmModel>().SingleOrDefault(x=>x.Id == filmId);

            return View(film);
        }
    }
}
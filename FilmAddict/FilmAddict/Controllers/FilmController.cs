using FilmAddict.App_Start;
using FilmAddict.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
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

        [HttpGet]
        public ActionResult Display(String id) {
            var filmId = new ObjectId(id);
            var film = filmCollection.AsQueryable<FilmModel>().SingleOrDefault(x=>x.Id == filmId);

            ViewBag.Comments = film.critics;

            return View(film);
        }

        public ActionResult Create() {
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult Create(FilmModel film) {

            if (ModelState.IsValid)

                try
                {
                    film.critics = new List<Critics>();
                    filmCollection.InsertOne(film);

                    return RedirectToAction("Index");
                }
                catch
                {

                    return View();

                }
            else
                return View();


        }
        //Add Comment
        [HttpPost]
        public ActionResult AddComment(String id, FilmModel film)
        {//Display film 


            try
            {
                var filter = Builders<FilmModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<FilmModel>.Update.Set("Critics", film.critics);
                var result = filmCollection.UpdateOne(filter, update);

     

                return RedirectToAction("Index");
            }
            catch
            {

                return View();

            }


        }
        public ActionResult Edit(String id) {

            var filmId = new ObjectId(id);
            var film = filmCollection.AsQueryable().SingleOrDefault(x => x.Id == filmId);
            return View(film);

        }
        [HttpPost]
        public ActionResult Edit(String id, FilmModel film)
        {

            try
            {
                var filter = Builders<FilmModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<FilmModel>.Update.Set("title",film.Title).Set("year",film.Year)
                    .Set("duration",film.Duration).Set("country",film.Country)
                    .Set("director",film.Director).Set("trailer",film.Trailer)
                    .Set("synopsis",film.Synopsis).Set("genres",film.Genres);

                var result = filmCollection.UpdateOne(filter,update);
                return RedirectToAction("Index");
            }
            catch { 
                return View();
            }

        }

        public ActionResult Delete(String id) {


            try
            {
                filmCollection.DeleteOne(Builders<FilmModel>.Filter.Eq("_id",ObjectId.Parse(id)));
                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }



    }
}
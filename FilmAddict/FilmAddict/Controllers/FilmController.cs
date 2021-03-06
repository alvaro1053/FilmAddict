﻿using FilmAddict.App_Start;
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
        private IMongoCollection<Billboard> billboardCollection;

        private IMongoCollection<UserAccount> userCollection;

        public FilmController() {
            dbcontext = new MongoDBContext();
            filmCollection = dbcontext.mongoDatabase.GetCollection<FilmModel>("Film");
            billboardCollection = dbcontext.mongoDatabase.GetCollection<Billboard>("Billboard");
        }

        // GET: Film
        public ActionResult Index()
        {
            List<FilmModel> films = filmCollection.AsQueryable<FilmModel>().ToList();
            ViewBag.logueado = Session["Username"];
            return View(films);
        }

        // My films
        public ActionResult MyFilms()
        {
            IList<FilmModel> films = new List<FilmModel>();
            ViewBag.logueado = Session["Username"];
            if (Session["Username"] != null)
            {
                userCollection = dbcontext.mongoDatabase.GetCollection<UserAccount>("User");
                List<UserAccount> users = userCollection.AsQueryable<UserAccount>().ToList();
                foreach(UserAccount u in users)
                {
                    if (u.Username.Equals(Session["Username"].ToString()))
                    {
                        films = u.Films;
                    }
                }
            }

            return View(films);
        }

        [HttpGet]
        public ActionResult Display(String id) {
            var filmId = new ObjectId(id);
            var film = filmCollection.AsQueryable<FilmModel>().SingleOrDefault(x=>x.Id == filmId);
            ViewBag.logueado = Session["Username"];
            ViewBag.Comments = film.critics;
            ViewBag.Genres = film.Genres;
            ViewBag.trailer = film.Trailer;
            ViewBag.film = film;
            var tuple = new Tuple<FilmAddict.Models.FilmModel, FilmAddict.Models.Critics>(film,new Critics());

            return View(tuple);
        }
        //Películas más comentadas
        public ActionResult Stats() {

            List<FilmModel> films = filmCollection.AsQueryable().ToList();
            List<FilmModel> nombre = new List<FilmModel>();
            List<int> comentarios = new List<int>();
            foreach (var a in films) {

                comentarios.Add(a.critics.Count);
                

            }
            foreach (var a in films)
            {
                if (comentarios.Max()==a.critics.Count) {

                    nombre.Add(a);
                }
                
                

            }//Película con mayor comentarios 
            if (nombre.Count!=0) {
                ViewBag.filmMoreComments = nombre.First();
            }
            if (comentarios.Count!=0) { 
            ViewBag.numberComments = comentarios.Max();
            }

            //Pelicula con mayor duración y menor 
            List<FilmModel> duracionMayor = new List<FilmModel>();
            List<FilmModel> duracionMenor = new List<FilmModel>();
            List<int> duraciones = new List<int>();
            foreach (var a in films)
            {

                duraciones.Add(a.Duration);


            }
            foreach (var a in films)
            {
                if (duraciones.Max() == a.Duration)
                {

                    duracionMayor.Add(a);
                }



            }
            foreach (var a in films)
            {
                if (duraciones.Min() == a.Duration)
                {

                    duracionMenor.Add(a);

                }



            }
            if (duracionMayor.Count!=0) {

                ViewBag.filmMoreDuration = duracionMayor.First();
                ViewBag.bestDuration = duraciones.Max();
                ViewBag.worstDuration = duraciones.Min();
                ViewBag.filmLessDuration = duracionMenor.First();
            }
            



            List<GenreStats> u = filmCollection.Aggregate().Unwind<FilmModel, FilmModel>
                (x => x.Genres).Group(x => x.Genres, g => new
                {
                    Count = g.Count()
                }).As<GenreStats>().ToList();

            List<int> datos = new List<int>();
            foreach (var i in u) {
                datos.Add(i.Count);
            }
            List<string> genres = new List<string>();
            foreach (var i in u)
            {
                genres.Add(i.Genre);
            }
            ViewBag.genres = genres;
            ViewBag.datos = datos;
            ViewBag.u = u;
            if (films.Count!=0) {


                var oldFilm = films.Select(x => x.Year).Min();
                ViewBag.oldFilm = oldFilm;
            }
            var billboardWithMoreFilms = billboardCollection.AsQueryable().ToList().OrderByDescending(x => x.films.Count).Take(3);
            ViewBag.billboardWithMoreFilms = billboardWithMoreFilms;

            //Usuario con mas pelis creadas
            userCollection = dbcontext.mongoDatabase.GetCollection<UserAccount>("User");
            List<UserAccount> users = new List<UserAccount>();
            ViewBag.sinUsuarios = userCollection.AsQueryable().ToList().Count();
            if (userCollection.AsQueryable().ToList().Count() > 0) {
                var n = userCollection.AsQueryable<UserAccount>().Max(x => x.Films.Count);
            
            foreach (var i in userCollection.AsQueryable().ToList()) {

                if (n==i.Films.Count) {
                    users.Add(i);
                }

            }
            
            ViewBag.usuarioMasActivo = users.First();
            }

            return View();

        }

        public ActionResult Create() {
            ViewBag.logueado = Session["Username"];
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult Create(FilmModel film) {
            if (Session["Username"] != null)
            {
                if (ModelState.IsValid)

                    try
                    {
                        film.critics = new List<Critics>();
                        List<string> genres = new List<string>();

                        foreach (string i in film.Genres[0].Split(','))
                        {
                            genres.Add(i);

                        }
                        film.Genres = genres.ToArray();



                        if (Session["Username"] != null)
                        {
                            userCollection = dbcontext.mongoDatabase.GetCollection<UserAccount>("User");
                            List<UserAccount> users = userCollection.AsQueryable<UserAccount>().ToList();
                            foreach (UserAccount u in users)
                            {
                                if (u.Username.Equals(Session["Username"].ToString()))
                                {

                                    u.Films.Add(film);

                                    var id = u.UserId.ToString();

                                    var filter = Builders<UserAccount>.Filter.Eq("_id", ObjectId.Parse(id));
                                    var update = Builders<UserAccount>.Update.Set("films", u.Films);

                                    var result = userCollection.UpdateOne(filter, update);

                                }
                            }
                        }

                        filmCollection.InsertOne(film);

                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        var error = e.GetBaseException();
                        return View();

                    }
                else
                    return View();
            }
                return View();


        }
        //Add Comment
        [HttpPost]
        public ActionResult AddComment(String id)
        {
            if (Session["Username"] != null)
            {
                if (ModelState.IsValid)
                    try
                    {
                        Critics c = new Critics();
                  
                        c.Name = Session["Username"].ToString();
                        c.Comment = Request.Form["Item2.Comment"];

                        var filter = Builders<FilmModel>.Filter.Eq("_id", ObjectId.Parse(id));
                        var filmId = new ObjectId(id);
                        Models.FilmModel film = filmCollection.AsQueryable<FilmModel>().SingleOrDefault(x => x.Id == filmId);
                        IList<Critics> l = film.critics;
                        l.Add(c);
                        var update = Builders<FilmModel>.Update.Set("Critics", l);
                        var result = filmCollection.UpdateOne(filter, update);



                        return RedirectToAction("Display/" + id);
                    }

                    catch
                    {

                        return RedirectToAction("Index");

                    }
                else
                    return View();
            }
            return View();


        }
        public ActionResult Edit(String id) {
            ViewBag.logueado = Session["Username"];
            var filmId = new ObjectId(id);
            var film = filmCollection.AsQueryable().SingleOrDefault(x => x.Id == filmId);
            return View(film);

        }
        [HttpPost]
        public ActionResult Edit(String id, FilmModel film)
        {
            if (Session["Username"] != null)
            {
                try
                {
                    var filter = Builders<FilmModel>.Filter.Eq("_id", ObjectId.Parse(id));
                    var update = Builders<FilmModel>.Update.Set("title", film.Title).Set("year", film.Year)
                        .Set("duration", film.Duration).Set("country", film.Country)
                        .Set("director", film.Director).Set("trailer", film.Trailer)
                        .Set("synopsis", film.Synopsis).Set("genres", film.Genres[0].Split(','));

                    var result = filmCollection.UpdateOne(filter, update);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();

        }

        public ActionResult Delete(String id) {
            if (Session["Username"] != null)
            {

                try
                {
                    
                    filmCollection.DeleteOne(Builders<FilmModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
                return View();
        }



    }
}
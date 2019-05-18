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
    public class BillboardController : Controller
    {

        private MongoDBContext dbcontext;
        private IMongoCollection<Billboard> billboardCollection;
        private IMongoCollection<FilmModel> filmCollection;
        

        public BillboardController()
        {
            dbcontext = new MongoDBContext();
            billboardCollection = dbcontext.mongoDatabase.GetCollection<Billboard>("Billboard");
            filmCollection = dbcontext.mongoDatabase.GetCollection<FilmModel>("Film");
        }


        // GET: Billboard
        public ActionResult Index()
        {
            ViewBag.logueado = Session["Username"];
            List<Billboard> billboards = billboardCollection.AsQueryable<Billboard>().ToList();

            return View(billboards);
        }

        // GET: Billboard/Display/5
        public ActionResult Display(String id)
        {
            var billboardId = new ObjectId(id);
            List<FilmModel> films = new List<FilmModel>();
            List<FilmModel> filmsResultantes = new List<FilmModel>();
            var billboard = billboardCollection.AsQueryable<Billboard>().SingleOrDefault(x => x.Id == billboardId);
           
            foreach(string i in billboard.films) {
                var film = filmCollection.AsQueryable().ToList().SingleOrDefault(x => x.Id == new ObjectId(i));
                if (film!=null) {
                    films.Add(film);
                }
            }

            ViewBag.films = films;
            
            foreach (FilmModel i in filmCollection.AsQueryable().ToList())
            {
                if (!billboard.films.Contains(i.Id.ToString())) {
                    filmsResultantes.Add(i);
                }
            }

            ViewBag.logueado = Session["Username"];
            

                ViewBag.allFilms = filmsResultantes;

            return View(billboard);
        }

        // GET: Billboard/Create
        public ActionResult Create()
        {
            ViewBag.logueado = Session["Username"];
            return View();
        }

        // POST: Billboard/Create
        [HttpPost]
        public ActionResult Create(Billboard b)
        {
            if (Session["Username"] != null)
            {
                ViewBag.logueado = Session["Username"];
                if (ModelState.IsValid)
                    try
                    {
                        // TODO: Add insert logic here
                        b.films = new List<string>();
                        billboardCollection.InsertOne(b);
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return View();
                    }
                else
                    return View();
            }
            return View();
        }

        // GET: Billboard/Edit/5
        public ActionResult Edit(String id)
        {
            ViewBag.logueado = Session["Username"];
            var billboardId = new ObjectId(id);
            var billboard = billboardCollection.AsQueryable<Billboard>().SingleOrDefault(x => x.Id == billboardId);

            return View(billboard);
        }

        // POST: Billboard/Edit/5
        [HttpPost]
        public ActionResult Edit(String id, Billboard b)
        {
            if (Session["Username"] != null)
            {
                ViewBag.logueado = Session["Username"];
                try
                {
                    // TODO: Add update logic here
                    var filter = Builders<Billboard>.Filter.Eq("_id", ObjectId.Parse(id));
                    var update = Builders<Billboard>.Update.Set("name", b.Name).Set("location", b.Location)
                        .Set("price", b.Price);


                    var result = billboardCollection.UpdateOne(filter, update);
                    return RedirectToAction("Index");

                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Billboard/Delete/5
        public ActionResult Delete(String id)
        {
            if (Session["Username"] != null)
            {
                ViewBag.logueado = Session["Username"];
                try
                {
                    billboardCollection.DeleteOne(Builders<Billboard>.Filter.Eq("_id", ObjectId.Parse(id)));
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        //Add Film
        [HttpPost]
        public ActionResult AddFilm(String id)
        {
            ViewBag.logueado = Session["Username"];
            if (Session["Username"] != null)
            {
                if (ModelState.IsValid)
                    try
                    {
                        var idFilm = Request.Form["select"];//id de la pelicula q quiero añadir



                        var filter = Builders<Billboard>.Filter.Eq("_id", ObjectId.Parse(id));//cojo el billboard actual
                        var bId = new ObjectId(id);
                        var b = billboardCollection.AsQueryable<Billboard>().SingleOrDefault(x => x.Id == bId);
                        IList<string> l = b.films;


                        foreach (string i in idFilm.Split(','))
                        {
                            l.Add(i);
                        }


                        var update = Builders<Billboard>.Update.Set("films", l);
                        var result = billboardCollection.UpdateOne(filter, update);

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
     

    }
}

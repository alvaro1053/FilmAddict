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

            List<Billboard> billboards = billboardCollection.AsQueryable<Billboard>().ToList();

            return View(billboards);
        }

        // GET: Billboard/Display/5
        public ActionResult Display(String id)
        {
            var billboardId = new ObjectId(id);
            List<FilmModel> films = new List<FilmModel>();
            var billboard = billboardCollection.AsQueryable<Billboard>().SingleOrDefault(x => x.Id == billboardId);
            
            foreach(string i in billboard.films) {
                var film = filmCollection.AsQueryable().ToList().SingleOrDefault(x => x.Id == new ObjectId(i));
                films.Add(film);
            }

            ViewBag.films = films;
            ViewBag.allFilms = filmCollection.AsQueryable().ToList();
            return View(billboard);
        }

        // GET: Billboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Billboard/Create
        [HttpPost]
        public ActionResult Create(Billboard b)
        {
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

        // GET: Billboard/Edit/5
        public ActionResult Edit(String id)
        {
            var billboardId = new ObjectId(id);
            var billboard = billboardCollection.AsQueryable<Billboard>().SingleOrDefault(x => x.Id == billboardId);

            return View(billboard);
        }

        // POST: Billboard/Edit/5
        [HttpPost]
        public ActionResult Edit(String id, Billboard b)
        {
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

        // GET: Billboard/Delete/5
        public ActionResult Delete(String id)
        {
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

        //Add Film
        [HttpPost]
        public ActionResult AddFilm(String id)//List<String>¿?
        {

            if (ModelState.IsValid)
                try
                {
                    var idFilm= Request.Form["select"];//id de la pelicula q quiero añadir

                    

                    var filter = Builders<Billboard>.Filter.Eq("_id", ObjectId.Parse(id));//cojo el billboard actual
                    var bId = new ObjectId(id);
                    var b = billboardCollection.AsQueryable<Billboard>().SingleOrDefault(x => x.Id == bId);
                    IList<string> l = b.films;
                    //Le voy a añadir a la lista de films solo las ids, si no puedo obtener los valores de sus atributos ->meto el objeto film para tener los atributos-> con la id 
                    //en el display busco por esa id y muetro el objeto.

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

        /*     // POST: Billboard/Delete/5
             [HttpPost]
             public ActionResult Delete(int id, FormCollection collection)
             {
                 try
                 {
                     // TODO: Add delete logic here

                     return RedirectToAction("Index");
                 }
                 catch
                 {
                     return View();
                 }
             }
         }*/
    }
}

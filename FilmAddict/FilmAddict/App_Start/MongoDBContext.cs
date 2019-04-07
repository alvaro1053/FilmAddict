using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmAddict.App_Start
{
    public class MongoDBContext
    {
        public  IMongoDatabase mongoDatabase;


        public  MongoDBContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            mongoDatabase = client.GetDatabase("FilmAddict");
            
        }
    }
}
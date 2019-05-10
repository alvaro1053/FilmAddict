using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmAddict.Models
{
    public class Billboard
    {

        [BsonId]
        public ObjectId Id
        {
            get; set;
        }

        [BsonElement("name")]
        public String Name { get; set; }

        [BsonElement("location")]
        public String Location { get; set; }

        [BsonElement("price")]
        public Double Price { get; set; }

        [BsonElement("films")]
        public IList<string> films { get; set; }

        //Lo paso a String , pero si quiero añadir film object lo paso a filmModel
    }
}
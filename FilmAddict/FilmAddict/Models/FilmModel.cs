using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmAddict.Models
{
    public class FilmModel
    {

        [BsonId]
        public ObjectId Id {
            get;set;
        }
        [BsonElement("title")]
        public String Title { get; set; }

        [BsonElement("year")]
        public String Year { get; set; }

        [BsonElement("duration")]
        public String Duration { get; set; }

        [BsonElement("country")]
        public String Country { get; set; }

        [BsonElement("director")]
        public String Director { get; set; }


        [BsonElement("trailer")]
        public String Trailer { get; set; }


        [BsonElement("synopsis")]
        public String Synopsis { get; set; }


        [BsonElement("genres")]
        public String [] Genres { get; set; }

        [BsonElement("Critics")]
        public Critics [] critics{ get; set; }

    }

    public class Critics
    {


        [BsonElement("name")]
        public String Name { get; set; }


        [BsonElement("comment")]
        public String Comment { get; set; }

    }
}
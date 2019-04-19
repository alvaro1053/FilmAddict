using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public IList <Critics> critics{ get; set; }

   
    }

    public class Critics
    {


        [BsonElement("name")]
        [Required(ErrorMessage = "The name is required")]
        [MinLength(1, ErrorMessage = "can not be empty ")]
        public String Name { get; set; }

        [Required(ErrorMessage = "The comment is required")]
        [MinLength(1, ErrorMessage = "can not be empty ")]
        [BsonElement("comment")]
        public String Comment { get; set; }

    }

    


}
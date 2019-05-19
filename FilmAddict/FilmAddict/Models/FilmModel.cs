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
        [Required(ErrorMessage = "The title of the film is obligatory")]
        [MinLength(1, ErrorMessage = "The title of the film must have at least 1 character")]
        public String Title { get; set; }

        [BsonElement("year")]
        [Required(ErrorMessage = "The year of the film is obligatory")]
        [Range(1800, 2019, ErrorMessage = "The year must be between 1800 and 2019")]
        public int Year { get; set; }

        [BsonElement("duration")]
        [Required(ErrorMessage = "The duration of the film is obligatory")]
        [Range(0, 1000, ErrorMessage = "The duration should be a number between 0 and 1000 minutes")]
        public int Duration { get; set; }

        [BsonElement("country")]
        [Required(ErrorMessage = "The country of the film is obligatory")]
        [MinLength(1, ErrorMessage = "The country of the film must have at least 1 character")]
        public String Country { get; set; }

        [BsonElement("director")]
        [Required(ErrorMessage = "The director of the film is obligatory")]
        [MinLength(1, ErrorMessage = "The director of the film must have at least 1 character")]
        public String Director { get; set; }


        [BsonElement("trailer")]
        [RegularExpression("https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)")]
        public String Trailer { get; set; }


        [BsonElement("synopsis")]
        [Required(ErrorMessage = "The synopsis of the film is obligatory")]
        [MinLength(10, ErrorMessage = "The synopsis of the film must have at least 10 characters")]
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
    public class GenreStats
    {
        [BsonElement("_id")]
        public string Genre { get; set; }
        public int Count { get; set; }
    }

}
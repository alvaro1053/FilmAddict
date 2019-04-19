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
        [Required(ErrorMessage = "El título de la película es obligatorio")]
        [MinLength(1, ErrorMessage = "El título de la película debe tener al menos 1 caracter")]
        public String Title { get; set; }

        [BsonElement("year")]
        [Required(ErrorMessage = "El año de la película es obligatorio")]
        [Range(1800, 2019, ErrorMessage = "El año debe ser entre 1800 y 2019")]
        public int Year { get; set; }

        [BsonElement("duration")]
        [Required(ErrorMessage = "La duración de la película es obligatorio")]
        [Range(0, 1000, ErrorMessage = "La duración debe ser un número entre 0 y 1000 minutos")]
        public int Duration { get; set; }

        [BsonElement("country")]
        [Required(ErrorMessage = "El país de la película es obligatorio")]
        [MinLength(1, ErrorMessage = "El título de la película debe tener al menos 1 caracter")]
        public String Country { get; set; }

        [BsonElement("director")]
        [Required(ErrorMessage = "El director de la película es obligatorio")]
        [MinLength(1, ErrorMessage = "El director de la película debe tener al menos 1 caracter")]
        public String Director { get; set; }


        [BsonElement("trailer")]
        [RegularExpression("https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)")]
        public String Trailer { get; set; }


        [BsonElement("synopsis")]
        [Required(ErrorMessage = "La synopsis de la película es obligatorio")]
        [MinLength(10, ErrorMessage = "La synopsis de la película debe tener al menos 10 caracteres")]
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
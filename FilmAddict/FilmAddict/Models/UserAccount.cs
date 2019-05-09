using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FilmAddict.Models
{
    public class UserAccount
    {

        [BsonId]
        public ObjectId UserId{ get; set; }

        [BsonElement("username")]
        [Required(ErrorMessage ="Username is required.")]
        public string Username { get; set; }

        [BsonElement("password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BsonElement("confirmPassword")]
        [Required(ErrorMessage = "Please confirm password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }
}

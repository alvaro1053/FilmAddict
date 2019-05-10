using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

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
        [Compare("Password",ErrorMessage ="Confirm password and password do not match!")]
        public string ConfirmPassword { get; set; }


    }
}

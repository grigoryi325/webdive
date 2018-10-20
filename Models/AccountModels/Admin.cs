using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jQuery.Models.AccountModels
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        public string LoginAdmin { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role => "admin";
    }
}
using jQuery.Models.ForumModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jQuery.Models.AccountModels
{
    public class User
    {
        private readonly string RoleUser = "user";

        public int Id { get; set; }

        [Required(ErrorMessage = "введите логин")]
        public string LoginUser { get; set; }

        [Required(ErrorMessage = "введите свой адрес электронной почты")]
        [RegularExpression(".+\\@.+\\..+",
            ErrorMessage = "введите правильно почтовый адрес")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role
        {
            get
            {
                return RoleUser;
            }
            set
            {
                value = RoleUser;
            }
        }
    }
}
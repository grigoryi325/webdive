using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace jQuery.Models
{
    public class Admin
    {
        protected string Aname = "Admin";
        protected string Apass = "badrhari";
        public bool checkName = false;
        public bool checkPass = false;

        public string AdminName {
            get
            {             
                return Aname;
            }
            set
            {
                if (value == Aname)
                {
                    checkName = true;
                }
            }
        }

        [Required]
        [DataType(DataType.Password)]
        public string Password {
            get
            {
                return Apass;
            }
            set
            {
                if (value == Apass)
                {
                    checkPass = true;
                }
            }
        }       
    }
}
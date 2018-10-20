using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Language_Name { get; set; }

        public ICollection<Thema> Themas { get; set; }
        public Language()
        {
            Themas = new List<Thema>();
        }
    }
}
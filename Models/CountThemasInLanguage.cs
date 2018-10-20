using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models
{
    public class CountThemasInLanguage
    {
        public int IdLanguage { get; set; }
        public string Language { get; set; }
        public int CountThemas { get; set; }

        public CountThemasInLanguage(int id, string language, int countthemas)
        {
            IdLanguage = id;
            Language = language;
            CountThemas = countthemas;
        }
    }
}
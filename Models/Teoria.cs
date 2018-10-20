using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models
{
    public class Teoria
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string CkTeoria { get; set; }

        public int LanguageId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models
{
    public class Check
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CkTeoria { get; set; }
        public string CkTest { get; set; }
        public string CkExplanation { get; set; }
        public string CkAnswer { get; set; }
        public string ControlCheck { get; set; }

        public int LanguageId { get; set; }
    }
}
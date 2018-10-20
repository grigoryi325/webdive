using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models.NewsModels
{
    public class Message
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
    }
}
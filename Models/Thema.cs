using jQuery.Models.ForumModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models
{
    public class Thema
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? LanguageId { get; set; }
        public Language Language { get; set; }

        public ICollection<AskQuestion> AskQuestions { get; set; }
        public Thema()
        {
            AskQuestions = new List<AskQuestion>();
        }
    }
}
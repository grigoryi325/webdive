using jQuery.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models.ForumModels
{
    public class AskQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }

        public int? ThemaId { get; set; }
        public Thema Thema { get; set; }

        public ICollection<Talking> Talkings { get; set; }
        public AskQuestion()
        {
            Talkings = new List<Talking>();
        }
    }
}
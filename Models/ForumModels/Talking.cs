using jQuery.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models.ForumModels
{
    public class Talking
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string DeleteText { get; set; }
        public string Status { get; set; }

        public int? AskQuestionId { get; set; }
        public AskQuestion AskQuestion { get; set; }

        public int UserId { get; set; }
        public string LoginUser { get; set; }
        public string Role { get; set; }

        public DateTime DateTime { get; set; }

    }
}
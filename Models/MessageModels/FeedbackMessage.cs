using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace jQuery.Models.MessageModels
{
    public class FeedbackMessage
    {
        public int Id { get; set; }
        public string EmailUser { get; set; }
        public string Thema { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public DateTime DateTimeUser { get; set; }

        public string AdminEmail { get; set; }
        public string ADescription { get; set; }
        public string AStatus { get; set; }

        public DateTime DateTimeAdmin { get; set; }

        public int? CategoryMessageId { get; set; }
        public CategoryMessage CategoryMessage { get; set; }
    }
}
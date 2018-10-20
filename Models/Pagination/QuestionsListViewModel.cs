using jQuery.Models.ForumModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models.Pagination
{
    public class QuestionsListViewModel
    {
        public IEnumerable<AskQuestion> AskQuestions { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
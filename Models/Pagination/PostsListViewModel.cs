using jQuery.Models.ForumModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models.Pagination
{
    public class PostsListViewModel
    {
        public IEnumerable<Talking> Talkings { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
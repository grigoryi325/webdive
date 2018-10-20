using jQuery.Models.AccountModels;
using jQuery.Models.ForumModels;
using jQuery.Models.MessageModels;
using jQuery.Models.NewsModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace jQuery.Models
{
    public class ThemaContext: DbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Thema> Themas { get; set; }
        public DbSet<Teoria> Teorias { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Explanation> Explanations { get; set; }

        public DbSet<Check> Cheks { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<FeedbackMessage> FeedbackMessages { get; set; }
        public DbSet<CategoryMessage> CategoryMessages { get; set; }

        public DbSet<Message> Messages { get; set; }


        public DbSet<AskQuestion> AskQuestions { get; set; }
        public DbSet<Talking> Talkings { get; set; }
    }
}
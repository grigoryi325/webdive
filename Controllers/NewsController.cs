using jQuery.Models;
using jQuery.Models.NewsModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQuery.Controllers
{
    public class NewsController : Controller
    {
        private ThemaContext db = new ThemaContext();

        // Страница всех опубликованных новостей
        public ActionResult Index()
        {
            return View(db.Messages.OrderByDescending(x => x.Id).ToList());
        }

        //страница где находятся все не опубликованные новости, тоесть те новости которые в разработке
        public ActionResult ListNews()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    return View(db.Messages.OrderByDescending(x => x.Id).ToList());
                }
                else
                {
                    return RedirectToAction("ErrorAccess", "Error");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
        }

        //метод публикации новости
        public ActionResult PublishedNews(int id)
        {
            Message message = db.Messages.Where(x => x.Id == id).FirstOrDefault();
            message.Status = "Published";
            db.SaveChanges();
            return RedirectToAction("ListNews");
        }
        //метод отзыва новости к редактированию при этом эта новость не видна на главной странице новостей
        public ActionResult CallAwayNews(int id)
        {
            Message message = db.Messages.Where(x => x.Id == id).FirstOrDefault();
            message.Status = "Editing";
            db.SaveChanges();
            return RedirectToAction("ListNews");
        }

        //создание новости
        public ActionResult CreateNews()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("ErrorAccess", "Error");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateNews(Message message)
        {
            Message news_message = new Message
            {
                Description = message.Description,
                DateTime = DateTime.Now,
                Status = "Preview"
            };

            db.Messages.Add(news_message);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        //редактирование новости
        public ActionResult EditNews(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    ViewBag.news = db.Messages.Where(x => x.Id == id).FirstOrDefault();
                    return View();
                }
                else
                {
                    return RedirectToAction("ErrorAccess", "Error");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditNews(Message message)
        {
            db.Entry(message).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ListNews");
        }
      
        //удаление новости
        public ActionResult DeleteNews(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    Message message = db.Messages.Where(x => x.Id == id).FirstOrDefault();
                    db.Messages.Remove(message);
                    db.SaveChanges();
                    return RedirectToAction("ListNews");
                }
                else
                {
                    return RedirectToAction("ErrorAccess", "Error");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jQuery.Models;
using jQuery.Models.MessageModels;

namespace jQuery.Controllers
{
    public class HomeController : Controller
    {
        private ThemaContext db = new ThemaContext();

        public ActionResult Index(int id = 0)
        {
            ViewBag.id = id;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            //подсчитываем количество тем в мовах програмирование, для того, чтобы не выводить в меню те языки в которых нету тем
            List<CountThemasInLanguage> countThemas = new List<CountThemasInLanguage>();
            int i = 1;
            foreach (var item in db.Languages.ToList())
            {
                int count_themas = db.Themas.Where(x => x.LanguageId == i).Count();
                countThemas.Add(new CountThemasInLanguage(i, item.Language_Name, count_themas));
                i++;
            }
            ViewBag.CountThemas = countThemas;

            //подсчитываем количество новых вопросов в курсах, это те вопросы на которые еще не дали ответов
            List<int> mass = new List<int>();
            //пробегаем по темам к поточному курсу
            foreach (var item in db.Languages.ToList())
            {
                int count = 0;
                foreach (var b in db.Themas.Where(x => x.LanguageId == item.Id).ToList())
                {
                    //пробегаем по вопросам соответствующих тем
                    foreach (var q in db.AskQuestions.Where(x => x.ThemaId == b.Id).ToList())
                    {
                        //подсчитываем количество ответов в соответствующих вопросах
                        var num = db.Talkings.Where(x => x.AskQuestionId == q.Id).Count();
                        //если на вопрос еще не дали неединого ответа то увеличиваем переменную count на единицу
                        if (num == 0)
                        {
                            count++;
                        }
                    }
                }
                //записываем результат в масив
                mass.Add(count);
            }
            //перебераем результующий массив и сумируем его значения
            int sum = 0;
            for (int j = 0; j < mass.Count(); j++)
            {
                sum += mass[j];
            }
            if (sum != 0)
            {
                ViewBag.ResultCount = sum;
            }
            else
            {
                ViewBag.ResultCount = null;
            }

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult ListThemas(int id = 0)
        {
            if (id != 0)
            {
                ViewBag.language = db.Languages.Where(x => x.Id == id).FirstOrDefault();
                ViewBag.Check = db.Cheks.Where(x => x.LanguageId == id).ToList();
            }
            return PartialView();
        }

        public ActionResult Contact()
        {
            SelectList categoryMessages = new SelectList(db.CategoryMessages, "Id", "NameCategory");
            ViewBag.CategoriesMessages = categoryMessages;

            return View();
        }

        [HttpPost]
        public ActionResult Contact(FeedbackMessage feedbackMessage)
        {
            try
            {
                var Email = Session["Email"].ToString();
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    db.FeedbackMessages.Add(feedbackMessage);
                    db.SaveChanges();
                    return RedirectToAction("Contact", "Home");
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

        public ActionResult Teoria(int id)
        {
            ViewBag.Nam = db.Themas.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Teoria = db.Teorias.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Check = db.Cheks.Where(x => x.Id == id).FirstOrDefault();
            return View();
        }

        public ActionResult Test(int id)
        {
            ViewBag.Thema = db.Themas.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Test = db.Tests.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Answer = db.Answers.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Check = db.Cheks.Where(x => x.Id == id).FirstOrDefault();
            return View();
        }

        public ActionResult Explanation(int id)
        {
            ViewBag.Nam = db.Themas.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Explanation = db.Explanations.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Check = db.Cheks.Where(x => x.Id == id).FirstOrDefault();
            return View(); 
        }
    }
}

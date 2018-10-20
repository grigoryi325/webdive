using jQuery.Models;
using jQuery.Models.ForumModels;
using jQuery.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQuery.Controllers
{
    public class ForumController : Controller
    {
        private ThemaContext db = new ThemaContext();

        // используем тот же алгоритм что и в Home контроллере, экшн Menu, тоесть выводим список только тех курсов, 
        // в которых создано не менее одной темы
        public ActionResult Index()
        {
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
            ViewBag.lenght = mass.Count();
            ViewBag.Count = mass;

            return View(db.Languages);
        }

        //список тем в курсе
        public ActionResult ListThemas(int id = 0)
        {
            if (id != 0)
            {
                ViewBag.language = db.Languages.Where(x => x.Id == id).FirstOrDefault();
                ViewBag.Check = db.Cheks.Where(x => x.LanguageId == id).ToList();             

                //подсчитываем количество новых вопросов в темах курса, это те вопросы на которые еще не дали ответов
                List<int> mass = new List<int>();
                //пробегаем по темам к поточному курсу
                foreach (var b in db.Themas.Where(x => x.LanguageId == id).ToList())
                {
                    //пробегаем по вопросам соответствующих тем
                    int count = 0;
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
                    //записываем результат в масив
                    mass.Add(count);              
                }
                ViewBag.lenght = mass.Count();
                ViewBag.Count = mass;
            }

            return View();
        }

        //создаем вопрос в конкретной теме
        public ActionResult CreateQestion(int id)
        {
            ViewBag.IdThemas = id;
            return View();
        }

        [HttpPost]
        public ActionResult CreateQestion(AskQuestion askQuestion)
        {
            db.AskQuestions.Add(askQuestion);
            db.SaveChanges();
            //редиректимся на страницу где был создан вопрос
            return RedirectToAction("AsqQuestion", new { id = askQuestion.ThemaId });
        }

        public int PageSize = 2;
        //выводим список тем которые относятся к конкретному вопросу
        public ActionResult AsqQuestion(int id, int page = 1)
        {
            QuestionsListViewModel questionsPage = new QuestionsListViewModel
            {

                AskQuestions = db.AskQuestions.Where(x => x.ThemaId == id).ToList()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = page,
                            ItemsPerPage = PageSize,
                            TotalItems = db.AskQuestions.Where(x => x.ThemaId == id).Count()
                        }
            };

            //подсчитываем количесто ответов у вопросах
            List<int> mass = new List<int>();
            foreach (var b in db.AskQuestions.Where(x => x.ThemaId == id).ToList())
            {              
                mass.Add(db.Talkings.Where(x => x.AskQuestionId == b.Id).Count());
            }
            mass.Reverse();
            ViewBag.lenght = mass.Count();
            ViewBag.Count = mass;

            ViewBag.ThemaId = id;
            ViewBag.ThemePage = db.Themas.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.QuestionsCount = db.AskQuestions.Where(x => x.ThemaId == id).Count();
            Session["page"] = page;
            return View(questionsPage);
        }

        //редактирование вопроса, редактируес сразу как сам вопрос так и его описание
        public ActionResult EditAsqQuestion(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    return View(db.AskQuestions.Where(x => x.Id == id).FirstOrDefault());
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

        [HttpPost]
        public ActionResult EditAsqQuestion(AskQuestion askQuestion)
        {
            db.Entry(askQuestion).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AsqQuestion", new { id = askQuestion.ThemaId, page = Session["page"] });
        }

        /*---------------страница описания вопроса и постов-----------------*/
        //описание к вопросу
        [ChildActionOnly]
        public ActionResult DescriptionQuestion(int id)
        {
            ViewBag.DescriptionQuestion = db.AskQuestions.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.IdQuestion = id;
            return PartialView();
        }
        //сохранения ответ
        [HttpPost, ValidateInput(false)]
        public ActionResult SavePost(int id, Talking talking)
        {
            db.Talkings.Add(talking);
            db.SaveChanges();
            return RedirectToAction("Discussion", new { id });
        }

        public int PageSize_discussion = 2;

        //выводим список ответов на вопрос
        public ActionResult Discussion(int id, int page = 1)
        {
            PostsListViewModel posts = new PostsListViewModel
            {
                Talkings = db.Talkings.Where(x => x.AskQuestionId == id).ToList()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * PageSize_discussion)
                .Take(PageSize_discussion),
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = page,
                            ItemsPerPage = PageSize_discussion,
                            TotalItems = db.Talkings.Where(x => x.AskQuestionId == id).Count()
                        }
            };

            ViewBag.DescriptionQuestion = db.AskQuestions.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.IdQuestion = id;
            Session["page_discussion"] = page;
            return View(posts);
        }

        /*-------------------------------------------------------------------*/

        //редактирование сообщения
        public ActionResult EditMessage(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    return View(db.Talkings.Where(x => x.Id == id).FirstOrDefault());
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
        public ActionResult EditMessage(int page, Talking talking)
        {

            db.Entry(talking).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Discussion", new { id = page, page = Session["page_discussion"] });
        }

        //удаление сообщения
        //если это делает пользователь то он просто удаляет свое сообщение из базы, 
        //администратор может как удалить свое сообщение так и пользователя
        //если он удаляет свое сообщение то оно удаляется из базы, если пользователя то оно ьлокируеться и изменяэться текст
        public ActionResult DelMessage(int id, int id_page)
        {
            var role = Session["Role"].ToString();
            var user_id = Session["Id"].ToString();

            Talking talking = db.Talkings.Where(x => x.Id == id).FirstOrDefault();

            if((role == "user") && (talking.UserId.ToString() == user_id))
            {
                db.Talkings.Remove(talking);
                db.SaveChanges();
            } else if ((role == "admin") && (talking.Role != "admin"))
            {
                talking.DeleteText = talking.Text;
                talking.Text = "Сообщение было удалено, так как имело нецензурную лексику или оскорбления в адрес пользователей сайта.";
                talking.Status = "blocked";
                db.SaveChanges();
            } else if ((role == "admin") && (talking.Role == "admin"))
            {
                db.Talkings.Remove(talking);
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
            return RedirectToAction("Discussion", new {id = id_page, page = Session["page_discussion"] });
        }

        public ActionResult Regulations()
        {
            return View();
        }

        /*----------------------------------- Персональна страница пользователя флрума---------------------------------------------
         -------------------------------------------------------------------------------------------------------------------------*/

        public int UPageSize = 4;
        //выводим список тем которые относятся к конкретному пользователю
        public ActionResult UserAsqQuestion(int id, int page = 1)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    QuestionsListViewModel questionsPage = new QuestionsListViewModel
                    {

                        AskQuestions = db.AskQuestions.Where(x => x.UserId == id).ToList()
                        .OrderByDescending(x => x.Id)
                        .Skip((page - 1) * UPageSize)
                        .Take(UPageSize),
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = page,
                            ItemsPerPage = UPageSize,
                            TotalItems = db.AskQuestions.Where(x => x.UserId == id).Count()
                        }
                    };

                    //подсчитываем количесто ответов у вопросах
                    List<int> mass = new List<int>();
                    foreach (var b in db.AskQuestions.Where(x => x.UserId == id).ToList())
                    {
                        mass.Add(db.Talkings.Where(x => x.AskQuestionId == b.Id).Count());
                    }

                    mass.Reverse();
                    ViewBag.lenght = mass.Count();
                    ViewBag.Count = mass;

                    ViewBag.ThemaId = id;
                    ViewBag.ThemePage = db.Themas.Where(x => x.Id == id).FirstOrDefault();
                    ViewBag.QuestionsCount = db.AskQuestions.Where(x => x.ThemaId == id).Count();
                    Session["user_page"] = page;
                    return View(questionsPage);
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

        //описание к вопросу
        [ChildActionOnly]
        public ActionResult UserDescriptionQuestion(int id)
        {
            ViewBag.DescriptionQuestion = db.AskQuestions.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.IdQuestion = id;
            return PartialView();
        }
        //сохранения ответ
        [HttpPost, ValidateInput(false)]
        public ActionResult UserSavePost(int id, Talking talking)
        {
            db.Talkings.Add(talking);
            db.SaveChanges();
            return RedirectToAction("UserDiscussion", new { id });
        }

        public int UPageSize_discussion = 2;

        //выводим список ответов на вопрос
        public ActionResult UserDiscussion(int id, int page = 1)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    PostsListViewModel posts = new PostsListViewModel
                    {
                        Talkings = db.Talkings.Where(x => x.AskQuestionId == id).ToList()
                        .OrderByDescending(x => x.Id)
                        .Skip((page - 1) * UPageSize_discussion)
                        .Take(UPageSize_discussion),
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = page,
                            ItemsPerPage = UPageSize_discussion,
                            TotalItems = db.Talkings.Where(x => x.AskQuestionId == id).Count()
                        }
                    };

                    ViewBag.DescriptionQuestion = db.AskQuestions.Where(x => x.Id == id).FirstOrDefault();
                    ViewBag.IdQuestion = id;
                    Session["user_page_discussion"] = page;
                    return View(posts);
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

        //редактирование вопроса, редактируем сразу как сам вопрос так и его описание
        public ActionResult UserEditAsqQuestion(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    return View(db.AskQuestions.Where(x => x.Id == id).FirstOrDefault());
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

        [HttpPost]
        public ActionResult UserEditAsqQuestion(AskQuestion askQuestion)
        {
            db.Entry(askQuestion).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UserAsqQuestion", new { id = Session["Id"], page = Session["user_page"] });
        }

        //редактирование сообщения
        public ActionResult UserEditMessage(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    return View(db.Talkings.Where(x => x.Id == id).FirstOrDefault());
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
        public ActionResult UserEditMessage(int page, Talking talking)
        {

            db.Entry(talking).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("UserDiscussion", new { id = page, page = Session["user_page_discussion"] });
        }

        //удаление сообщения
        //если это делает пользователь то он просто удаляет свое сообщение из базы, 
        //администратор может как удалить свое сообщение так и пользователя
        //если он удаляет свое сообщение то оно удаляется из базы, если пользователя то оно ьлокируеться и изменяэться текст
        public ActionResult UserDelMessage(int id, int id_page)
        {
            var role = Session["Role"].ToString();
            var user_id = Session["Id"].ToString();

            Talking talking = db.Talkings.Where(x => x.Id == id).FirstOrDefault();

            if ((role == "user") && (talking.UserId.ToString() == user_id))
            {
                db.Talkings.Remove(talking);
                db.SaveChanges();
            }        
            else
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
            return RedirectToAction("UserDiscussion", new { id = id_page, page = Session["user_page_discussion"] });
        }
    }
}
using jQuery.Models;
using jQuery.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQuery.Controllers
{
    public class AdminController : Controller
    {
        private ThemaContext db = new ThemaContext();
        // GET: Admin
        public ActionResult Index()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
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

                    ViewBag.CountNewMessae = db.FeedbackMessages.Where(x => x.Status == "yes").Count();
                    IEnumerable<Language> resultIndex = db.Database.SqlQuery<Language>(@"SELECT * FROM Languages"); 
                    return View(resultIndex);
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

        public ActionResult Login()
        {
              return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            try
            {
                var adm = db.Admins.Single(u => u.LoginAdmin == admin.LoginAdmin && u.Password == admin.Password);
                Session["Id"] = adm.Id.ToString();
                Session["Login"] = adm.LoginAdmin;
                Session["Email"] = adm.Email;
                Session["Role"] = adm.Role;
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException)
            {
                ViewBag.ErrorLogin = "не верный логин или пароль";
                return View();
            }
        }

        //редактирование данных аккаунта
        public ActionResult EditAccount(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    return View(db.Admins.Where(x => x.Id == id).FirstOrDefault());
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
        public ActionResult EditAccount(int id, string LoginAdmin, string Email, string OldPassword, string NewPassword, string ReNewPassword)
        {
            //ищем пользователя в системе
            var usr = db.Admins.Where(x => x.Id == id).FirstOrDefault();
            //переверяем введенные данные с теми что у базе
            if (usr.Password != OldPassword)
            {
                return RedirectToAction("ErrorConfirmOldPassword", "Error");
            }
            else if (NewPassword != ReNewPassword)
            {
                return RedirectToAction("ErrorConfirmNewPassword", "Error");
            }
            else
            {
                try
                {
                    //то меняем старые даные пользователя на новые
                    Admin admin = db.Admins.Where(x => x.Id == id).FirstOrDefault();
                    admin.LoginAdmin = LoginAdmin;
                    admin.Email = Email;
                    admin.Password = NewPassword;
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return RedirectToAction("ErrorEditAccount", "Error");
                }
            }
        }

        //выход
        public ActionResult DelSession()
        {
            if (Session["Id"] != null)
            {
                Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("ErrorAccess", "Error");
            }
        }

        public ActionResult CreateThema()
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    SelectList language = new SelectList(db.Languages, "Id", "Language_Name");
                    ViewBag.Language = language;
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

        [HttpPost]
        public ActionResult CteateThema(Thema thema, Check check, Language language)
        {
                db.Themas.Add(thema);
                db.Cheks.Add(check);

                Teoria teoria = new Teoria { CkTeoria = "No", LanguageId = language.Id };
                db.Teorias.Add(teoria);

                Test test = new Test { CkTest = "No", LanguageId = language.Id };
                db.Tests.Add(test);

                Answer answer = new Answer { CkAnswer = "No", LanguageId = language.Id };
                db.Answers.Add(answer);

                Explanation explanation = new Explanation { CkExplanation = "No", LanguageId = language.Id };
                db.Explanations.Add(explanation);

                Check create_themas = new Check { LanguageId = language.Id };

                db.SaveChanges();
                return RedirectToAction("CreateTeoria", "Admin", new {thema.Id} );
        }

        public ActionResult EditThema(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    return View(db.Themas.Where(x => x.Id == id).FirstOrDefault());
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
        public ActionResult EditThema(int id, Thema thema, Check check)
        {
            db.Entry(thema).State = EntityState.Modified;

            Check CkTeor = db.Cheks.Where(x => x.Id == id).FirstOrDefault();
            CkTeor.Name = check.Name;

            db.Entry(CkTeor).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult CreateTeoria(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    ViewBag.IdTeoria = id;
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
        public ActionResult CreateTeoria(int id, Teoria teoria, Check check)
        {
            Teoria Teor = db.Teorias.Where(x => x.Id == id).FirstOrDefault();
            Teor.Description = teoria.Description;
            Teor.CkTeoria = teoria.CkTeoria;

            Check CkTeor = db.Cheks.Where(x => x.Id == id).FirstOrDefault();
            CkTeor.CkTeoria = check.CkTeoria;

            db.SaveChanges();
            return RedirectToAction("CreateTest", "Admin", new { teoria.Id });
        }

        public ActionResult EditTeoria(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    ViewBag.teoria = db.Teorias.Where(x => x.Id == id).FirstOrDefault();
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
        public ActionResult EditTeoria(Teoria teoria)
        {
            db.Entry(teoria).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult CreateTest(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    ViewBag.IdTest = id;
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
        public ActionResult CreateTest(int id, Test test, Answer answer, Check check)
        {
                Check CkTes = db.Cheks.Where(x => x.Id == id).FirstOrDefault();
                CkTes.CkTest = check.CkTest;

                Test Tes = db.Tests.Where(x => x.Id == id).FirstOrDefault();
                Tes.One = test.One;
                Tes.Two = test.Two;
                Tes.Three = test.Three;
                Tes.Four = test.Four;
                Tes.Five = test.Five;
                Tes.Six = test.Six;
                Tes.Seven = test.Seven;
                Tes.Eight = test.Eight;
                Tes.Nine = test.Nine;
                Tes.Ten = test.Ten;
                Tes.CkTest = test.CkTest;

                Answer Answ = db.Answers.Where(x => x.Id == id).FirstOrDefault();
                Answ.AnswerOne = answer.AnswerOne;
                Answ.AnswerTwo = answer.AnswerTwo;
                Answ.AnswerThree = answer.AnswerThree;
                Answ.AnswerFour = answer.AnswerFour;
                Answ.AnswerFive = answer.AnswerFive;
                Answ.AnswerSix = answer.AnswerSix;
                Answ.AnswerSeven = answer.AnswerSeven;
                Answ.AnswerEight = answer.AnswerEight;
                Answ.AnswerNine = answer.AnswerNine;
                Answ.AnswerTen = answer.AnswerTen;

                db.SaveChanges();
                return RedirectToAction("CreateExplanation", "Admin", new { test.Id });
        }

        public ActionResult EditTest(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    ViewBag.Test = db.Tests.Where(x => x.Id == id).FirstOrDefault();
                    ViewBag.Answer = db.Answers.Where(x => x.Id == id).FirstOrDefault();
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
        public ActionResult EditTest(Test test, Answer answer)
        {

                db.Entry(test).State = EntityState.Modified;
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
        }

        public ActionResult CreateExplanation(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    ViewBag.IdExplanation = id;
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
        public ActionResult CreateExplanation(int id, Explanation explanation, Check check)
        {
                Explanation Exp = db.Explanations.Where(x => x.Id == id).FirstOrDefault();
                Exp.Description = explanation.Description;
                Exp.CkExplanation = explanation.CkExplanation;

                Check CkExp = db.Cheks.Where(x => x.Id == id).FirstOrDefault();
                CkExp.CkExplanation = check.CkExplanation;
                db.SaveChanges();

                return RedirectToAction("Index");
        }

        public ActionResult EditExplanation(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    ViewBag.explanation = db.Explanations.Where(x => x.Id == id).FirstOrDefault();
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
        public ActionResult EditExplanation(Explanation explanation)
        {

                db.Entry(explanation).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
        }

        public ActionResult PublishedThemas(int status)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    if(status != 0)
                    {
                        Check check = db.Cheks.Where(x => x.Id == status).FirstOrDefault();
                        check.ControlCheck = "published";
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
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

        public ActionResult CallAway(int status)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    if (status != 0)
                    {
                        Check check = db.Cheks.Where(x => x.Id == status).FirstOrDefault();
                        check.ControlCheck = null;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
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


        public ActionResult Delete(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    Thema thema = db.Themas.Where(x => x.Id == id).FirstOrDefault();
                    Teoria teoria = db.Teorias.Where(x => x.Id == id).FirstOrDefault();
                    Test test = db.Tests.Where(x => x.Id == id).FirstOrDefault();
                    Explanation explanation = db.Explanations.Where(x => x.Id == id).FirstOrDefault();
                    Answer answer = db.Answers.Where(x => x.Id == id).FirstOrDefault();
                    Check check = db.Cheks.Where(x => x.Id == id).FirstOrDefault();

                    db.Themas.Remove(thema);
                    db.Teorias.Remove(teoria);
                    db.Tests.Remove(test);
                    db.Explanations.Remove(explanation);
                    db.Answers.Remove(answer);
                    db.Cheks.Remove(check);
                    db.SaveChanges();

                return RedirectToAction("Index");
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

        public ActionResult Check(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "admin")
                {
                    ViewBag.check = db.Cheks.Where(x => x.LanguageId == id).ToList();
                    ViewBag.language_name = db.Languages.Where(x => x.Id == id).FirstOrDefault();
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
    }
}
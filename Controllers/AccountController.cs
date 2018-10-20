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
    public class AccountController : Controller
    {
        private ThemaContext db = new ThemaContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            {
                var usr = db.Users.Single(u => u.LoginUser == user.LoginUser && u.Password == user.Password);
                Session["Id"] = usr.Id.ToString();
                Session["Login"] = usr.LoginUser;
                Session["Email"] = usr.Email;
                Session["Role"] = usr.Role;
                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException)
            {
                ViewBag.ErrorLogin = "не верный логин или пароль";
                return View();
            }

        }

        //регистрация нового пользователя
        public ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(User users, string Password, string RePassword)
        {
            //проверяем, совпадает ли подтверждение паролей
            if (Password != RePassword)
            {
                ViewBag.ErrorConfirmPassword = "пароли не совпадают";
                return View();
            }
            else
            {
                //ищем пользователя в базе
                var login = db.Users.Where(x => x.LoginUser == users.LoginUser).FirstOrDefault();
                var email = db.Users.Where(x => x.Email == users.Email).FirstOrDefault();
                //если такой пользователь уже зарегистрирован то выдаем ошибку
                if (login != null)
                {
                    ViewBag.ErrorRegisterLogin = "пользователь c логином "+login.LoginUser.ToString()+" уже существует";
                    return View();
                }
                else if (email != null)
                {
                    ViewBag.ErrorRegisterEmail = "пользователь c email: " + email.Email.ToString() + " уже существует";
                    return View();
                }
                //если такого пользователя нет в системе то регистрируем его
                else
                {
                    if (ModelState.IsValid)
                    {
                        db.Users.Add(users);
                        db.SaveChanges();

                        return RedirectToAction("Login");
                    } else
                    {
                        return View();
                    }                   
                }
            }
        }

        //редактирование данных аккаунта
        public ActionResult EditAccount(int id)
        {
            try
            {
                var Role = Session["Role"].ToString();

                if (Role == "user")
                {
                    return View(db.Users.Where(x => x.Id == id).FirstOrDefault());
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
        public ActionResult EditAccount(int id, string LoginUser, string Email, string OldPassword, string NewPassword, string ReNewPassword)
        {
            //ищем пользователя в системе
            var usr = db.Users.Where(x => x.Id == id).FirstOrDefault();
            //переверяем введенные данные с теми что у базе
            //если пароли совпадают старый и новые
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
                    User user = db.Users.Where(x => x.Id == id).FirstOrDefault();
                    user.LoginUser = LoginUser;
                    user.Email = Email;
                    user.Password = NewPassword;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
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
    }
}
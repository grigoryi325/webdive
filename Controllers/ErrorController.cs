using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQuery.Controllers
{
    public class ErrorController : Controller
    {
        //Ошибка при вхлде в аккаунт
        public ActionResult ErrorLogin()
        {
            return View();
        }
        //ошибка при регистрации (пользователь стаким login уже зарегистрирован)
        public ActionResult ErrorRegisterLogin()
        {
            return View();
        }
        //ошибка при регистрации (пользователь стаким email уже зарегистрирован)
        public ActionResult ErrorRegisterEmail()
        {
            return View();
        }
        //ошибка редактирования данных аккаунта
        public ActionResult ErrorEditAccount()
        {
            return View();
        }
        //ошибка доступа
        public ActionResult ErrorAccess()
        {
            return View();
        }
        //ошибка подтверждения пароля
        public ActionResult ErrorConfirmOldPassword()
        {
            return View();
        }
        //ошибка подтверждения пароля
        public ActionResult ErrorConfirmNewPassword()
        {
            return View();
        }
        //ошибка добавления нового канала(если такой канал уже существует в базе)
        public ActionResult ErrorAddNewTheme()
        {
            return View();
        }
    }
}
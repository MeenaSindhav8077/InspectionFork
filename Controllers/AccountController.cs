using Inspection.Web.DataBase;
using Inspection.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Inspection.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        public ActionResult _Login(LoginModel _Model)
        {
            try
            {
                ISUser _user = DB.ISUsers.Where(p => p.UserName == _Model.UserName && p.Password == _Model.Password).FirstOrDefault();
                if (_user != null)
                {
                    Session["userid"] = _user.ID;
                    Session["Username"] = _user.UserName;
                    Session["Usertype"] = _user.UserType;
                    FormsAuthentication.SetAuthCookie(_Model.UserName, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return View();
        }
    }
}
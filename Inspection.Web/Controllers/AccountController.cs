using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Inspection.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
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
            LoginModel loginModel = new LoginModel();
            return View(loginModel);
        }

        ITe_INDIAEntities1 DB = new ITe_INDIAEntities1();
        LogService logService = new LogService();

        public ActionResult _Login(LoginModel _Model)
        {
            string username = _Model.UserName;
            string password = _Model.Password;
            try
            {
                USER_MST _user = DB.USER_MST.FirstOrDefault(p => p.TenentID == 10 && p.PASSWORD_CHNG == password && p.LOGIN_ID.Contains(username));
                if (_user != null)
                {
                    Session["userid"] = _user.USER_ID;
                    Session["Username"] = username;
                    Session["name"] = _user.FIRST_NAME;
                    Session["password"] = password;
                    Session["pic"] = "http://192.168.1.97/pic/" + _user.personID + ".jpg";
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
                logService.AddLog(ex, "_Login", "AccountController");
                throw;
            }
        }
        public ActionResult Logout()
        {
            // Log the user out
            FormsAuthentication.SignOut();

            // Clear the session if needed
            Session.Clear();

            return RedirectToAction("Login", "Account");

        }


        public ActionResult Register()
        {
            return View();
        }
    }
}
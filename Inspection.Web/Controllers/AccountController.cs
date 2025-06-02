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

        ITEIndiaEntities DB = new ITEIndiaEntities();
        LogService logService = new LogService();

        public ActionResult _Login(LoginModel _Model)
        {
            string username = _Model.UserName;
            string password = _Model.Password;
            try
            {// Fetch user from Final_Inspection_UserList table using UserName and Password
                Final_Inspection_UserList _user = DB.Final_Inspection_UserList.FirstOrDefault(
                    p => p.UserName == username && p.Password == password
                );
                if (_user != null)
                {
                    Session["userid"] = _user.UserID;
                    Session["Username"] = username;
                    Session["name"] = _user.FirstName;
                    Session["password"] = password;
                    Session["pic"] = ""; // No personID in this table, so leave blank or set as needed
                    FormsAuthentication.SetAuthCookie(_Model.UserName, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "User does not exist.";
                    return View("Login", _Model);
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
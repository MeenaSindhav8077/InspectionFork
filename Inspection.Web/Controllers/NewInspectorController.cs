using System.Web.Mvc;
using Google;
using Inspection.Web.DataBase;
using Inspection.Web.Models;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;

namespace Inspection.Web.Controllers
{
    public class NewInspectorController : Controller
    {

        ITEIndiaEntities DB = new ITEIndiaEntities();

        //private string HashPassword(string password)
        //{
        //    using (var sha256 = SHA256.Create())
        //    {
        //        var bytes = Encoding.UTF8.GetBytes(password);
        //        var hash = sha256.ComputeHash(bytes);
        //        return Convert.ToBase64String(hash);
        //    }
        //}

        // GET: NewInspector
        [HttpGet]
        public ActionResult NewInspector(int id = 0)
        {
            NewInspectorViewModel _viewModel = new NewInspectorViewModel();
            NewInspectorModel viewModel = new NewInspectorModel();
            string status = "add";

            if (id > 0)
            {
                var _Inspection_UserList = DB.Final_Inspection_UserList.FirstOrDefault(p => p.UserID == id);
                if (_Inspection_UserList != null)
                {
                    viewModel.UserID = _Inspection_UserList.UserID;
                    viewModel.FirstName = _Inspection_UserList.FirstName;
                    viewModel.LastName = _Inspection_UserList.LastName;
                    viewModel.Password = _Inspection_UserList.Password;
                    viewModel.Department = _Inspection_UserList.Department;

                    status = "edit";
                }
            }

            _viewModel = new NewInspectorViewModel
            {
                NewInspector = viewModel,
                Inspectors = DB.Final_Inspection_UserList.Select(i => new NewInspectorModel
                {
                    UserID = i.UserID,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Department = i.Department
                }).ToList(),
                Status = status
            };

            return View(_viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewInspector(NewInspectorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var exists = DB.Final_Inspection_UserList.Any(i => i.UserID == viewModel.NewInspector.UserID);
                if (exists)
                {
                    ModelState.AddModelError("NewInspector.UserID", "An inspector with this ID already exists.");
                }
                else
                {
                    try
                    {
                        var newInspector = new Final_Inspection_UserList
                        {
                            UserID = viewModel.NewInspector.UserID,
                            FirstName = viewModel.NewInspector.FirstName,
                            LastName = viewModel.NewInspector.LastName,
                            Department = viewModel.NewInspector.Department,
                            Password = viewModel.NewInspector.Password,
                            UserName = viewModel.NewInspector.FirstName + "." + viewModel.NewInspector.LastName
                        };

                        DB.Final_Inspection_UserList.Add(newInspector);
                        DB.SaveChanges();

                        TempData["SuccessMessage"] = "Inspector added successfully!";
                        return RedirectToAction("NewInspector");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "An error occurred while adding the inspector.");
                    }
                }
            }

            // Re-fetch inspectors on error
            viewModel.Inspectors = DB.Final_Inspection_UserList.Select(i => new NewInspectorModel
            {
                UserID = i.UserID,
                FirstName = i.FirstName,
                LastName = i.LastName,
                Department = i.Department
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInspector(NewInspectorViewModel inspector)
        {
            if (ModelState.IsValid)
            {
                var existing = DB.Final_Inspection_UserList.FirstOrDefault(i => i.UserID == inspector.NewInspector.UserID);
                if (existing != null)
                {
                    existing.FirstName = inspector.NewInspector.FirstName;
                    existing.LastName = inspector.NewInspector.LastName;
                    existing.Department = inspector.NewInspector.Department;
                    existing.Password = inspector.NewInspector.Password;
                   
                   
                    DB.SaveChanges();
                    TempData["SuccessMessage"] = "Inspector updated successfully!";
                }
                else
                {
                    ModelState.AddModelError("", "Inspector not found.");
                }
            }
            return RedirectToAction("NewInspector");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteInspector(int userId)
        {
            var inspector = DB.Final_Inspection_UserList.FirstOrDefault(i => i.UserID == userId);
            if (inspector != null)
            {
                DB.Final_Inspection_UserList.Remove(inspector);
                DB.SaveChanges();
                TempData["SuccessMessage"] = "Inspector deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Inspector not found.";
            }
            return RedirectToAction("NewInspector");
        }
    }
}

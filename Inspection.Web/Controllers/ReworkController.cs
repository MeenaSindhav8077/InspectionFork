using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class ReworkController : Controller
    {
        // GET: Rework
        public ActionResult Index()
        {
            return View();
        }
    }
}
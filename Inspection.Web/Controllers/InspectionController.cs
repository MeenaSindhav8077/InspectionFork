using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    public class InspectionController : Controller
    {
        // GET: Inspection
        public ActionResult Index()
        {
            return View();
        }
    }
}
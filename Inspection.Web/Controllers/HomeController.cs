using Inspection.Web.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        public ActionResult Index()
        {
            var currentDate = DateTime.Now;
            var twoDaysAgo = currentDate.AddDays(-2);

            try
            {
                var _data = DB.Final_Inspection_Data.Where(v => v.Inward_Date < twoDaysAgo).ToList();

                if (_data.Count > 0)
                {
                    var matchingDataList = _data.Where(entry => DB.Final_Inspection_Process.Any(secondEntry => secondEntry.PID != entry.ID)).ToList();

                }
            }
            catch (Exception)
            {

            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class AfterInspectionController : Controller
    {
        // GET: AfterInspection
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        List<InwardDataModel> _List = new List<InwardDataModel>();
        public ActionResult Index()
        {
            _List = (from model in DB.Final_Inspection_Data.OrderByDescending(p => p.ID)
                    select new InwardDataModel
                    {
                        id = model.ID,
                        InwardTime = model.Inward_Time,
                        InwardDate = model.Inward_Date,
                        JobNo = model.JobNum,
                        Partno = model.PartNum,
                        Stage = model.Stage,
                        ERev = model.EpiRev,
                        ActualRev = model.ActRev,
                        Qty = model.qty,
                        Status = model.Status,
                       // RequideMrb = mrb
                        //currentstage = value,
                        //_submodel = new Submodel(),
                    }
                        ).ToList();

            return View(_List);
        }

        [HttpPost]
        public ActionResult YourAction(int id, bool checkeded)
        {
            // Use the id and checked values as needed
            // ...

            // Assuming you want to return a JSON response
            return Json(new { success = true, message = "Data successfully processed" });
        }
    }
}
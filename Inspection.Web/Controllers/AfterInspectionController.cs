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
        [Authorize]
        public ActionResult Index()
        {
            _List = (from model in DB.Final_Inspection_Process.OrderByDescending(p => p.ID)
                    select new InwardDataModel
                    {
                        id = model.ID,
                        InwardTime = model.starttime,
                        InwardDate = model.Inspection_date,
                        JobNo = model.JobNum,
                        Partno = model.PartNum,
                        Stage = model.Stage,
                        // ERev = model.e,
                        //  ActualRev = model.ActRev,
                        IQTY = model.Inspection_Qty,
                        Status = model.Inspection_Type,
                        RequideMrb = model.CkMRB ?? false,
                       // RequideMrb = model.ch
                    }
                        ).ToList();

            return View(_List);
        }

        
        [Authorize]
        public ActionResult Mrbrequred(String id)
        {
            try
            {
                int ID = Convert .ToInt32(id);  
                Final_Inspection_Process _data = DB.Final_Inspection_Process.Where(v => v.ID == ID).FirstOrDefault();

                if (_data != null)
                {
                    _data.CkMRB = true;
                    DB.SaveChanges();
                }
                TempData["SuccessMessage"] = "Data saved successfully.";
            }
            catch (Exception)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
            }
            return RedirectToAction("Index");
        }
    }
}
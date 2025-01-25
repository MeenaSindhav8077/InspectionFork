using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Inspection.Web.Service;
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
        ITe_INDIAEntities1 DB = new ITe_INDIAEntities1();
        List<InwardDataModel> _List = new List<InwardDataModel>();
        LogService logService = new LogService();
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                _List = (from model in DB.Final_Inspection_Data.Where(p=>p.Active == true && p.closerequest == true).OrderByDescending(p => p.ID)
                         select new InwardDataModel
                         {
                             id = model.ID,
                             InwardTime = model.Inward_Time,
                             InwardDate = model.Inward_Date,
                             JobNo = model.JobNum,
                             Partno = model.PartNum,
                             ProcessStage = model.Stage,
                             ERev = model.EpiRev,
                             ActualRev = model.ActRev,
                             Qty = model.Inspection_Qty,
                             InspectionType = model.Inspection_Type,
                         }).ToList();
            }
            catch (Exception ex)
            {
                logService.AddLog(ex, "Index", "AfterInspectionController");
            }

            return View(_List);
        }
    }
}
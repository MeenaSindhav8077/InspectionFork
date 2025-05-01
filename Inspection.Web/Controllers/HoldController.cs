using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Inspection.Web.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    public class HoldController : Controller
    {
        // GET: Hold
        ITEIndiaEntities DB = new ITEIndiaEntities();
        LogService logService = new LogService();
        public ActionResult Index()
        {
            List<InwardDataModel> _List = new List<InwardDataModel>();
            try
            {
                _List = (from model in DB.Final_Inspection_Data.Where(l => l.Hold == true).OrderByDescending(p => p.ID)
                         select new InwardDataModel
                         {
                             id = model.ID,
                             JobNo = model.JobNum,
                             Qty = model.Inspection_Qty,
                             SampleQuantity = model.Sample_Qty,
                             Partno = model.PartNum,
                             InwardDate = model.Inward_Date,
                             InwardTime = model.Inward_Time,
                             InspectionType = model.Inspection_Type,
                             QualityStage = model.QualityStage,
                             Note = model.Note,
                             ActualRev =model.ActRev,
                             ERev =model.EpiRev
                         }).ToList();
            }
            catch (Exception ex)
            {
                logService.AddLog(ex, "HoldIndex", "HoldController");
            }
            return View(_List);
        }
    }
}
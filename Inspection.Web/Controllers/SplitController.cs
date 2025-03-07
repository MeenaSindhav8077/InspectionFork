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
    public class SplitController : Controller
    {
        // GET: Split
        ITe_INDIAEntities1 DB = new ITe_INDIAEntities1();
        LogService logService = new LogService();
        public ActionResult Index()
        {
            List<InwardDataModel> _List = new List<InwardDataModel>(); 
            try
            {
                _List = (from model in DB.Final_Inspection_Data.Where(l => l.split == true).OrderByDescending(p => p.ID)
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
                                               ActualRev = model.ActRev,
                                               ERev = model.EpiRev
                                           }).ToList();
            }
            catch (Exception ex)
            {
                logService.AddLog(ex, "SplitIndex", "SplitController");
            }
            return View(_List);
        }
    }
}
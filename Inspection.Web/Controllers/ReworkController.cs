using Inspection.Web.DataBase;
using Inspection.Web.Models;
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
        ITe_INDIAEntities1 DB = new ITe_INDIAEntities1();
        public ActionResult Index()
        {
            List<InwardDataModel> inwardDataModel = new List<InwardDataModel>();
            try
            {
                inwardDataModel = (from model in DB.Final_Inspection_Process.OrderByDescending(p => p.ID)
                                   select new InwardDataModel
                                   {
                                       id = model.ID,
                                       JobNo = model.JobNum,
                                       IQTY = model.Inspection_Qty,
                                       Partno = model.PartNum,
                                       InwardDate = model.Inspection_date,
                                       InspectionType = model.Inspection_Type,
                                   }).ToList();
            }
            catch (Exception ex)
            {
            }
            return View(inwardDataModel);
        }
    }
}
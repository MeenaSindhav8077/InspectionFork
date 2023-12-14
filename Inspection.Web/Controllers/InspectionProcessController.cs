using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Inspection.Web.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class InspectionProcessController : Controller
    {
        // GET: InspectionProcess
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        List<InwardDataModel> List = new List<InwardDataModel>();
        Inspectionservice _service = new Inspectionservice();
        public ActionResult Index()
        {
            try
            {

                List = (from model in DB.Final_Inspection_Data.OrderByDescending(p=>p.ID)
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
                            threadinspection = model.Thread_Inspection,
                            visualinspection = model.Visual_Inspection,
                            humidity = model.Humidity_Inspection,
                            finalinspection = model.Final_Inspection

                        }
                        ).ToList();
               
            }
            catch (Exception ex)
            {
                throw;
            }

            return PartialView("Index", List);
        }
    }
}
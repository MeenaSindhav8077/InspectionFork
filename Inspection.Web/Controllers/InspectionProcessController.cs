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
                var query = (from model in DB.Final_Inspection_Data
                             group model by model.JobNum into g
                             select new
                             {
                                 JobNo = g.Key,
                                 id = g.Max(p => p.ID),
                                 InwardTime = g.Max(p => p.Inward_Time),
                                 InwardDate = g.Max(p => p.Inward_Date),
                                 Partno = g.Max(p => p.PartNum),
                                 ERev = g.Max(p => p.EpiRev),
                                 ActualRev = g.Max(p => p.ActRev),
                                 Qty = g.Max(p => p.qty),
                                 Statuschange = g.Select(p => p.Statuschange).All(s => s == true), 
                                 InspectionTypes = g.Select(p => p.Inspection_Type).ToList(),
                                 ProcessStages = g.Select(p => p.Stage).ToList(),
                                 quiality = g.Select(p => p.QualityStage).ToList(),
                             }).ToList();

                List = query.Select(item => new InwardDataModel     
                {
                    JobNo = item.JobNo,
                    id = item.id,
                    InwardTime = item.InwardTime,
                    InwardDate = item.InwardDate,
                    Partno = item.Partno,
                    ERev = item.ERev,
                    ActualRev = item.ActualRev,
                    Qty = item.Qty,
                    Statuschange = item.Statuschange,
                    InspectionType = string.Join(",", item.InspectionTypes),
                    ProcessStage = string.Join(",", item.ProcessStages),
                    QualityStage = string.Join(",", item.quiality),
                }).OrderByDescending(model => model.id)
                  .ThenBy(model => model.JobNo)
                  .ToList();

            }   
            catch (Exception ex)
            {
            }

            return PartialView("Index", List);
        }
    }
}
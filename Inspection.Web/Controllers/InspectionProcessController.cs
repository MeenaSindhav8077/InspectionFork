using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Inspection.Web.Scripts;
using Inspection.Web.Service;
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
        ITEIndiaEntities DB = new ITEIndiaEntities();
        List<InwardDataModel> List = new List<InwardDataModel>();
        Inspectionservice _service = new Inspectionservice();
        LogService logService = new LogService();
        public ActionResult Index(string search, int page = 1)
        {
            var model = new InspectionViewModel
            {
                PageNumber = page,
                PageSize = 10,
                Search = search
            };
            try
            {
                var query = DB.Final_Inspection_Data.Where(data => data.Active == true && data.Delete == false &&( data.closerequest == false || data.closerequest == null));

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(data => data.JobNum.Contains(search) || data.PartNum.Contains(search) || (data.Note.Contains(search)));
                }

                var groupedData = query.GroupBy(data => new { data.JobNum, data.QualityStage })
                    .Select(g => new
                    {
                        JobNo = g.Key.JobNum,
                        Id = g.Max(p => p.ID),
                        InwardTime = g.Max(p => p.Inward_Time),
                        InwardDate = g.Max(p => p.Inward_Date),
                        Partno = g.Max(p => p.PartNum), 
                        ERev = g.Max(p => p.EpiRev),
                        ActualRev = g.Max(p => p.ActRev),
                        Qty = g.Max(p => p.Inspection_Qty),
                        Statuschange = g.Select(p => p.Statuschange).All(s => s == true),
                        InspectionTypes = g.Select(p => p.Inspection_Type).ToList(),
                        ProcessStages = g.Select(p => p.Stage).ToList(),
                        QualityStages = g.Key.QualityStage,
                        Notes = g.Select(p => p.Note).ToList(),
                        supplier = g.Select(p => p.Suppliername).ToList(),
                    }).ToList();

                model.Items = groupedData.Select(item => new InwardDataModel
                {
                    JobNo = item.JobNo,
                    QualityStage = item.QualityStages,
                    id = item.Id,
                    InwardTime = item.InwardTime,
                    InwardDate = item.InwardDate,
                    Partno = item.Partno,
                    ERev = item.ERev,
                    ActualRev = item.ActualRev,
                    Qty = item.Qty,
                    Statuschange = item.Statuschange,
                    InspectionType = string.Join(",", item.InspectionTypes),
                    ProcessStage = string.Join(",", item.ProcessStages),
                    //QualityStage = string.Join(",", item.QualityStages),
                    Note = string.Join(",", item.Notes),
                    Supplier = string.Join(",", item.supplier),
                }).OrderByDescending(m => m.id).ToList();

                // Pagination
                model.TotalRecords = model.Items.Count();

                model.Items = model.Items.Skip((model.PageNumber - 1) * model.PageSize)
                                         .Take(model.PageSize)
                                         .ToList();
            }
            catch (Exception ex)
            {
                logService.AddLog(ex, "InspectionProcessIndex", "InspectionProcessController");
            }
            return PartialView("Index", model);
        }
    }
}
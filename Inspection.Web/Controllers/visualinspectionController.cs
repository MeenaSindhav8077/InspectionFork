using Inspection.Web.DataBase;
using Inspection.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    public class visualinspectionController : Controller
    {
        // GET: visualinspection
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        InwardDataModel List = new InwardDataModel();
        public ActionResult Index(int? id , string value)
        {
            try
            {
                List = (from model in DB.Final_Inspection_Data.Where(p=>p.ID == id)
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
                            Qty = model.Qty,
                            Status = model.Status,
                            currentstage = value,
                            _submodel = new Submodel(),
                        }
                        ).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
            return View(List);
        }
        public ActionResult AddData(Submodel _model)
        {
            try
            {
                if (_model != null)
                {
                    Final_Inspection_Process _Inspection_Data = new Final_Inspection_Process();
                    //_Inspection_Data.Process = _model.process;
                    //_Inspection_Data.JobNum = _model.jobnum;
                    //_Inspection_Data.PartNum = _model.partno;
                    //_Inspection_Data.Inspection_Type = _model.ins;
                    _Inspection_Data.Inspection_date = _model.inspectiondate;
                    _Inspection_Data.starttime = _model.StartTime;
                    _Inspection_Data.endtime = _model.EndTime;
                    _Inspection_Data.Inspection_Qty = Convert.ToInt32(_model.InspectedQty);
                    _Inspection_Data.done_by = _model.User;
                    DB.Final_Inspection_Process.Add(_Inspection_Data);
                    DB.SaveChanges();

                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index");
        }
    }
}
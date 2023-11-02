using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Inspection.Web.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Inspection.Web.Controllers
{
    public class InwardController : Controller
    {
        // GET: Inward
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        public ActionResult _AddInward()
        {
            InwardDataModel model = new InwardDataModel();
            Inspectionservice _Service = new Inspectionservice();

            model._Stage = _Service.GetInspectiontype();

            return View(model);
        }


        public ActionResult AddInward(InwardDataModel _model)
        {
            try
            {
                if (_model != null)
                {

                    EpicorERPEntities _DB = new EpicorERPEntities();
                    try
                    {
                        JobHead _job = _DB.JobHeads.Where(m => m.JobNum == _model.JobNo).FirstOrDefault();
                        if (_job != null)
                        {
                            _model.Partno = _job.PartNum;
                            _model.ERev = _job.RevisionNum;
                        }
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (_model.MStatus.Count > 0)
                        {
                           // foreach (var mitem in _model.MStatus)
                            //{
                                _model.finalinspection = _model.MStatus.Contains("Final Inspection")  ? true : false;
                                _model.humidity = _model.MStatus.Contains("Humidity")  ? true : false;
                                _model.threadinspection = _model.MStatus.Contains("Thread Inspection")  ? true : false;
                                _model.visualinspection = _model.MStatus.Contains("Visual Inspection")  ? true : false;
                           
                           // }   

                        }
                        //string status = string.Join(",", _model.MStatus);
                        //_model.Status = status;

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    int ID = DB.Final_Inspection_Data.Where(p => p.MID == 1).Count() > 0 ? Convert.ToInt32(DB.Final_Inspection_Data.Where(p => p.MID == 1).Max(p => p.ID) + 1) : 1;

                    Final_Inspection_Data _Inspection_Data = new Final_Inspection_Data();
                    _Inspection_Data.ID = ID;
                    _Inspection_Data.MID = 1;
                    _Inspection_Data.Inspection_ID = "Insp" + ID;
                    _Inspection_Data.Inward_Time = _model.InwardTime;
                    _Inspection_Data.Inward_Date = _model.InwardDate;
                    _Inspection_Data.JobNum = _model.JobNo;
                    _Inspection_Data.PartNum = _model.Partno;
                    _Inspection_Data.Stage = _model.Stage;
                    _Inspection_Data.EpiRev = _model.ERev;
                    _Inspection_Data.ActRev = _model.ActualRev;
                    _Inspection_Data.Qty = _model.Qty;
                    _Inspection_Data.Status = _model.Status;
                    _Inspection_Data.Final_Inspection = _model.finalinspection;
                    _Inspection_Data.Humidity_Inspection = _model.humidity;
                    _Inspection_Data.Thread_Inspection = _model.threadinspection;
                    _Inspection_Data.Visual_Inspection = _model.visualinspection;
                    DB.Final_Inspection_Data.Add(_Inspection_Data);
                    DB.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return RedirectToAction("_AddInward");
        }

        public ActionResult GetPartnorevno(string idjobnumber)
        {
            InwardDataModel _model = new InwardDataModel();

            EpicorERPEntities _DB = new EpicorERPEntities();
            try
            {
                JobHead _job = _DB.JobHeads.Where(m => m.JobNum == idjobnumber).FirstOrDefault();
                if (_job != null)
                {
                    _model.Partno = _job.PartNum;
                    _model.ERev = _job.RevisionNum;
                    _model.JobNo = _job.JobNum;

                    return Json(_model, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}
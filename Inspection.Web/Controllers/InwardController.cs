using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Inspection.Web.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class InwardController : Controller
    {
        // GET: Inward
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        public ActionResult _AddInward()
        {
            MainInwardModel model = new MainInwardModel();
            List<InwardDataModel> List = new List<InwardDataModel>();
            try
            {
                List = (from modal in DB.Final_Inspection_Data.Where(k => k.Active == true && k.Delete == false).OrderByDescending(p => p.ID)
                        select new InwardDataModel
                        {
                            id = modal.ID,
                            InwardDate = modal.Inward_Date,
                            Qty = modal.qty,
                            InwardTime = modal.Inward_Time,
                            JobNo = modal.JobNum,
                            Partno = modal.PartNum,
                            Stage = modal.Stage,
                            ERev = modal.EpiRev,
                            ActualRev = modal.ActRev,
                            Status = modal.Status,
                        }
                       ).ToList();

            }
            catch (Exception ex)
            {
            }
            var maindata = new MainInwardModel
            {

                _INWARDList = List,
            };

            return View(maindata);
        }

        public ActionResult AddInward(MainInwardModel model)
        {
            try
            {
                InwardDataModel _model = new InwardDataModel();
                if (_model != null)
                {
                    EpicorERPEntities _DB = new EpicorERPEntities();
                    try
                    {
                        Final_Inspection_Data _data = DB.Final_Inspection_Data.Where(m=>m.ID == model._INWARD.id && m.Active ==  true && m.Delete == false).FirstOrDefault();
                        if (_data != null)
                        {
                           _data.Inward_Time = model._INWARD.InwardTime;
                           _data.Inward_Date = model._INWARD.InwardDate;
                           _data.JobNum = model._INWARD.JobNo;
                           _data.PartNum = model._INWARD.Partno;
                           _data.Stage = model._INWARD.Stage;
                           _data.EpiRev = model._INWARD.ERev;
                           _data.ActRev = model._INWARD.ActualRev;
                           _data.qty = model._INWARD.Qty;
                           _data.Status = model._INWARD.Status;
                           _data.Inspection_Type = model._INWARD.Status;
                            _data.CurrentDate = DateTime.Now;
                            DB.SaveChanges();

                            TempData["SuccessMessage"] = "Data Updated successfully.";

                        }
                        else
                        {

                            JobHead _job = _DB.JobHeads.Where(m => m.JobNum == model._INWARD.JobNo).FirstOrDefault();
                            if (_job != null)
                            {
                                model._INWARD.Partno = _job.PartNum;
                                model._INWARD.ERev = _job.RevisionNum;
                            }
                            int stage = Convert.ToInt32(model._INWARD.Stage);
                            Final_Inspection_Stage_Master _stage = DB.Final_Inspection_Stage_Master.Where(v => v.Stage == stage).FirstOrDefault();
                            if (_stage != null)
                            {
                                model._INWARD.Stage = $"{_stage.stage_part_status.Trim()} - {_stage.Stage}";
                            }

                            int ID = DB.Final_Inspection_Data.Where(p => p.MID == 1).Count() > 0 ? Convert.ToInt32(DB.Final_Inspection_Data.Where(p => p.MID == 1).Max(p => p.ID) + 1) : 1;

                            Final_Inspection_Data _Inspection_Data = new Final_Inspection_Data();
                            _Inspection_Data.ID = ID;
                            _Inspection_Data.MID = 1;
                            _Inspection_Data.Inspection_ID = "Insp" + ID;
                            _Inspection_Data.Inward_Time = model._INWARD.InwardTime;
                            _Inspection_Data.Inward_Date = model._INWARD.InwardDate;
                            _Inspection_Data.JobNum = model._INWARD.JobNo;
                            _Inspection_Data.PartNum = model._INWARD.Partno;
                            _Inspection_Data.Stage = model._INWARD.Stage;
                            _Inspection_Data.EpiRev = model._INWARD.ERev;
                            _Inspection_Data.ActRev = model._INWARD.ActualRev;
                            _Inspection_Data.qty = model._INWARD.Qty;
                            _Inspection_Data.Status = model._INWARD.Status;
                            _Inspection_Data.Inspection_Type = model._INWARD.Status;
                            //_Inspection_Data.Final_Inspection = _model.finalinspection;
                            //_Inspection_Data.Humidity_Inspection = _model.humidity;
                            //_Inspection_Data.Thread_Inspection = _model.threadinspection;
                            //_Inspection_Data.Visual_Inspection = _model.visualinspection;
                            _Inspection_Data.CurrentDate = DateTime.Now;
                            _Inspection_Data.Active = true;
                            _Inspection_Data.Delete = false;
                            DB.Final_Inspection_Data.Add(_Inspection_Data);
                            DB.SaveChanges();

                            TempData["SuccessMessage"] = "Data saved successfully.";
                        }

                    }
                    catch (Exception ex)
                    {
                        TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
            }

            return RedirectToAction("_AddInward");
        }

        public ActionResult Edit(int id)
        {
            MainInwardModel model = new MainInwardModel();
            InwardDataModel _model = new InwardDataModel();
            List<InwardDataModel> List = new List<InwardDataModel>();
            try
            {
               
                if (_model != null)
                {

                    try
                    {
                        Final_Inspection_Data _data = DB.Final_Inspection_Data.Where(l => l.ID == id).FirstOrDefault();
                        if (_data != null)
                        {
                            _model.id = _data.ID;
                            _model.InwardDate = _data.Inward_Date;
                            _model.Status = _data.Status;
                            _model.JobNo = _data.JobNum;
                            _model.Partno = _data.PartNum;
                            _model.Stage =_model.Stage;
                            _model.ERev = _data.EpiRev;
                            _model.ActualRev = _data.ActRev;
                            _model.Qty = _data.qty;
                        }

                        List = (from modal in DB.Final_Inspection_Data.Where(k => k.Active == true && k.Delete == false).OrderByDescending(p => p.ID)
                                select new InwardDataModel
                                {
                                    id = modal.ID,
                                    InwardDate = modal.Inward_Date,
                                    Qty = modal.qty,
                                    InwardTime = modal.Inward_Time,
                                    JobNo = modal.JobNum,
                                    Partno = modal.PartNum,
                                    Stage = modal.Stage,
                                    ERev = modal.EpiRev,
                                    ActualRev = modal.ActRev,
                                    Status = modal.Status,
                                }
                     ).ToList();

                    }
                    catch (Exception ex)
                    {
                    }
                   
                   
                }
            }
            catch (Exception ex)
            {
            }

            var maindata = new MainInwardModel
            {

                _INWARD = _model,
                _INWARDList = List,
            };
            return PartialView("_AddInward", maindata);
        }
        public ActionResult Delete(int id)
        {
            MainInwardModel model = new MainInwardModel();
            InwardDataModel _model = new InwardDataModel();
            List<InwardDataModel> List = new List<InwardDataModel>();
            try
            {
                if (_model != null)
                {
                    try
                    {
                        Final_Inspection_Data _data = DB.Final_Inspection_Data.Where(l => l.ID == id).FirstOrDefault();
                        if (_data != null)
                        {
                            _data.Delete = true;
                            _data.Active =false;
                            _data.CurrentDate = DateTime.Now;
                            DB.SaveChanges();
                        }

                        List = (from modal in DB.Final_Inspection_Data.Where(k=>k.Active == true && k.Delete == false).OrderByDescending(p => p.ID)
                                select new InwardDataModel
                                {
                                    id = modal.ID,
                                    InwardDate = modal.Inward_Date,
                                    Qty = modal.qty,
                                    InwardTime = modal.Inward_Time,
                                    JobNo = modal.JobNum,
                                    Partno = modal.PartNum,
                                    Stage = modal.Stage,
                                    ERev = modal.EpiRev,
                                    ActualRev = modal.ActRev,
                                    Status = modal.Status,
                                }
                     ).ToList();

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }

            var maindata = new MainInwardModel
            {

                _INWARD = _model,
                _INWARDList = List,
            };
            return PartialView("_AddInward", maindata);
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
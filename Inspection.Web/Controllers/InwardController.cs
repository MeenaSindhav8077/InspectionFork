using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Inspection.Web.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using static Inspection.Web.Controllers.InwardController;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class InwardController : Controller
    {
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
                            ProcessStage = modal.Stage,
                            ERev = modal.EpiRev,
                            ActualRev = modal.ActRev,
                            InspectionType = modal.Inspection_Type,
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
                           _data.Stage = model._INWARD.ProcessStage;
                           _data.EpiRev = model._INWARD.ERev;
                           _data.ActRev = model._INWARD.ActualRev;
                           _data.qty = model._INWARD.Qty;
                           _data.Statuschange = false;
                           _data.Inspection_Type = model._INWARD.InspectionType;
                            _data.CurrentDate = DateTime.Now;
                            DB.SaveChanges();

                           // TempData["SuccessMessage"] = "Data Updated successfully.";

                        }
                        else
                        {

                            JobHead _job = _DB.JobHeads.Where(m => m.JobNum == model._INWARD.JobNo).FirstOrDefault();
                            if (_job != null)
                            {
                                model._INWARD.Partno = _job.PartNum;
                                model._INWARD.ERev = _job.RevisionNum;
                            }
                            int stage = Convert.ToInt32(model._INWARD.ProcessStage);
                            Final_Inspection_Stage_Master _stage = DB.Final_Inspection_Stage_Master.Where(v => v.Stage == stage).FirstOrDefault();
                            if (_stage != null)
                            {
                                model._INWARD.ProcessStage = $"{_stage.stage_part_status.Trim()} - {_stage.Stage}";
                            }

                            int ID = DB.Final_Inspection_Data.Where(p => p.MID == 1).Count() > 0 ? Convert.ToInt32(DB.Final_Inspection_Data.Where(p => p.MID == 1).Max(p => p.ID) + 1) : 1;

                            Final_Inspection_Data _Inspection_Data = new Final_Inspection_Data();
                            _Inspection_Data.ID = ID;
                            _Inspection_Data.MID = 1;
                            _Inspection_Data.Inspection_ID = "Insp" + ID;
                            _Inspection_Data.Inward_Time = model._INWARD.InwardTime;
                            _Inspection_Data.Inward_Date = model._INWARD.InwardDate;//InwardDate = Convert.ToDateTime(model.Inward_Date).ToString("yyyy-MM-dd"),
                            _Inspection_Data.JobNum = model._INWARD.JobNo;
                            _Inspection_Data.PartNum = model._INWARD.Partno;
                            _Inspection_Data.Stage = model._INWARD.ProcessStage;
                            _Inspection_Data.EpiRev = model._INWARD.ERev;
                            _Inspection_Data.ActRev = model._INWARD.ActualRev;
                            _Inspection_Data.qty = model._INWARD.Qty;
                            _Inspection_Data.Statuschange = false;
                            _Inspection_Data.Inspection_Type = model._INWARD.InspectionType;
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
            MainInwardModel models = new MainInwardModel();
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
                            _model.InspectionType = _data.Inspection_Type;
                            _model.JobNo = _data.JobNum;
                            _model.Partno = _data.PartNum;
                            _model.ProcessStage = _model.ProcessStage;
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
                                    ProcessStage = modal.Stage,
                                    ERev = modal.EpiRev,
                                    ActualRev = modal.ActRev,
                                    InspectionType = modal.Inspection_Type,
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
                                    ProcessStage = modal.Stage,
                                    ERev = modal.EpiRev,
                                    ActualRev = modal.ActRev,
                                    InspectionType = modal.Inspection_Type,
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

        public ActionResult finalinwarddata(FormCollection form)
        {
            string successMessage = "";
            string warningMessage = "";
            string errormessage = "";

            string jsonData = form["jsonData"];
            List<List<InwardItem>> yourData = JsonConvert.DeserializeObject<List<List<InwardItem>>>(jsonData);
            var jobno = yourData.SelectMany(list => list).FirstOrDefault(item => item.name == "_INWARD.JobNo").value;

            Final_Inspection_Data _datas = DB.Final_Inspection_Data.Where(l => l.JobNum == jobno).FirstOrDefault();
            if (_datas == null)
            {
                foreach (var item in yourData)
                {
                    try
                    {

                        Final_Inspection_Data _data = new Final_Inspection_Data();

                        int ID = DB.Final_Inspection_Data.Where(p => p.MID == 1).Count() > 0 ? Convert.ToInt32(DB.Final_Inspection_Data.Where(p => p.MID == 1).Max(p => p.ID) + 1) : 1;
                        _data.ID = ID;
                        _data.MID = 1;
                        _data.Inspection_ID = "Insp" + ID;
                        _data.Inward_Time = item.First(i => i.name == "_INWARD.InwardTime").value;
                        _data.Inward_Date = DateTime.Parse(item.First(i => i.name == "_INWARD.InwardDate").value);
                        _data.JobNum = item.First(i => i.name == "_INWARD.JobNo").value;
                        _data.PartNum = item.First(i => i.name == "_INWARD.Partno").value;
                        _data.Stage = item.First(i => i.name == "_INWARD.ProcessStage").value;
                        _data.QualityStage = item.First(i => i.name == "_INWARD.QualityStage").value;
                        _data.EpiRev = item.First(i => i.name == "_INWARD.ERev").value;
                        _data.ActRev = item.First(i => i.name == "_INWARD.ActualRev").value;
                        _data.qty = item.First(i => i.name == "_INWARD.Qty").value;
                        _data.Statuschange = false;
                        _data.Inspection_Type = item.First(i => i.name == "_INWARD.InspectionType").value.Trim();
                        _data.CurrentDate = DateTime.Now;
                        DB.Final_Inspection_Data.Add(_data);
                        DB.SaveChanges();

                        successMessage = "Data Saved Successfually!";

                    }

                    catch (DbEntityValidationException ex)
                    {
                        errormessage = ex.Message;

                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Console.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                            }
                        }
                    }

                }
            }
            else
            {
                warningMessage = "JobNo is all ready Exist!";
            }
            var response = new
            {
                
                successMessage = successMessage,
                errormessage = errormessage,
                warningMessage = warningMessage,
                
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public class InwardItem
        {
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}
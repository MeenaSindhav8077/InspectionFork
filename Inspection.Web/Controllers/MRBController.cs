using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Inspection.Web.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using System.Reflection.PortableExecutable;
using Inspection.Web.Scripts;
using System.Drawing;
using System.Web.UI.WebControls;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class MRBController : Controller
    {
        // GET: MRB
        ITe_INDIAEntities1 DB = new ITe_INDIAEntities1();
        LogService logService = new LogService();
        Inspectionservice inspectionservice = new Inspectionservice();
        Maineservice _service = new Maineservice();
        [Authorize]
        public ActionResult Index(string inspectiotype)
        {
            List<InwardDataModel> inwardDataModel = new List<InwardDataModel>();
            try
            {
                //if (inspectiotype != null)
                //{
                //    Session["inspectiontype"] = inspectiotype;
                //}
                //else
                //{
                //    inspectiotype = Session["inspectiontype"].ToString();
                //}
                string[] parts = "5 - Parts waiting for MRB".Split(new string[] { " - " }, StringSplitOptions.None);
                int? _stages = Convert.ToInt32(parts[0].Trim());

                string _Stage = DB.Final_Inspection_Stage_Master.Where(l => l.Stage == _stages).Select(l => l.stage_part_status).FirstOrDefault();
                List<InwardDataModel> finalInspectionData = null;

                var finalInspectionProcess = (from model in DB.Final_Inspection_Process.OrderByDescending(p => p.ID)
                                              select new InwardDataModel
                                              {
                                                  id = model.ID,
                                                  JobNo = model.JobNum,
                                                  IQTY = model.Inspection_Qty,
                                                  Partno = model.PartNum,
                                                  InwardDate = model.Inspection_date,
                                                  InspectionType = model.Inspection_Type,
                                              }).ToList();
                if (inspectiotype != null)
                {
                    finalInspectionData = (from model in DB.Final_Inspection_Data.Where(l => l.Stage.Trim() == _Stage && l.Inspection_Type == inspectiotype).OrderByDescending(p => p.ID)
                                           select new InwardDataModel
                                           {
                                               id = model.ID,
                                               JobNo = model.JobNum,
                                               Qty = model.Inspection_Qty,
                                               SampleQuantity = model.Sample_Qty,
                                               Partno = model.PartNum,
                                               InwardDate = model.Inward_Date,
                                               InspectionType = model.Inspection_Type,
                                               QualityStage = model.QualityStage,
                                               Note = model.Note,
                                               checkmrbdone = model.waitingformrb
                                           }).ToList();
                }
                else
                {
                    finalInspectionData = (from model in DB.Final_Inspection_Data.Where(l => l.Stage.Trim() == _Stage).OrderByDescending(p => p.ID)
                                           select new InwardDataModel
                                           {
                                               id = model.ID,
                                               JobNo = model.JobNum,
                                               Qty = model.Inspection_Qty,
                                               SampleQuantity = model.Sample_Qty,
                                               Partno = model.PartNum,
                                               InwardDate = model.Inward_Date,
                                               InspectionType = model.Inspection_Type,
                                               QualityStage = model.QualityStage,
                                               Note = model.Note,
                                               checkmrbdone = model.waitingformrb
                                           }).ToList();
                }
                inwardDataModel.AddRange(finalInspectionData);
            }
            catch (Exception ex)
            {
                logService.AddLog(ex, "MrbIndex", "MrbController");
            }
            return View(inwardDataModel);
        }
        [HttpPost]
        public ActionResult _AddMrbdatanew(mrbmainmodel _model, FormCollection formCollection)
        {
            try
            {
                int COUNT = 1;
                int COUNTs = 1;
                int mid = _model._MrbModel != null ? 0 : _model._MrbModel.Id;
                try
                {
                    if (_model != null)
                    {
                        string[] Qty = formCollection.GetValues("qty");

                        Final_Inspection_Data _fdata = DB.Final_Inspection_Data.Where(p => p.ID == _model._MrbModel.Id).FirstOrDefault();
                        if (_fdata != null)
                        {
                            _fdata.waitingformrb = true;
                            DB.SaveChanges();
                        }
                        foreach (string item in Qty)
                        {
                            string[] rcode;
                            string[] location;
                            string[] subqty;
                            List<string> rcodeList = new List<string>();
                            List<string> locationList = new List<string>();
                            List<string> subqtyList = new List<string>();
                            if (COUNT <= 1)
                            {
                                rcode = formCollection.GetValues("Rcode");
                                location = formCollection.GetValues("Location");
                                subqty = formCollection.GetValues("subqty");
                            }
                            else
                            {
                                string str = "Rcode" + -COUNT;
                                string strs = "Location" + -COUNT;
                                string strss = "subqty" + -COUNT;
                                rcode = formCollection.GetValues(str);
                                location = formCollection.GetValues(strs);
                                subqty = formCollection.GetValues(strss);
                            }
                            string[] rcodeS = formCollection.GetValues("RcodeproductRate" + -COUNTs);
                            string[] locations = formCollection.GetValues("LocationproductLocation" + -COUNTs);
                            string[] subqtyvs = formCollection.GetValues("subqtyproductSubqty" + -COUNTs);
                            if (subqtyvs == null)
                            {
                                subqtyvs = formCollection.GetValues("subqtysubqty" + -COUNTs);
                            }
                            if (rcode != null)
                            {
                                rcodeList.AddRange(rcode);
                            }
                            if (rcodeS != null)
                            {
                                rcodeList.AddRange(rcodeS);
                            }
                            if (location != null)
                            {
                                locationList.AddRange(location);
                            }
                            if (locations != null)
                            {
                                locationList.AddRange(locations);
                            }
                            if (subqty != null)
                            {
                                subqtyList.AddRange(subqty);
                            }
                            if (subqtyvs != null)
                            {
                                subqtyList.AddRange(subqtyvs);
                            }
                            Final_Inspection_Mrb_Data _Inspection_Data = new Final_Inspection_Mrb_Data();
                            _Inspection_Data.Qty = Convert.ToInt32(item);
                            _Inspection_Data.JobNo = _model._MrbModel.jobno;
                            _Inspection_Data.PartNo = _model._MrbModel.partno;
                            _Inspection_Data.Gid = _model._MrbModel.Id;
                            _Inspection_Data.Active = true;
                            _Inspection_Data.Deleted = false;
                            _Inspection_Data.QualityStage = _model._MrbModel.Qualitystage;
                            DB.Final_Inspection_Mrb_Data.Add(_Inspection_Data);
                            DB.SaveChanges();

                            for (int i = 0; i < rcodeList.Count; i++)
                            {
                                Final_Inspection_Mrb_Rcode _data = new Final_Inspection_Mrb_Rcode();

                                _data.PID = _Inspection_Data.ID;
                                _data.Rcode = rcodeList[i];
                                _data.SubQty = subqtyList[i];
                                _data.Rtaxt = locationList[i];
                                var currentRcode = rcodeList[i];
                                var secondFieldValue = DB.Final_Inspection_RCode.FirstOrDefault(v => v.RCode == currentRcode)?.Description;
                                _data.Remark = secondFieldValue;
                                _data.Deleted = false;
                                _data.Active = true;
                                DB.Final_Inspection_Mrb_Rcode.Add(_data);
                                DB.SaveChanges();
                            }
                            COUNT++;
                            COUNTs++;
                        }
                        GeneratePdfAndSendEmail(_model._MrbModel.Id);
                        TempData["SuccessMessage"] = "Data saved successfully.";
                    }
                }
                catch (DbEntityValidationException e)
                {

                    TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    logService.AddLog(e, "_AddMrbdatanew", "MrbController");
                }
                return RedirectToAction("Index");
                //return RedirectToAction("Mrbform");
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
            }
            return View();
        }
        public ActionResult Mrbform(int id)
        {
            MrbModel _model = new MrbModel();
            List<MrbModel> _LIst = new List<MrbModel>();
            mrbmainmodel _Model = new mrbmainmodel();
            List<MrbdecisioModel> _mrbmodel = new List<MrbdecisioModel>();
            try
            {
                Final_Inspection_Mrb_Data _data = DB.Final_Inspection_Mrb_Data.Where(p => p.Gid == id).FirstOrDefault();
                if (_data != null)
                {

                    _model = (from model in DB.Final_Inspection_Data.Where(p => p.ID == id)
                              select new MrbModel
                              {
                                  Id = model.ID,
                                  //Serialno = model.PID,
                                  jobno = model.JobNum,
                                  qty = model.Inspection_Qty,
                                  partno = model.PartNum,
                                  stage = model.Stage,
                                  inspectiontype = model.Inspection_Type,
                                  date = model.Inward_Date,
                                  Qualitystage = model.QualityStage,
                                  note = model.Note,
                                  Sampleqty = model.Sample_Qty,
                              }).FirstOrDefault();
                }
                else
                {
                    _model = (from model in DB.Final_Inspection_Data.Where(p => p.ID == id)
                              select new MrbModel
                              {
                                  Id = model.ID,
                                  jobno = model.JobNum,
                                  qty = model.Inspection_Qty,
                                  partno = model.PartNum,
                                  stage = model.Stage,
                                  inspectiontype = model.Inspection_Type,
                                  date = model.Inward_Date,
                                  Sampleqty = model.Sample_Qty,
                                  Qualitystage = model.QualityStage,
                                  note = model.Note,
                              }).FirstOrDefault();

                }
                var list = new mrbmainmodel
                {
                    _MrbModel = _model,
                    _MrbModellist = _LIst,
                    mrbdecisioModel = _mrbmodel,

                };
                return PartialView("_MRBAdd", list);
            }
            catch (DbEntityValidationException e)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                logService.AddLog(e, "Mrbform", "MrbController");
            }
            return View();
        }
        [Authorize]
        public ActionResult MrbDesicion(int id)
        {
            MrbModel _model = new MrbModel();
            List<MrbModel> _LIst = new List<MrbModel>();
            List<MrbdecisioModel> _LIsts = new List<MrbdecisioModel>();
            mrbmainmodel _Model = new mrbmainmodel();
            var list = new mrbmainmodel();
            try
            {
                _model = (from model in DB.Final_Inspection_Data.Where(p => p.ID == id)
                          select new MrbModel
                          {
                              Id = model.ID,
                              jobno = model.JobNum,
                              qty = model.Inspection_Qty,
                              partno = model.PartNum,
                              stage = model.Stage,
                              inspectiontype = model.Inspection_Type,
                              date = model.Inward_Date,
                              MRbDate = model.MRBDate,
                              //inspectedby = model.,
                              iId = id,

                          }).FirstOrDefault();

                _LIsts = (from model1 in DB.Final_Inspection_Mrb_Data.Where(p => p.Gid == id && p.Active == true && p.Deleted == false && (p.ReworkMrb == null || p.ReworkMrb == false))
                          join model2 in DB.Final_Inspection_Mrb_Rcode
                          .Where(p => p.Active == true && p.Deleted == false)
                          on model1.ID equals model2.PID into relatedRecords
                          select new MrbdecisioModel
                          {
                              Id = model1.ID,
                              jobno_j = model1.JobNo,
                              Qtys = model1.Qty,
                              partno_p = model1.PartNo,
                              ids = relatedRecords.Select(p => p.id).ToList(),
                              Reject = relatedRecords.Select(p => p.Reject).ToList(),
                              Accept = relatedRecords.Select(p => p.Accept).ToList(),
                              Rework = relatedRecords.Select(p => p.Rework).ToList(),
                              Sorting = relatedRecords.Select(p => p.Sorting).ToList(),
                              Resorting = relatedRecords.Select(p => p.Resorting).ToList(),
                              Deviation = relatedRecords.Select(p => p.Deviation).ToList(),
                              ReworkMRB = relatedRecords.Select(p => p.Reworkinmrb).ToList(),
                              ReMeasured = relatedRecords.Select(p => p.Remeasured).ToList(),
                              Split = relatedRecords.Select(p => p.Split).ToList(),
                              Hold = relatedRecords.Select(p => p.Hold).ToList(),
                              Rcode = relatedRecords.Select(r => r.Rcode).ToList(),
                              Description = relatedRecords.Select(r => r.Remark).ToList(),
                              location = relatedRecords.Select(r => r.Rtaxt).ToList(),
                              Desicion = relatedRecords.Select(r => r.Desicion).ToList(),
                              subqty = relatedRecords.Select(r => r.SubQty).ToList(),
                              inersubqty = relatedRecords.Select(r => r.DesicionSubQty).ToList()
                          }).ToList();

                if (_model == null)
                {
                    _model = new MrbModel();
                }
                list = new mrbmainmodel
                {
                    _MrbModel = _model,
                    _MrbModellist = _LIst,
                    mrbdecisioModel = _LIsts,
                };

            }
            catch (DbEntityValidationException e)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                logService.AddLog(e, "MrbDesicion", "MrbController");
            }

            return PartialView("_MrbDesicion", list);
        }
        public ActionResult _AddDesicion(string qty, int id, int iid)
        {
            MrbModel model = new MrbModel();
            try
            {
                if (id != null)
                {
                    Final_Inspection_Mrb_Rcode _rcode = DB.Final_Inspection_Mrb_Rcode.Where(l => l.id == id).FirstOrDefault();
                    if (_rcode != null)
                    {
                        model.subqty = _rcode.SubQty;
                        model.Description = _rcode.Remark;
                        model.iId = iid;
                    }
                }
            }
            catch (Exception ex)
            {

                logService.AddLog(ex, "_AddDesicion", "MrbController");
            }
            return PartialView("_mrbdesiciondata", model);
        }
        public ActionResult AddDatetomrb(mrbmainmodel _model)
        {
            MrbModel model = new MrbModel();
            try
            {
                if (_model._MrbModel.Id != null)
                {
                    Final_Inspection_Data _rcode = DB.Final_Inspection_Data.Where(l => l.ID == _model._MrbModel.Id).FirstOrDefault();
                    if (_rcode != null)
                    {
                        _rcode.MRBDate = _model._MrbModel.MRbDate;
                        DB.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                logService.AddLog(ex, "_AddDesicion", "MrbController");
            }
            return RedirectToAction("MrbDesicion", new { id = _model._MrbModel.Id });
        }
        [HttpPost]
        public ActionResult AddDesicion(mrbmainmodel _model, FormCollection formCollection)
        {
            try
            {
                int count = 0;
                try
                {
                    if (_model != null)
                    {
                        string[] Desicion = formCollection.GetValues("Mrbdecisio");
                        string[] Rcode = formCollection.GetValues("rcods");

                        foreach (var item in _model.mrbdecisioModel)
                        {
                            int ID = item.Id;
                            List<Final_Inspection_Mrb_Rcode> _data = DB.Final_Inspection_Mrb_Rcode.Where(l => l.Active == true && l.Deleted == false && l.PID == ID).ToList();
                            if (_data != null)
                            {
                                for (int i = 0; i < _data.Count; i++)
                                {
                                    if (_data[i].Rcode == Rcode[count])
                                    {
                                        _data[i].Desicion = Desicion[count];
                                        DB.SaveChanges();
                                    }
                                    count++;
                                }
                            }
                        }
                        TempData["SuccessMessage"] = "Data saved successfully.";
                    }
                }
                catch (DbEntityValidationException e)
                {
                    TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    logService.AddLog(e, "AddDesicion", "MrbController");
                }
                return RedirectToAction("MrbDesicion", new { id = _model._MrbModel.Id });
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                logService.AddLog(ex, "AddDesicion", "MrbController");
            }
            return View();
        }
        public ActionResult GetDescription(string Rcode)
        {
            try
            {
                Final_Inspection_RCode _data = DB.Final_Inspection_RCode.Where(v => v.RCode == Rcode).FirstOrDefault();
                if (_data != null)
                {
                    var defectDescription = _data.Description;
                    return Json(defectDescription, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                logService.AddLog(ex, "GetDescription", "MrbController");
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DecisionData(MrbModel _model, FormCollection formCollection)
        {
            try
            {
                int? pid = 0;
                int newAcceptQty = 0;
                int newRejectQty = 0;
                int newReworkQty = 0;
                int newsortingQty = 0;
                int newdaviationQty = 0;
                bool isSplit = false;
                bool isHold = false;
                bool issorting = false;
                bool isrework = false;
                bool isdeviation = false;
                bool ispacking = false;
                int totalmrbqty = 0;
                int grandTotal = 0;
                List<string> decisionsList = new List<string>();
                try
                {

                    var mrbRcode = DB.Final_Inspection_Mrb_Rcode.FirstOrDefault(h => h.id == _model.Id);
                    pid = _model.iId;
                    Final_Inspection_Data final_Inspection = DB.Final_Inspection_Data.Where(k => k.ID == _model.iId).FirstOrDefault();
                    if (mrbRcode != null)
                    {
                        foreach (var item in _model.DecisionItems)
                        {
                            string decision = item.Decisionmrb;
                            int qty = item.SubQtyMrb;

                            switch (decision)
                            {
                                case "Reject":
                                    mrbRcode.Reject = qty;
                                    newRejectQty += qty;
                                    break;
                                case "Accept":
                                    mrbRcode.Accept = qty;
                                    newAcceptQty += qty;
                                    break;
                                case "Rework":
                                    mrbRcode.Rework = qty;
                                    newReworkQty += qty;
                                    isrework = true;
                                    final_Inspection.InReworkDate = DateTime.Now;
                                   _service.SendEmailMovePart(qty, final_Inspection.JobNum , final_Inspection.PartNum, final_Inspection.QualityStage,final_Inspection.Inspection_Type);
                                    break;
                                case "Sorting":
                                    mrbRcode.Sorting = qty;
                                    issorting = true;
                                    newsortingQty += qty;
                                    final_Inspection.InSortingDate = DateTime.Now;
                                    break;
                                case "Re-sorting":
                                    mrbRcode.Resorting = qty;
                                    break;
                                case "Deviation":
                                    mrbRcode.Deviation = qty;
                                    isdeviation = true;
                                    newdaviationQty += qty;
                                    final_Inspection.InDaviationTime = DateTime.Now;
                                    break;
                                case "Rework in MRB":
                                    mrbRcode.Reworkinmrb = qty;
                                    break;
                                case "Re-Measured":
                                    mrbRcode.Remeasured = qty;
                                    break;
                                case "Split":
                                    mrbRcode.Split = qty;
                                    isSplit = true;
                                    break;
                                case "Hold":
                                    mrbRcode.Hold = qty;
                                    isHold = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                        DB.SaveChanges();
                        
                        if (final_Inspection != null)
                        {
                            final_Inspection.Accept_Qty = (final_Inspection.Accept_Qty ?? 0) + newAcceptQty;
                            final_Inspection.Reject_Qty = (final_Inspection.Reject_Qty ?? 0) + newRejectQty;
                            final_Inspection.Rework_Qty = (final_Inspection.Rework_Qty ?? 0) + newReworkQty;
                            final_Inspection.Deviation_Qty = (final_Inspection.Deviation_Qty ?? 0) + newdaviationQty;
                            final_Inspection.Sorting_Qty = (final_Inspection.Sorting_Qty ?? 0) + newsortingQty;
                            final_Inspection.split = (final_Inspection.split ?? false) || isSplit;
                            final_Inspection.Hold = (final_Inspection.Hold ?? false) || isHold;
                            final_Inspection.waitingforrework = (final_Inspection.waitingforrework ?? false) || isrework;
                            final_Inspection.waitingforsorting = (final_Inspection.waitingforsorting ?? false) || issorting;
                            final_Inspection.indeviation = (final_Inspection.indeviation ?? false) || isdeviation;

                            DB.SaveChanges();
                        }
                        List<Final_Inspection_Mrb_Data> _data = DB.Final_Inspection_Mrb_Data.Where(p => p.Gid == _model.iId).ToList();

                        if (_data != null && _data.Count > 0)
                        {
                            totalmrbqty = _data.Sum(p => p.Qty);
                        }

                        List<Final_Inspection_Mrb_Rcode> _Inspection_Mrb_Rcode = new List<Final_Inspection_Mrb_Rcode>();
                        if (_data != null && _data.Count > 0)
                        {
                            var dataIds = _data.Select(p => p.ID).ToList();

                            _Inspection_Mrb_Rcode = DB.Final_Inspection_Mrb_Rcode.Where(v => v.PID.HasValue && dataIds.Contains(v.PID.Value)).ToList();
                        }
                        if (_Inspection_Mrb_Rcode != null && _Inspection_Mrb_Rcode.Count > 0)
                        {
                            int totalReject = _Inspection_Mrb_Rcode.Sum(r => r.Reject ?? 0);
                            int totalAccept = _Inspection_Mrb_Rcode.Sum(r => r.Accept ?? 0);
                            int totalRework = _Inspection_Mrb_Rcode.Sum(r => r.Rework ?? 0);
                            int totalSorting = _Inspection_Mrb_Rcode.Sum(r => r.Sorting ?? 0);
                            int totalResorting = _Inspection_Mrb_Rcode.Sum(r => r.Resorting ?? 0);
                            int totalDeviation = _Inspection_Mrb_Rcode.Sum(r => r.Deviation ?? 0);
                            int totalReworkinmrb = _Inspection_Mrb_Rcode.Sum(r => r.Reworkinmrb ?? 0);
                            int totalRemeasured = _Inspection_Mrb_Rcode.Sum(r => r.Remeasured ?? 0);
                            int totalSplit = _Inspection_Mrb_Rcode.Sum(r => r.Split ?? 0);
                            int totalHold = _Inspection_Mrb_Rcode.Sum(r => r.Hold ?? 0);

                            grandTotal = totalReject + totalAccept + totalRework + totalSorting + totalResorting +
                                            totalDeviation + totalReworkinmrb + totalRemeasured + totalSplit + totalHold;

                        }
                        foreach (var item in _Inspection_Mrb_Rcode)
                        {
                            if (item.Reject != null) decisionsList.Add("Reject");
                            if (item.Accept != null) decisionsList.Add("Accept");
                            if (item.Rework != null) decisionsList.Add("Rework");
                            if (item.Sorting != null) decisionsList.Add("Sorting");
                            if (item.Resorting != null) decisionsList.Add("Resorting");
                            if (item.Deviation != null) decisionsList.Add("Deviation");
                            if (item.Reworkinmrb != null) decisionsList.Add("Reworkinmrb");
                            if (item.Remeasured != null) decisionsList.Add("Remeasured");
                            if (item.Split != null) decisionsList.Add("Split");
                            if (item.Hold != null) decisionsList.Add("Hold");
                        }
                        string decisions = string.Join(", ", decisionsList.Distinct());
                        if (grandTotal == totalmrbqty)
                        {
                            if (decisions == "Reject, Accept")
                            {
                                if (final_Inspection.Inspection_Type == "Visual" || final_Inspection.Inspection_Type == "Thread")
                                {
                                    final_Inspection.Stage = "9 - Parts Ready To Next Operation";
                                }
                                else if (final_Inspection.Inspection_Type == "Final")
                                {
                                    final_Inspection.Stage = "9 - Parts inspection completed and waiting for file complete";
                                    final_Inspection.completedandwaiting = true;
                                }
                            }
                            else
                            {
                                final_Inspection.Stage = decisions;
                            }
                            final_Inspection.waitingformrb = false;
                        }
                        DB.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    logService.AddLog(e, "DecisionData", "MrbController");
                }
                return RedirectToAction("MrbDesicion", new { id = pid });
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                logService.AddLog(ex, "DecisionData", "MrbController");
            }
            return View();
        }
        public ActionResult Generatepdf(int id)
        {
            MrbModel _model = new MrbModel();
            List<MrbModel> _LIst = new List<MrbModel>();
            List<MrbdecisioModel> _LIsts = new List<MrbdecisioModel>();
            mrbmainmodel _Model = new mrbmainmodel();
            try
            {
                _model = (from model in DB.Final_Inspection_Process.Where(p => p.ID == id)
                          select new MrbModel
                          {
                              Id = model.ID,
                              jobno = model.JobNum,
                              Qty_qty = model.Inspection_Qty,
                              partno = model.PartNum,
                              stage = model.Stage,
                              inspectiontype = model.Inspection_Type,
                              date = model.Inspection_date,
                              inspectedby = model.done_by
                          }).FirstOrDefault();

                _LIst = (from model in DB.Final_Inspection_Process.Where(p => p.ID == id)
                         select new MrbModel
                         {
                             Id = model.ID,
                             jobno = model.JobNum,
                             Qty_qty = model.Inspection_Qty,
                             partno = model.PartNum,
                             stage = model.Stage,
                             inspectiontype = model.Inspection_Type,
                             date = model.Inspection_date,
                             inspectedby = model.done_by
                         }).ToList();

                _LIsts = (from model1 in DB.Final_Inspection_Mrb_Data.Where(p => p.Gid == id && p.Active == true && p.Deleted == false && (p.ReworkMrb == null || p.ReworkMrb == false))
                          join model2 in DB.Final_Inspection_Mrb_Rcode
                          .Where(p => p.Active == true && p.Deleted == false)
                          on model1.ID equals model2.PID into relatedRecords
                          select new MrbdecisioModel
                          {
                              Id = model1.ID,
                              jobno_j = model1.JobNo,
                              Qtys = model1.Qty,
                              partno_p = model1.PartNo,
                              ids = relatedRecords.Select(p => p.id).ToList(),
                              Reject = relatedRecords.Select(p => p.Reject).ToList(),
                              Accept = relatedRecords.Select(p => p.Accept).ToList(),
                              Rework = relatedRecords.Select(p => p.Rework).ToList(),
                              Sorting = relatedRecords.Select(p => p.Sorting).ToList(),
                              Resorting = relatedRecords.Select(p => p.Resorting).ToList(),
                              Deviation = relatedRecords.Select(p => p.Deviation).ToList(),
                              ReworkMRB = relatedRecords.Select(p => p.Reworkinmrb).ToList(),
                              ReMeasured = relatedRecords.Select(p => p.Remeasured).ToList(),
                              Split = relatedRecords.Select(p => p.Split).ToList(),
                              Hold = relatedRecords.Select(p => p.Hold).ToList(),
                              Rcode = relatedRecords.Select(r => r.Rcode).ToList(),
                              Description = relatedRecords.Select(r => r.Remark).ToList(),
                              location = relatedRecords.Select(r => r.Rtaxt).ToList(),
                              Desicion = relatedRecords.Select(r => r.Desicion).ToList(),
                              subqty = relatedRecords.Select(r => r.SubQty).ToList(),
                              inersubqty = relatedRecords.Select(r => r.DesicionSubQty).ToList()
                          }).ToList();

                if (_model == null)
                {
                    _model = (from model in DB.Final_Inspection_Data.Where(p => p.ID == id)
                              select new MrbModel
                              {
                                  Id = model.ID,
                                  jobno = model.JobNum,
                                  qty = model.Inspection_Qty,
                                  partno = model.PartNum,
                                  stage = model.Stage,
                                  inspectiontype = model.Inspection_Type,
                                  date = model.Inward_Date,
                                  note = model.Note,
                                  Qualitystage = model.QualityStage,
                              }).FirstOrDefault();
                }

                string listtoinspby = string.Join(", ", DB.Final_Inspection_Process.Where(p => p.ID == id).Select(v => v.done_by).ToList());

                var list = new mrbmainmodel
                {
                    _MrbModel = _model,
                    _MrbModellist = _LIst,
                    mrbdecisioModel = _LIsts,
                    inspectedby = listtoinspby,

                };

                return PartialView("MrbDecisionPDF", list);
            }
            catch (DbEntityValidationException e)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                logService.AddLog(e, "Generatepdf", "MrbController");
            }

            return View();
        }
        public ActionResult Editmrbdata(MrbModel _model)
        {
            try
            {
                int count = 0;
                int newAcceptQty = 0;
                int newRejectQty = 0;
                int newReworkQty = 0;
                int newsortingQty = 0;
                int newdaviationQty = 0;
                bool isSplit = false;
                bool isHold = false;
                bool issorting = false;
                bool isrework = false;
                bool isdeviation = false;
                bool ispacking = false;
                int totalmrbqty = 0;
                int grandTotal = 0;
                List<string> decisionsList = new List<string>();
                try
                {
                    if (_model != null)
                    {
                        Final_Inspection_Mrb_Rcode _data = DB.Final_Inspection_Mrb_Rcode.Where(p => p.id == _model.Id && p.Active == true && p.Deleted == false).FirstOrDefault();
                        if (_data != null)
                        {
                            string _description = DB.Final_Inspection_RCode.Where(p => p.RCode == _model.Rcodes).Select(p => p.Description).FirstOrDefault();

                            _data.Rcode = _model.Rcodes;
                            _data.Remark = _description;
                            _data.Rtaxt = _model.Location;
                            _data.SubQty = _model.subqty;
                            DB.SaveChanges();
                            // how to update decision
                        }
                        var mrbRcode = DB.Final_Inspection_Mrb_Rcode.FirstOrDefault(h => h.id == _model.Id);
                        if (_model.DecisionItems != null)
                        {
                            foreach (var item in _model.DecisionItems)
                            {
                                string decision = item.Decisionmrb;
                                int qty = item.SubQtyMrb;

                                switch (decision)
                                {
                                    case "Reject":
                                        mrbRcode.Reject = qty;
                                        newRejectQty += qty;
                                        break;
                                    case "Accept":
                                        mrbRcode.Accept = qty;
                                        newAcceptQty += qty;
                                        break;
                                    case "Rework":
                                        mrbRcode.Rework = qty;
                                        newReworkQty += qty;
                                        isrework = true;
                                        break;
                                    case "Sorting":
                                        mrbRcode.Sorting = qty;
                                        issorting = true;
                                        newsortingQty += qty;
                                        break;
                                    case "Re-sorting":
                                        mrbRcode.Resorting = qty;
                                        break;
                                    case "Deviation":
                                        mrbRcode.Deviation = qty;
                                        isdeviation = true;
                                        newdaviationQty += qty;
                                        break;
                                    case "Rework in MRB":
                                        mrbRcode.Reworkinmrb = qty;
                                        break;
                                    case "Re-Measured":
                                        mrbRcode.Remeasured = qty;
                                        break;
                                    case "Split":
                                        mrbRcode.Split = qty;
                                        isSplit = true;
                                        break;
                                    case "Hold":
                                        mrbRcode.Hold = qty;
                                        isHold = true;
                                        break;
                                    default:
                                        break;
                                }
                                //mrbRcode.DesicionSubQty = 
                            }

                            DB.SaveChanges();
                            Final_Inspection_Data final_Inspection = DB.Final_Inspection_Data.Where(k => k.ID == _model.iId).FirstOrDefault();
                            if (final_Inspection != null)
                            {
                                final_Inspection.Accept_Qty = (final_Inspection.Accept_Qty ?? 0) + newAcceptQty;
                                final_Inspection.Reject_Qty = (final_Inspection.Reject_Qty ?? 0) + newRejectQty;
                                final_Inspection.Rework_Qty = (final_Inspection.Rework_Qty ?? 0) + newReworkQty;
                                final_Inspection.Deviation_Qty = (final_Inspection.Deviation_Qty ?? 0) + newdaviationQty;
                                final_Inspection.Sorting_Qty = (final_Inspection.Sorting_Qty ?? 0) + newsortingQty;
                                final_Inspection.split = (final_Inspection.split ?? false) || isSplit;
                                final_Inspection.Hold = (final_Inspection.Hold ?? false) || isHold;
                                final_Inspection.waitingforrework = (final_Inspection.waitingforrework ?? false) || isrework;
                                final_Inspection.waitingforsorting = (final_Inspection.waitingforsorting ?? false) || issorting;
                                final_Inspection.indeviation = (final_Inspection.indeviation ?? false) || isdeviation;

                                DB.SaveChanges();
                            }
                            List<Final_Inspection_Mrb_Data> data = DB.Final_Inspection_Mrb_Data.Where(p => p.Gid == _model.iId).ToList();

                            if (_data != null && data.Count > 0)
                            {
                                totalmrbqty = data.Sum(p => p.Qty);
                            }

                            List<Final_Inspection_Mrb_Rcode> _Inspection_Mrb_Rcode = new List<Final_Inspection_Mrb_Rcode>();
                            if (_data != null && data.Count > 0)
                            {
                                var dataIds = data.Select(p => p.ID).ToList();

                                _Inspection_Mrb_Rcode = DB.Final_Inspection_Mrb_Rcode.Where(v => v.PID.HasValue && dataIds.Contains(v.PID.Value)).ToList();
                            }
                            if (_Inspection_Mrb_Rcode != null && _Inspection_Mrb_Rcode.Count > 0)
                            {
                                int totalReject = _Inspection_Mrb_Rcode.Sum(r => r.Reject ?? 0);
                                int totalAccept = _Inspection_Mrb_Rcode.Sum(r => r.Accept ?? 0);
                                int totalRework = _Inspection_Mrb_Rcode.Sum(r => r.Rework ?? 0);
                                int totalSorting = _Inspection_Mrb_Rcode.Sum(r => r.Sorting ?? 0);
                                int totalResorting = _Inspection_Mrb_Rcode.Sum(r => r.Resorting ?? 0);
                                int totalDeviation = _Inspection_Mrb_Rcode.Sum(r => r.Deviation ?? 0);
                                int totalReworkinmrb = _Inspection_Mrb_Rcode.Sum(r => r.Reworkinmrb ?? 0);
                                int totalRemeasured = _Inspection_Mrb_Rcode.Sum(r => r.Remeasured ?? 0);
                                int totalSplit = _Inspection_Mrb_Rcode.Sum(r => r.Split ?? 0);
                                int totalHold = _Inspection_Mrb_Rcode.Sum(r => r.Hold ?? 0);

                                grandTotal = totalReject + totalAccept + totalRework + totalSorting + totalResorting +
                                                totalDeviation + totalReworkinmrb + totalRemeasured + totalSplit + totalHold;

                            }
                            foreach (var item in _Inspection_Mrb_Rcode)
                            {
                                if (item.Reject != null) decisionsList.Add("Reject");
                                if (item.Accept != null) decisionsList.Add("Accept");
                                if (item.Rework != null) decisionsList.Add("Rework");
                                if (item.Sorting != null) decisionsList.Add("Sorting");
                                if (item.Resorting != null) decisionsList.Add("Resorting");
                                if (item.Deviation != null) decisionsList.Add("Deviation");
                                if (item.Reworkinmrb != null) decisionsList.Add("Reworkinmrb");
                                if (item.Remeasured != null) decisionsList.Add("Remeasured");
                                if (item.Split != null) decisionsList.Add("Split");
                                if (item.Hold != null) decisionsList.Add("Hold");
                            }
                            string decisions = string.Join(", ", decisionsList.Distinct());
                            if (grandTotal == totalmrbqty)
                            {
                                if (decisions == "Reject, Accept")
                                {
                                    if (final_Inspection.Inspection_Type == "Visual" || final_Inspection.Inspection_Type == "Thread")
                                    {
                                        final_Inspection.Stage = "9 - Parts Ready To Next Operation";
                                    }
                                    else if (final_Inspection.Inspection_Type == "Final")
                                    {
                                        final_Inspection.Stage = "9 - Parts inspection completed and waiting for file complete";
                                        final_Inspection.completedandwaiting = true;
                                    }
                                }
                                else
                                {
                                    final_Inspection.Stage = decisions;
                                }
                                final_Inspection.waitingformrb = false;
                            }
                            DB.SaveChanges();
                        }
                    }
                }
                catch (DbEntityValidationException e)
                {
                    TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    logService.AddLog(e, "AddDesicion", "MrbController");
                }
                return RedirectToAction("MrbDesicion", new { id = _model.iId });
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                logService.AddLog(ex, "AddDesicion", "MrbController");
            }
            return View();
        }
        public ActionResult _EditMRB(int id, int iid)
        {
            MrbModel _model = new MrbModel();
            List<MrbModel> _LIst = new List<MrbModel>();
            List<MrbdecisioModel> list = new List<MrbdecisioModel>();
            mrbmainmodel _Model = new mrbmainmodel();
            try
            {

                Final_Inspection_Mrb_Rcode _Mrb_Rcode = DB.Final_Inspection_Mrb_Rcode.Where(p => p.Active == true && p.Deleted == false && p.id == id).FirstOrDefault();
                if (_Mrb_Rcode != null)
                {
                    _model.Rcodes = _Mrb_Rcode.Rcode;
                    _model.Description = _Mrb_Rcode.Remark;
                    _model.Location = _Mrb_Rcode.Rtaxt;
                    _model.subqty = _Mrb_Rcode.SubQty;
                    if (_Mrb_Rcode.Reject != null)
                    {
                        _model.note = "Reject: " + _Mrb_Rcode.Reject;
                    }
                    else if (_Mrb_Rcode.Accept != null)
                    {
                        _model.note = "Accept: " + _Mrb_Rcode.Accept;

                    }
                    else if (_Mrb_Rcode.Rework != null)
                    {
                        _model.note = "Rework: " + _Mrb_Rcode.Rework;
                    }
                    else if (_Mrb_Rcode.Sorting != null)
                    {
                        _model.note = "Sorting: " + _Mrb_Rcode.Sorting;
                    }
                    else if (_Mrb_Rcode.Resorting != null)
                    {
                        _model.note = "Resorting: " + _Mrb_Rcode.Resorting;
                    }
                    else if (_Mrb_Rcode.Deviation != null)
                    {
                        _model.note = "Deviation: " + _Mrb_Rcode.Deviation;
                    }
                    else if (_Mrb_Rcode.Reworkinmrb != null)
                    {
                        _model.note = "Reworkinmrb: " + _Mrb_Rcode.Reworkinmrb;
                    }
                    else if (_Mrb_Rcode.Remeasured != null)
                    {
                        _model.note = "Remeasured: " + _Mrb_Rcode.Remeasured;
                    }
                    else
                    {
                        _model.note = null;
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                logService.AddLog(e, "Generatepdf", "MrbController");
            }
            return PartialView("_EditMRBpopup", _model);
        }
        public ActionResult _DeleteMRb(int id)
        {
            MrbModel _model = new MrbModel();
            List<MrbModel> _LIst = new List<MrbModel>();
            List<MrbdecisioModel> _LIsts = new List<MrbdecisioModel>();
            mrbmainmodel _Model = new mrbmainmodel();
            int? _id = 0;
            try
            {
                Final_Inspection_Mrb_Rcode _Inspection_Mrb_Rcode = DB.Final_Inspection_Mrb_Rcode.Where(p => p.id == id && p.Active == true && p.Deleted == false).FirstOrDefault();
                if (_Inspection_Mrb_Rcode != null)
                {
                    _Inspection_Mrb_Rcode.Active = false;
                    _Inspection_Mrb_Rcode.Deleted = true;
                    DB.SaveChanges();
                }
                _id = DB.Final_Inspection_Mrb_Data.Where(p => p.ID == _Inspection_Mrb_Rcode.PID).Select(g => g.Gid).FirstOrDefault();
            }
            catch (DbEntityValidationException e)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                logService.AddLog(e, "_DeleteMRb", "MrbController");
            }
            return RedirectToAction("MrbDesicion", new { id = _id });
        }
        [HttpPost]
        public ActionResult GeneratePdfAndSendEmail(int id)
        {
            var pdfBytes = GeneratePdfBytes("PdfTemplate", id);

            SendEmailWithAttachment(pdfBytes);

            return View("Success");
        }
        public byte[] GeneratePdfBytes(string viewName, int id)
        {

            mrbmainmodel model = inspectionservice.Getdataforpdf(id);
            string htmlContent = RenderViewToString(viewName, model);

            var pdf = new ViewAsPdf
            {
                ViewName = viewName, // Specify the view name
                IsGrayScale = false, // Optional: set other options as needed
                //PageSize = Size.A4,
                //PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 15, Right = 15, Top = 15, Bottom = 15 },
                Model = model // Pass the model to the view if required
            };

            return pdf.BuildFile(ControllerContext); // BuildFile to get PDF as byte array
        }
        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        private void SendEmailWithAttachment(byte[] pdfBytes)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    var emailMessage = new MailMessage();
                    emailMessage.From = new MailAddress("unnati@sswhite.net");
                    emailMessage.To.Add("ashvini@sswhite.net");
                    emailMessage.Subject = "Subject of the email";
                    emailMessage.Body = "Body of the email";

                    // Attach PDF to email
                    emailMessage.Attachments.Add(new Attachment(new MemoryStream(pdfBytes), "GeneratedPdf.pdf", "application/pdf"));

                    // Configure SMTP settings
                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("unnati@sswhite.net", "diev lxfd kxyw avza");
                    client.EnableSsl = true;

                    // Send email
                    client.Send(emailMessage);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
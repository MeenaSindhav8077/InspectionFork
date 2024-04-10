using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Rotativa;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class MRBController : Controller
    {
        // GET: MRB
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        [Authorize]
        public ActionResult Index()
        {
            List<InwardDataModel> inwardDataModel = new List<InwardDataModel>();

            try
            {
                inwardDataModel = (from model in DB.Final_Inspection_Process.Where(l => l.waitingMRB == true ).OrderByDescending(p => p.ID)
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
        [HttpPost]
        public ActionResult AddMrbdata(mrbmainmodel _model, FormCollection formCollection)
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

            try
            {
                _model = (from model in DB.Final_Inspection_Process.Where(p => p.ID == id)
                          select new MrbModel
                          {
                              Id = model.ID,
                              jobno = model.JobNum,
                              Qty = model.Inspection_Qty,
                              partno = model.PartNum,
                              stage = model.Stage,
                              inspectiontype = model.Inspection_Type,
                              date = model.Inspection_date,
                              inspectedby = model.done_by

                              //Location = model.location,
                          }
                        ).FirstOrDefault();

                _LIst = (from model in DB.Final_Inspection_Process.Where(p => p.ID == id)
                         select new MrbModel
                         {
                             Id = model.ID,
                             jobno = model.JobNum,
                             Qty = model.Inspection_Qty,
                             partno = model.PartNum,
                             stage = model.Stage,
                             inspectiontype = model.Inspection_Type,
                             date = model.Inspection_date,
                             inspectedby = model.done_by

                             //Location = model.location,
                         }
                        ).ToList();

                var list = new mrbmainmodel
                {
                    _MrbModel = _model,
                    _MrbModellist = _LIst,

                };

                return PartialView("_MRBAdd", list);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authorize]
        public ActionResult MrbDesicion(int id)
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
                              Qty = model.Inspection_Qty,
                              partno = model.PartNum,
                              stage = model.Stage,
                              inspectiontype = model.Inspection_Type,
                              date = model.Inspection_date,
                              inspectedby = model.done_by
                          }
                        ).FirstOrDefault();

                _LIst = (from model in DB.Final_Inspection_Process.Where(p => p.ID == id)
                         select new MrbModel
                         {
                             Id = model.ID,
                             jobno = model.JobNum,
                             Qty = model.Inspection_Qty,
                             partno = model.PartNum,
                             stage = model.Stage,
                             inspectiontype = model.Inspection_Type,
                             date = model.Inspection_date,
                             inspectedby = model.done_by
                         }
                        ).ToList();

                _LIsts = (from model1 in DB.Final_Inspection_Mrb_Data.Where(p => p.Gid == id)
                          join model2 in DB.Final_Inspection_Mrb_Rcode on model1.ID equals model2.PID
                          into relatedRecords
                          select new MrbdecisioModel
                          {
                              Id = model1.ID,
                              jobno = model1.JobNo,
                              //Qty = model1.Qty,
                              partno = model1.PartNo,
                              Rcode = relatedRecords.Select(r => r.Rcode).ToList(),
                              Description = relatedRecords.Select(r => r.Remark).ToList(),
                              location = relatedRecords.Select(r => r.Rtaxt).ToList(),
                              Desicion = relatedRecords.Select(r => r.Desicion).ToList(),
                              subqty = relatedRecords.Select(r => r.SubQty).ToList(),
                              inersubqty = relatedRecords.Select(r => r.DesicionSubQty).ToList()
                          }
                          ).ToList();
                var list = new mrbmainmodel
                {
                    _MrbModel = _model,
                    _MrbModellist = _LIst,
                    mrbdecisioModel = _LIsts,

                };
                return PartialView("_MrbDesicion", list);
            }
            catch (Exception ex)
            {
                throw;
            }
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
                }
                return RedirectToAction("MrbDesicion", new { id = _model._MrbModel.Id });
            }
            catch (Exception ex)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
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
            catch (Exception)
            {
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }
    }
}
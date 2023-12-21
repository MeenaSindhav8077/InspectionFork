using Inspection.Web.DataBase;
using Inspection.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                inwardDataModel = (from model in DB.Final_Inspection_Process.Where(l=>l.CkMRB == true).OrderByDescending(p => p.ID)
                        select new InwardDataModel
                        {
                            id = model.ID,
                            JobNo = model.JobNum,
                            IQTY = model.Inspection_Qty,
                            Partno = model.PartNum,
                            InwardDate = model.Inspection_date,
                            Status = model.Inspection_Type,
                        }).ToList();
            }
            catch (Exception ex)
            {

            }

            return View(inwardDataModel);
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddMrbdata(mrbmainmodel _model, string[] ProductNames)
        {
            try
            {
                try
                {
                    if (_model != null)
                    {
                        string Comasepretedrcode = string.Join(", ", ProductNames);

                        Final_Inspection_Mrb_Data  _Inspection_Data = new Final_Inspection_Mrb_Data();

                        _Inspection_Data.RCode = Comasepretedrcode;
                        _Inspection_Data.Desccription = Comasepretedrcode;
                       // _Inspection_Data.location = _model.Location;
                        //_Inspection_Data.SerialNo = _model.Serialno;
                        _Inspection_Data.Active = true;
                        _Inspection_Data.Deleted = false;
                       // DB.Final_Inspection_Mrb_Data.Add(_Inspection_Data);
                        //DB.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            return View();
        }
        public ActionResult MrbData()
        {
            List<MrbModel> List = new List<MrbModel>(); 

            try
            {
                List = (from model in DB.Final_Inspection_Mrb_Data.OrderByDescending(p => p.ID)
                        select new MrbModel
                        {
                            Id = model.ID,
                            Serialno = model.SerialNo, 
                           // Qty = model.Qty,
                            Rcodes = model.RCode,
                            Description = model.RCode,
                            Location = model.location,
                        }
                        ).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }

            return PartialView("_MrbDataList", List);
        }
        [Authorize]
        public ActionResult Mrbform(int id)
        {
            MrbModel _model = new MrbModel();
            List<MrbModel> _LIst = new List<MrbModel>();
            mrbmainmodel _Model  = new mrbmainmodel();

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
                            status = model.status,
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
                              status = model.status,
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

                return PartialView("_AddMRb", list);
            }
            catch (Exception ex)
            {
                throw;
            }

           
        }
    }
}
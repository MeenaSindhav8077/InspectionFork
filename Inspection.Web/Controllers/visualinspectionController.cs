using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class visualinspectionController : Controller
    {
        // GET: visualinspection
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        InwardDataModel List = new InwardDataModel();
        List<Submodel> _List = new List<Submodel>();

        [Authorize]
        public ActionResult Index(int? id, string value) 
        {
            try
            {
                if (id != null && value != null)
                {
                    Session["uid"] = id;
                    Session["stage"] = value;
                }
                else
                {
                    id = id = Convert.ToInt32(Session["uid"]);
                    value = value = Session["stage"].ToString();
                }


                List = (from model in DB.Final_Inspection_Data.Where(p => p.ID == id)
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
                            currentstage = value,
                            _submodel = new Submodel(),
                        }
                        ).FirstOrDefault();


                _List = (from model in DB.Final_Inspection_Process.Where(p => p.PID == id)
                        select new Submodel
                        {
                            id = model.ID,
                            inspectiondate = model.Inspection_date,
                            StartTime = model.starttime,
                            EndTime = model.endtime,
                            InspectedQty = model.Inspection_Qty,
                            InspectionBy = model.done_by,
                        }
                       ).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            var maininwarddata = new mAINPROGRESSModel
            {
                _INWARD = List,
                SUBMODEL =_List,

            };

            return View(maininwarddata);
        }
        [Authorize]
        public ActionResult _Index()
        {
            try
            {
                InwardDataModel model = new InwardDataModel();


                //List = (from models in DB.Final_Inspection_Process
                //        select new InwardDataModel.submodel
                //        {
                //            id = model.ID,
                //            InwardTime = model.Inward_Time,
                //            InwardDate = model.Inward_Date,
                //            JobNo = model.JobNum,
                //            Partno = model.PartNum,
                //            Stage = model.Stage,
                //            ERev = model.EpiRev,
                //            ActualRev = model.ActRev,
                //            Qty = model.Qty,
                //            Status = model.Status,
                //            currentstage = value,
                //        }
                //        ).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        [Authorize]
        public ActionResult AddData(mAINPROGRESSModel _model)
        {
            try
            {

                int id = Convert.ToInt32(Session["uid"]);
                int ID = DB.Final_Inspection_Process.Where(p => p.MID == 1).Count() > 0 ? Convert.ToInt32(DB.Final_Inspection_Process.Where(p => p.MID == 1).Max(p => p.ID) + 1) : 1;

                if (_model != null)
                {
                    Final_Inspection_Process _Inspection_Data = new Final_Inspection_Process();
                    _Inspection_Data.PID = id;
                    _Inspection_Data.ID = ID;
                    _Inspection_Data.MID = 1;
                    _Inspection_Data.Inspection_ID = "2";
                    _Inspection_Data.Rework_Id = 0;
                    _Inspection_Data.Inspection_date = _model._submodel.inspectiondate;
                    _Inspection_Data.starttime = _model._submodel.StartTime;
                    _Inspection_Data.endtime = _model._submodel.EndTime;
                    _Inspection_Data.Inspection_Qty = Convert.ToInt32(_model._submodel.InspectedQty);
                    _Inspection_Data.done_by = _model._submodel.InspectionBy;
                    _Inspection_Data.JobNum = _model._INWARD.JobNo;
                    _Inspection_Data.PartNum = _model._INWARD.Partno;
                    _Inspection_Data.status = _model._INWARD.Status;
                    _Inspection_Data.Stage = _model._INWARD.Stage;
                    _Inspection_Data.Inspection_Type = _model._INWARD.Status;
                    DB.Final_Inspection_Process.Add(_Inspection_Data);
                    DB.SaveChanges();

                    TempData["SuccessMessage"] = "Data saved successfully.";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
            }
            return RedirectToAction("Index");
        }
      
        [HttpPost]
        public ActionResult ENDTIMEQTY(string id, string endtime , string qty)//, string EndTime, int InspectedQty, string JobNo
        {
            try
            {
                if (id != null && endtime != null && qty != null)
                {
                    int ID = Convert.ToInt32(id);
                    Final_Inspection_Process _Inspection_Data = DB.Final_Inspection_Process.Where(V => V.ID == ID).FirstOrDefault();
                    if (_Inspection_Data != null)
                    {
                        _Inspection_Data.endtime = endtime;
                        _Inspection_Data.Inspection_Qty = Convert.ToInt32(qty);
                        DB.SaveChanges();
                    } 
                    TempData["SuccessMessage"] = "Data saved successfully.";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
            }
            return RedirectToAction("Index");
        }
    }
}
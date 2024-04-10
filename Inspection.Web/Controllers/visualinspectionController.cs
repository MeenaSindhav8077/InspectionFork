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
        List<InwardDataModel> _data = new List<InwardDataModel>();
        List<Submodel> _List = new List<Submodel>();
        

       [Authorize]
        public ActionResult Index(int? id, string value, string jobno)
         {

            int? totalfinalInspectedQty = null;
            int? totalvisualInspectedQty = null;
            int? totaltharedInspectedQty = null;
            bool? finalsts = false;
            bool? visualsts = false;
            bool? thareadsts = false;
            bool? fhumiditysts = false;
            int? totalhumidityInspectedQty = null;

            try
            {
                var finalqty = "";
                var visualqty = "";
                var thareadqty = "";
                var fhumidityqty = "";
                

                if (id != null && value != null)
                {
                    Session["uid"] = id;
                    Session["stage"] = value;
                    Session["jobno"] = jobno;
                }
                else
                {
                    id = id = Convert.ToInt32(Session["uid"]);
                    value = value = Session["stage"].ToString();
                    jobno = jobno = Session["jobno"].ToString();
                }
                var dataList = DB.Final_Inspection_Data.Where(p => p.JobNum == jobno).ToList();
                if (dataList != null && dataList.Count > 0)
                {
                    foreach (var data in dataList)
                    {
                        if (data.Inspection_Type == "Final")
                        {
                            finalqty = data.Sample_Qty;
                            finalsts = data.Statuschange;
                        }
                        else if (data.Inspection_Type == "Visual")
                        {
                            visualqty = data.Sample_Qty;
                            visualsts = data.Statuschange;
                        }
                        else if (data.Inspection_Type == "Thread")
                        {
                            thareadqty = data.Sample_Qty;
                            thareadsts = data.Statuschange;
                        }
                        else if (data.Inspection_Type == "Humidity")
                        {
                            fhumidityqty = data.Sample_Qty;
                            fhumiditysts = data.Statuschange;
                        }
                    }
                }
                List = (from model in DB.Final_Inspection_Data.Where(p => p.ID == id)
                        select new InwardDataModel
                        {
                            id = model.ID,
                            InwardTime = model.Inward_Time,
                            InwardDate = model.Inward_Date,
                            JobNo = model.JobNum,
                            Partno = model.PartNum,
                            ProcessStage = model.Stage,
                            ERev = model.EpiRev,
                            ActualRev = model.ActRev,
                            Qty = model.qty,
                            InspectionType = model.Inspection_Type,
                            Statuschange = model.Statuschange,
                            SampleQuantity = model.Sample_Qty,
                            currentstage = value,
                            finalinspection = finalqty,
                            visualinspection = visualqty,
                            threadinspection = thareadqty,
                            humidity = fhumidityqty,
                        }
                        ).FirstOrDefault();


                _List = (from model in DB.Final_Inspection_Process.Where(p => p.JobNum == jobno)
                         select new Submodel
                         {
                             id = model.ID,
                             inspectiondate = model.Inspection_date,
                             StartTime = model.starttime,
                             EndTime = model.endtime,
                             InspectedQty = model.Inspection_Qty,
                             InspectionBy = model.done_by,
                             InspectionTYPE = model.Inspection_Type,
                         }
                       ).ToList();

                totalfinalInspectedQty = DB.Final_Inspection_Process.Where(p => p.JobNum == jobno && p.Inspection_Type == "Final").Sum(p => p.Inspection_Qty);
                totaltharedInspectedQty = DB.Final_Inspection_Process.Where(p => p.JobNum == jobno && p.Inspection_Type == "Thread").Sum(p => p.Inspection_Qty);
                totalvisualInspectedQty = DB.Final_Inspection_Process.Where(p => p.JobNum == jobno && p.Inspection_Type == "Visual").Sum(p => p.Inspection_Qty);
                totalhumidityInspectedQty = DB.Final_Inspection_Process.Where(p => p.JobNum == jobno && p.Inspection_Type == "Humidity").Sum(p => p.Inspection_Qty);

                if (jobno != null)
                {
                    _data = (from model in DB.Final_Inspection_Data.Where(l => l.JobNum == jobno)
                             select new InwardDataModel
                             {
                                 id = model.ID,
                                 InwardTime = model.Inward_Time,
                                 InwardDate = model.Inward_Date,
                                 JobNo = model.JobNum,
                                 Partno = model.PartNum,
                                 ProcessStage = model.Stage,
                                 ERev = model.EpiRev,
                                 ActualRev = model.ActRev,
                                 Qty = model.qty,
                                 InspectionType = model.Inspection_Type,
                                 Statuschange = model.Statuschange,
                                 SampleQuantity = model.Sample_Qty,
                                 currentstage = value,
                             }
                       ).ToList();
                }

            }
            catch (Exception)
            {

            }
            var maininwarddata = new mAINPROGRESSModel
            {
                _INWARD = List,
                SUBMODEL = _List,
                TOTALfinalQTY = totalfinalInspectedQty.ToString(),
                TOTALhumidityQTY = totalhumidityInspectedQty.ToString(),
                TOTALtharedQTY = totaltharedInspectedQty.ToString(),
                TOTALvisualQTY = totalvisualInspectedQty.ToString(),
                finalstatus   = finalsts,
                visualstatus  = visualsts,
                tharedstatus  = thareadsts,
                humiditystatus = fhumiditysts,



                _INWARDList = _data,

            };
            return View(maininwarddata);
        }
        [Authorize]
        public ActionResult _Index()
        {
            try
            {
                InwardDataModel model = new InwardDataModel();
                
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

                if (_model._submodel != null)
                {

                    int _ID =0;
                    var inspectionData = DB.Final_Inspection_Data.Where(l => l.JobNum == _model._INWARD.JobNo && l.Inspection_Type == _model._INWARD.InspectionType).FirstOrDefault();
                    if (inspectionData != null)
                    {
                        _ID = inspectionData.ID;
                    }

                    Final_Inspection_Process _Inspection_Data = new Final_Inspection_Process();
                    _Inspection_Data.PID = _ID;
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
                    _Inspection_Data.Statuschange = false;
                    _Inspection_Data.Stage = _model._INWARD.ProcessStage;
                    _Inspection_Data.Inspection_Type = _model._INWARD.InspectionType;
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
        public ActionResult ENDTIMEQTY(string id, string endtime, string qty , string instype)
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
                    TempData["inspectiontype"] = instype;

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
            }
            return RedirectToAction("Index");
        }

        public ActionResult statuschange(mAINPROGRESSModel _model)
        {
            try
            {
                if (_model != null)
                {
                    int ID = _model._INWARD.id;
                    Final_Inspection_Data _Inspection_Data = DB.Final_Inspection_Data.Where(V => V.JobNum == _model._INWARD.JobNo && V.Inspection_Type == _model._INWARD.InspectionType).FirstOrDefault();
                    if (_Inspection_Data != null)
                    {
                        _Inspection_Data.Stage = _model._submodel.Stage;
                        _Inspection_Data.Statuschange = true;
                    }
                    bool mrb = false;
                    bool rework = false;
                    bool sorting = false;
                    bool deviation = false;
                    bool packing = false;
                    List<Final_Inspection_Process> _I_Data = DB.Final_Inspection_Process.Where(V => V.JobNum == _model._INWARD.JobNo && V.Inspection_Type == _model._INWARD.InspectionType).ToList();
                    if (_I_Data != null)
                    {
                        if (_model._submodel.Stage.Trim() == "2 - Parts waiting for MRB")
                        {
                            mrb = true;
                        }
                        else if (_model._submodel.Stage.Trim() == "5 - Parts in rework" || _model._submodel.Stage.Trim() == "4 - Parts waiting for rework")
                        {
                            rework = true;
                        }
                        else if (_model._submodel.Stage.Trim() == "3 - Parts waitng for sorting")
                        {
                            sorting = true;
                        }
                        else if (_model._submodel.Stage.Trim() == "7 - Parts in deviation")
                        {
                            deviation = true;
                        }
                        else if (_model._submodel.Stage.Trim() == "10 - Parts Ready For packing")
                        {
                            packing = true;
                        }
                        foreach (var item in _I_Data)
                        {
                            item.Stage = _model._submodel.Stage;
                            item.Statuschange = true;
                            item.waitingMRB = mrb; 
                            item.waitngsorting = sorting; 
                            item.deviation = deviation; 
                            item.reworkWAITING = rework; 
                            item.packingwaiting = packing; 
                        }
                    }
                    DB.SaveChanges();

                    TempData["SuccessMessage"] = "Stage Change successfully.";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["WarningMessage"] = "Warning: Something went wrong Data Not Saved.";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Addsampleqty(mAINPROGRESSModel _model)
        {
            try
            {
                if (_model != null)
                {
                    int ID = _model._INWARD.id;
                    Final_Inspection_Data _Inspection_Data = DB.Final_Inspection_Data.Where(V => V.JobNum == _model._INWARD.JobNo && V.Inspection_Type == _model._INWARD.typevalue).FirstOrDefault();
                    if (_Inspection_Data != null)
                    {
                        _Inspection_Data.Sample_Qty = _model._submodel.SampleQuantity;
                        _Inspection_Data.MES = _model._submodel.MES;
                    }
                    DB.SaveChanges();

                    TempData["SuccessMessage"] = "Data saved Successfually.";

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
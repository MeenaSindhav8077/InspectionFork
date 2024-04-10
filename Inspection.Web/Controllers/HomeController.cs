using Inspection.Web.DataBase;
using Inspection.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        public ActionResult Index()
        {
            MainhOMEInwardModel maindata = new MainhOMEInwardModel();
            List<HOMEmODEL> finalCounts = new List<HOMEmODEL>();
            HOMEmODEL _modelfinal = new HOMEmODEL();
            HOMEmODEL _modelvisual  = new HOMEmODEL();
            HOMEmODEL _modelthared = new HOMEmODEL();
            HOMEmODEL _modelhumidity = new HOMEmODEL();


            try
            {
                // Fetch data from the database
                var stages = DB.Final_Inspection_Stage_Master.ToList();
                var finalInspectionData = DB.Final_Inspection_Data.ToList();

                // Filter data by inspection type
                var FINAL = finalInspectionData.Where(p => p.Inspection_Type == "Final").ToList();
                var VISUAL = finalInspectionData.Where(p => p.Inspection_Type == "Visual").ToList();
                var THARED = finalInspectionData.Where(p => p.Inspection_Type == "Thread").ToList();
                var HUMIDITY = finalInspectionData.Where(p => p.Inspection_Type == "Humidity").ToList();

                // Define a function to calculate counts
                Func<List<Final_Inspection_Data>, string, int> calculateCount = (list, stage) =>
                {
                    return list.Where(k => k.Stage == stage).Count();
                };

                _modelfinal.PartsWaitingForFinalCount = FINAL.Where(k => k.Stage.Trim() == "1 - Parts Waiting For Final").Count();
                _modelfinal.PartsWaitingForMRBCount   = FINAL.Where(k => k.Stage.Trim()== "2 - Parts waiting for MRB").Count();
                _modelfinal.PartsWaitingForSortingCount = FINAL.Where(k => k.Stage.Trim()== "3 - Parts waitng for sorting").Count();
                _modelfinal.PartsWaitingForReworkCount = FINAL.Where(k => k.Stage.Trim()== "4 - Parts waiting for rework").Count();
                _modelfinal.PartsinreworkCount = FINAL.Where(k => k.Stage.Trim()== "5 - Parts in rework").Count();
                _modelfinal.ReworkcompleteandwaitingforinspectionCount = FINAL.Where(k => k.Stage.Trim()== "6 - Rework complete and waiting for inspection").Count();
                _modelfinal.Partsindeviationcount = FINAL.Where(k => k.Stage.Trim()== "7 - Parts in deviation").Count();
                _modelfinal.PartdonothaveunitpriceandrevissueCount = FINAL.Where(k => k.Stage.Trim()== "8 - Part donot have unit price and rev issue").Count();
                _modelfinal.PartsInspectioncompletedandwaitingforfilecompleteCount = FINAL.Where(k => k.Stage.Trim()== "9 - Parts Inspection completed and waiting for file complete").Count();
                _modelfinal.PartsReadyForpackingCount = FINAL.Where(k => k.Stage.Trim()== "10 - Parts Ready For packing").Count();
                _modelfinal.PartsmovedfromqualityCount = FINAL.Where(k => k.Stage.Trim()== "11 - Parts moved from quality").Count();
                _modelfinal.PartsWaitingForHumidityCount = FINAL.Where(k => k.Stage.Trim() == "12 -  Parts Waiting For Humidity").Count();
                maindata.Final = _modelfinal;

                maindata.finalpendinginspection = _modelfinal.PartsWaitingForFinalCount + _modelfinal.PartsWaitingForSortingCount + _modelfinal.ReworkcompleteandwaitingforinspectionCount;


                _modelvisual.PartsWaitingForFinalCount = VISUAL.Where(k => k.Stage.Trim() == "1 - Parts Waiting For Final").Count();
                _modelvisual.PartsWaitingForMRBCount = VISUAL.Where(k => k.Stage.Trim() == "2 - Parts waiting for MRB").Count();
                _modelvisual.PartsWaitingForSortingCount = VISUAL.Where(k => k.Stage.Trim() == "3 - Parts waitng for sorting").Count();
                _modelvisual.PartsWaitingForReworkCount = VISUAL.Where(k => k.Stage.Trim() == "4 - Parts waiting for rework").Count();
                _modelvisual.PartsinreworkCount = VISUAL.Where(k => k.Stage.Trim() == "5 - Parts in rework").Count();
                _modelvisual.ReworkcompleteandwaitingforinspectionCount = VISUAL.Where(k => k.Stage.Trim() == "6 - Rework complete and waiting for inspection").Count();
                _modelvisual.Partsindeviationcount = VISUAL.Where(k => k.Stage.Trim() == "7 - Parts in deviation").Count();
                _modelvisual.PartdonothaveunitpriceandrevissueCount = VISUAL.Where(k => k.Stage.Trim() == "8 - Part donot have unit price and rev issue").Count();
                _modelvisual.PartsInspectioncompletedandwaitingforfilecompleteCount = VISUAL.Where(k => k.Stage.Trim() == "9 - Parts Inspection completed and waiting for file complete").Count();
                _modelvisual.PartsReadyForpackingCount = VISUAL.Where(k => k.Stage.Trim() == "10 - Parts Ready For packing").Count();
                _modelvisual.PartsmovedfromqualityCount = VISUAL.Where(k => k.Stage.Trim() == "11 - Parts moved from quality").Count();
                _modelvisual.PartsWaitingForHumidityCount = VISUAL.Where(k => k.Stage.Trim() == "12 -  Parts Waiting For Humidity").Count();
                maindata.Visual = _modelvisual;

                maindata.visualpendinginspection = _modelvisual.PartsWaitingForFinalCount + _modelvisual.PartsWaitingForSortingCount + _modelvisual.ReworkcompleteandwaitingforinspectionCount;

                _modelthared.PartsWaitingForFinalCount                              = THARED.Where(k => k.Stage.Trim() == "1 - Parts Waiting For Final").Count();
                _modelthared.PartsWaitingForMRBCount                                = THARED.Where(k => k.Stage.Trim() == "2 - Parts waiting for MRB").Count();
                _modelthared.PartsWaitingForSortingCount                            = THARED.Where(k => k.Stage.Trim() == "3 - Parts waitng for sorting").Count();
                _modelthared.PartsWaitingForReworkCount                             = THARED.Where(k => k.Stage.Trim() == "4 - Parts waiting for rework").Count();
                _modelthared.PartsinreworkCount                                     = THARED.Where(k => k.Stage.Trim() == "5 - Parts in rework").Count();
                _modelthared.ReworkcompleteandwaitingforinspectionCount             = THARED.Where(k => k.Stage.Trim() == "6 - Rework complete and waiting for inspection").Count();
                _modelthared.Partsindeviationcount                                  = THARED.Where(k => k.Stage.Trim() == "7 - Parts in deviation").Count();
                _modelthared.PartdonothaveunitpriceandrevissueCount                 = THARED.Where(k => k.Stage.Trim() == "8 - Part donot have unit price and rev issue").Count();
                _modelthared.PartsInspectioncompletedandwaitingforfilecompleteCount = THARED.Where(k => k.Stage.Trim() == "9 - Parts Inspection completed and waiting for file complete").Count();
                _modelthared.PartsReadyForpackingCount                              = THARED.Where(k => k.Stage.Trim() == "10 - Parts Ready For packing").Count();
                _modelthared.PartsmovedfromqualityCount                             = THARED.Where(k => k.Stage.Trim() == "11 - Parts moved from quality").Count();
                _modelthared.PartsWaitingForHumidityCount = THARED.Where(k => k.Stage.Trim() == "12 -  Parts Waiting For Humidity").Count();
                maindata.Thread = _modelthared;

                maindata.tharedpendinginspection = _modelthared.PartsWaitingForFinalCount + _modelthared.PartsWaitingForSortingCount + _modelthared.ReworkcompleteandwaitingforinspectionCount;

                _modelhumidity.PartsWaitingForFinalCount                              = HUMIDITY.Where(k => k.Stage.Trim() == "1 - Parts Waiting For Final").Count();
                _modelhumidity.PartsWaitingForMRBCount                                = HUMIDITY.Where(k => k.Stage.Trim() == "2 - Parts waiting for MRB").Count();
                _modelhumidity.PartsWaitingForSortingCount                            = HUMIDITY.Where(k => k.Stage.Trim() == "3 - Parts waitng for sorting").Count();
                _modelhumidity.PartsWaitingForReworkCount                             = HUMIDITY.Where(k => k.Stage.Trim() == "4 - Parts waiting for rework").Count();
                _modelhumidity.PartsinreworkCount                                     = HUMIDITY.Where(k => k.Stage.Trim() == "5 - Parts in rework").Count();
                _modelhumidity.ReworkcompleteandwaitingforinspectionCount             = HUMIDITY.Where(k => k.Stage.Trim() == "6 - Rework complete and waiting for inspection").Count();
                _modelhumidity.Partsindeviationcount                                  = HUMIDITY.Where(k => k.Stage.Trim() == "7 - Parts in deviation").Count();
                _modelhumidity.PartdonothaveunitpriceandrevissueCount                 = HUMIDITY.Where(k => k.Stage.Trim() == "8 - Part donot have unit price and rev issue").Count();
                _modelhumidity.PartsInspectioncompletedandwaitingforfilecompleteCount = HUMIDITY.Where(k => k.Stage.Trim() == "9 - Parts Inspection completed and waiting for file complete").Count();
                _modelhumidity.PartsReadyForpackingCount                              = HUMIDITY.Where(k => k.Stage.Trim() == "10 - Parts Ready For packing").Count();
                _modelhumidity.PartsmovedfromqualityCount                             = HUMIDITY.Where(k => k.Stage.Trim() == "11 - Parts moved from quality").Count();
                _modelhumidity.PartsWaitingForHumidityCount = HUMIDITY.Where(k => k.Stage.Trim() == "12 - Parts Waiting For Humidity").Count();
                maindata.Humidity = _modelhumidity;

                maindata.humiditypendinginspection = _modelhumidity.PartsWaitingForHumidityCount + _modelhumidity.PartsWaitingForSortingCount + _modelhumidity.ReworkcompleteandwaitingforinspectionCount;
            }
            catch (Exception)
            {

            }
           
            return View(maindata);
        }

        public ActionResult Getdatatables(string Type, string stage)
        {
            int stgeno = Convert.ToInt32(stage);
            string _Stage  = DB.Final_Inspection_Stage_Master.Where(l=>l.Stage == stgeno).Select(l=>l.stage_part_status).FirstOrDefault();

            List<Final_Inspection_Data> _Data =DB.Final_Inspection_Data.Where(p=>p.Inspection_Type == Type && p.Stage == _Stage.Trim()).ToList();


            return PartialView("Getdatatables", _Data);
        }

        public ActionResult PartHistory()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
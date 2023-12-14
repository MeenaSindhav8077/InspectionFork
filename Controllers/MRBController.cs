using Inspection.Web.DataBase;
using Inspection.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Controllers
{
    public class MRBController : Controller
    {
        // GET: MRB
        ITe_INDIAEntities DB = new ITe_INDIAEntities();
        public ActionResult Index()
        {
            MrbModel model = new MrbModel();
            ///model.Rcode = new List<string>(); // Initialize the Rcode list
                                              // Additional initialization if needed

            return View(model);
        }
        [HttpPost]
        public ActionResult AddMrbdata(MrbModel _model, string[] ProductNames)
        {
            try
            {
                try
                {
                    if (_model != null)
                    {

                        string Comasepretedrcode = string.Join(", ", ProductNames);

                        Final_Inspection_Mrb_Data  _Inspection_Data = new Final_Inspection_Mrb_Data();


                        _Inspection_Data.Qty = _model.Qty;
                        _Inspection_Data.RCode = Comasepretedrcode;
                        _Inspection_Data.Desccription = Comasepretedrcode;
                        _Inspection_Data.location = _model.Location;
                        _Inspection_Data.SerialNo = _model.Serialno;
                        _Inspection_Data.Active = true;
                        _Inspection_Data.Deleted = false;
                        DB.Final_Inspection_Mrb_Data.Add(_Inspection_Data);
                        DB.SaveChanges();

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
                            Qty = model.Qty,
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
    }
}
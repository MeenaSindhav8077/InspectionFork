using Inspection.Web.DataBase;
using Inspection.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Helpers;
using Inspection.Web.Service;
using System.Reflection.Metadata;
using System.Drawing;
using System.Collections;
using System.Web.UI.WebControls;

namespace Inspection.Web.Controllers
{
    [Authorize]
    public class DocumentVerificationController : Controller
    {
        // GET: DocumentVerification
        ITe_INDIAEntities1 DB = new ITe_INDIAEntities1();
        LogService logService = new LogService();
        Maineservice _service = new Maineservice();
        [Authorize]
        public ActionResult Index(int id)
        {
            Dmainmodel dmainmodel = new Dmainmodel(); 
            InspectionModel _model = new InspectionModel();
            List<Docmodel> _Dmodel = new List<Docmodel>();

            Final_Inspection_Data final_Inspection_Data = DB.Final_Inspection_Data.Where(p => p.ID == id && p.Inspection_Type == "Final").FirstOrDefault();
            
            dmainmodel.Id = id;
            dmainmodel.jobno = final_Inspection_Data.JobNum;
            dmainmodel.partno = final_Inspection_Data.PartNum;
            dmainmodel.qty = final_Inspection_Data.Inspection_Qty;

            _Dmodel = (from modal in DB.Final_Inspection_Document_Data
                       join verification in DB.Final_Inspection_Document_Varification
                           on modal.ID equals verification.DocumentID into verifications
                       from verification in verifications.DefaultIfEmpty()
                       where verification.IID == id
                       select new Docmodel
                       {
                           Id = modal.ID,
                           Documentname = modal.DocumentName,
                           varificationinstuction = modal.VerifiactionInstruction,
                           comment = verification != null ? verification.Comments : modal.Comments,
                           passfail = verification != null ? verification.Passfail : modal.Passfail,
                           aftercorectionpassfail = verification != null ? verification.AfterCorrectionpassfail : modal.AfterCorrectionpassfail,
                       }).ToList();

            if (_Dmodel.Count == 0 && _Dmodel != null)
            {
                _Dmodel = (from modal in DB.Final_Inspection_Document_Data
                           select new Docmodel
                           {
                               Id = modal.ID,
                               Documentname = modal.DocumentName,
                               varificationinstuction = modal.VerifiactionInstruction,
                               comment = modal.Comments,
                               passfail = modal.Passfail,
                               aftercorectionpassfail = modal.AfterCorrectionpassfail,
                           }).ToList();
            }
            dmainmodel.docmodels = _Dmodel;
            return View(dmainmodel);
        }
        public ActionResult SaveDecision(Dmainmodel model)
        {

            if (model != null)
            {
                Final_Inspection_Data _Inspection_Data = DB.Final_Inspection_Data.Where(p=>p.ID == model.Id).FirstOrDefault();
                
                string jobno = DB.Final_Inspection_Data.Where(p => p.ID == model.Id).Select(v => v.JobNum).FirstOrDefault();
                string stage = DB.Final_Inspection_Data.Where(p => p.ID == model.Id).Select(v => v.Stage).FirstOrDefault();

                List<Final_Inspection_Data> Inspection_Data = DB.Final_Inspection_Data
                    .Where(v => v.JobNum == jobno && (v.QualityStage == stage || (v.QualityStage.Contains(stage))) && (v.closerequest == false || v.closerequest == null) && v.Active == true && v.Delete == false)
                    .ToList();
                Final_Inspection_Document_Varification _data = DB.Final_Inspection_Document_Varification.Where(p => p.IID == model.Id).FirstOrDefault();
                if (_data != null)
                {
                    foreach (var item in model.docmodels)
                    {

                        Final_Inspection_Document_Varification _fdata = DB.Final_Inspection_Document_Varification.Where(p => p.DocumentID == item.Id && p.IID == model.Id).FirstOrDefault();
                             

                        if (item.aftercorectionpassfail != null)
                        {
                            _fdata.AfterCorrectionpassfail = item.aftercorectionpassfail;
                            DB.SaveChanges();
                        }
                    }
                }
                else
                {
                    Final_Inspection_Document_Varification _Document_Varification = new Final_Inspection_Document_Varification();

                    foreach (var item in model.docmodels)
                    {
                        _Document_Varification.IID = model.Id;
                        _Document_Varification.DocumentID = item.Id;
                        _Document_Varification.Comments = item.comment;
                        _Document_Varification.Passfail = item.passfail;
                        _Document_Varification.Active = true;
                        _Document_Varification.Delete = false;
                        DB.Final_Inspection_Document_Varification.Add(_Document_Varification);
                        DB.SaveChanges();

                        List<string> _email = null;
                        if (item.passfail == "FAIL")
                        {
                            string email =  DB.Final_Inspection_Document_Data.Where(p => p.ID == item.Id).Select(v=>v.Email).FirstOrDefault();
                            if (email != null) {
                                _email =  email.Split(',').ToList();
                            }

                            _service.SendEmail(_email, item.Id, jobno, item.comment);
                        }
                    }
                }
                List<Final_Inspection_Document_Varification> _datalist = DB.Final_Inspection_Document_Varification.Where(p => p.IID == model.Id).ToList();
                bool initialAllPass = _datalist.All(d => d.Passfail == "PASS" || d.Passfail == "Not Applicable");
                bool allPassAfterCorrection = false;
                if (initialAllPass)
                {
                    _Inspection_Data.Stage = "10 - Parts Ready For Packing";
                    _Inspection_Data.readyforpacking = true;
                    _Inspection_Data.CloseRequstDate = DateTime.Now;
                    DB.SaveChanges();
                    Inspection_Data.ForEach(item => item.CloseRequstDate = DateTime.Now);
                    DB.SaveChanges();
                }
                else
                {
                    bool CHKallPassAfterCorrection =  _datalist.All(d => d.AfterCorrectionpassfail == null || d.AfterCorrectionpassfail == "PASS");
                    if (CHKallPassAfterCorrection) {

                        
                            _Inspection_Data.Stage = "10 - Parts Ready For Packing";
                            _Inspection_Data.readyforpacking = true;
                            _Inspection_Data.CloseRequstDate = DateTime.Now;
                            DB.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index" , new { id = model.Id});
        }
    }
}
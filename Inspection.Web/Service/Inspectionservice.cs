using Inspection.Web.DataBase;
using Inspection.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Scripts
{
    public class Inspectionservice
    {
        ITEIndiaEntities DB = new ITEIndiaEntities();
        public List<SelectListItem> GetInspectiontype()
        {

            List<SelectListItem> selectListItems = DB.Final_Inspection_Stage_Master.GroupBy(tol => tol.stage_part_status.Trim()).Select(group => group.FirstOrDefault())
                .Select(tol => new SelectListItem
                {
                    Value = tol.stage_part_status.ToString(),
                    Text = tol.stage_part_status
                }).ToList();

            return selectListItems;
        }
        public List<SelectListItem> GetInspectiontypewise(string _types)
        {
            List<string> valuesToExclude = new List<string>();
            List<SelectListItem> selectListItems = DB.Final_Inspection_Stage_Master.GroupBy(tol => tol.stage_part_status.Trim()).Select(group => group.FirstOrDefault())
               .Select(tol => new SelectListItem
               {
                   Value = tol.ID.ToString(),
                   Text = tol.stage_part_status
               }).ToList();

            if (_types == "Final")
            {
                valuesToExclude = new List<string> { "1 - Parts waiting for Thread", "1 - Parts waiting for Visual", "1 - Parts waiting for Humidity", "10 - Visual Inspection Completed", "10 - Thread Inspection Completed", "9 - Parts Ready To Next Operation" };

                selectListItems.RemoveAll(item => valuesToExclude.Contains(item.Text));
            }
            else if (_types == "Visual")
            {
                valuesToExclude = new List<string> { "1 - Parts waiting for Thread", "1 - Parts waiting for Final", "1 - Parts waiting for Humidity", "11 - Parts moved from Quality", "10 - Thread Inspection Completed", "10 - Parts Ready For Packing" };

                selectListItems.RemoveAll(item => valuesToExclude.Contains(item.Text));
            }
            else if (_types == "Thread")
            {
                valuesToExclude = new List<string> { "1 - Parts waiting for Final", "1 - Parts waiting for Visual", "1 - Parts waiting for Humidity", "10 - Visual Inspection Completed", "11 - Parts moved from Quality", "10 - Parts Ready For Packing", "9 - Parts inspection completed and waiting for file complete" };

                selectListItems.RemoveAll(item => valuesToExclude.Contains(item.Text));
            }
            else
            {
                valuesToExclude = new List<string> { "1 - Parts waiting for Thread", "1 - Parts waiting for Visual", "10 - Visual Inspection Completed", "10 - Thread Inspection Completed" };

                selectListItems.RemoveAll(item => valuesToExclude.Contains(item.Text));
            }
            return selectListItems;
        }
        public List<SelectListItem> Getuser()
        {
            ITEIndiaEntities DB = new ITEIndiaEntities();

            var names = new List<string>
    {
        "Hemang Pipariya",
        "Aagam Dasadiya",
        "Sandip Patil",
        "Dilshad Sumra",
        "Kaushik Makwana",
        "Chandrasinh Parmar",
        "Kartik Chauhan",
        "Hitesh Parmar",
        "Dharmik Khavadiya",
        "Kartik Parmar",
        "Savan Matariya",
        "Harshad Parmar",
        "Monika Satapara",
        "Kaushal Thakar",
        "Dilip Punani",
        "Dilip Chauhan",
        "Ghanshyam Aniyaliya"
    };

            List<SelectListItem> selectListItems = DB.user_data
                .Where(k => names.Contains(k.name))
                .GroupBy(tol => tol.name.Trim())
                .Select(group => group.FirstOrDefault())
                .Select(tol => new SelectListItem
                {
                    Value = tol.name.ToString(),
                    Text = tol.name
                }).ToList();

            return selectListItems;
       
        }

    public List<SelectListItem> GetRcodes()
    {
            ITEIndiaEntities DB = new ITEIndiaEntities();

        List<SelectListItem> selectListItems = DB.Final_Inspection_RCode.GroupBy(tol => tol.RCode.Trim()).Select(group => group.FirstOrDefault()).OrderBy(k => k.RCode)
            .Select(tol => new SelectListItem
            {
                Value = tol.ID.ToString(),
                Text = tol.RCode.ToString()
            }).OrderBy(p => p.Value).ToList();

        return selectListItems;
    }
    public List<SelectListItem> GetDescription()
    {
            ITEIndiaEntities DB = new ITEIndiaEntities();

        List<SelectListItem> selectListItems = DB.Final_Inspection_RCode.GroupBy(tol => tol.RCode.Trim()).Select(group => group.FirstOrDefault())
            .Select(tol => new SelectListItem
            {
                Value = tol.ID.ToString(),
                Text = tol.Description.ToString()
            }).ToList();

        return selectListItems;
    }

    //public List<SelectListItem> Getdesicion()
    //{
    //    ITe_INDIAEntities DB = new ITe_INDIAEntities();

    //    List<SelectListItem> selectListItems = DB.Final_Inspection_MRB_Decision.GroupBy(tol => tol.MRBDecision.Trim()).Select(group => group.FirstOrDefault())
    //        .Select(tol => new SelectListItem
    //        {
    //            Value = tol.ID.ToString(),
    //            Text = tol.MRBDecision.ToString()
    //        }).ToList();

    //    return selectListItems;
    //}


    public mrbmainmodel Getdataforpdf(int id)
    {

        MrbModel _model = new MrbModel();
        List<MrbModel> _LIst = new List<MrbModel>();
        List<MrbdecisioModel> _LIsts = new List<MrbdecisioModel>();
        mrbmainmodel _Model = new mrbmainmodel();
        var list = new mrbmainmodel();

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
                      }
                    ).FirstOrDefault();

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
                     }
                    ).ToList();

            _LIsts = (from model1 in DB.Final_Inspection_Mrb_Data.Where(p => p.Gid == id)
                      join model2 in DB.Final_Inspection_Mrb_Rcode on model1.ID equals model2.PID
                      into relatedRecords
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
                      }
                      ).ToList();
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
                          }
                     ).FirstOrDefault();
            }
            list = new mrbmainmodel
            {
                _MrbModel = _model,
                _MrbModellist = _LIst,
                mrbdecisioModel = _LIsts,

            };
        }
        catch (Exception)
        {
            throw;
        }
        return list;
    }

        //public List<SelectListItem> Getstatusdrp()
        //{
        //    ITe_INDIAEntities DB = new ITe_INDIAEntities();

        //    //List<SelectListItem> selectListItems = DB.Final_Inspection_Data.Where(p => p.Final_Inspection ?? false &&  p.Visual_Inspection ?? p.Thread_Inspection ?? p.Humidity).Tolist();
        //    //var selectListItems = DB.Final_Inspection_Data.Where()

        //    return selectListItems;
        //}


        public DataTable GetJobDetails(string idjobnumber)
        {
            DataTable dataTable = new DataTable();

            string _connectionString = "Data Source=SSWDB.SSWHITE.local;Initial Catalog=master;Persist Security Info=False;User ID=pluto;password=seki!kyu;Connection Timeout=300";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT JobNum, PartNum,RevisionNum FROM [KineticLiveDB].[SaaS1143_62653].[Erp].[Jobhead] WHERE JobNum = @idjobnumber"; // Removed 'custid' column

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Adding parameters
                    command.Parameters.AddWithValue("@idjobnumber", idjobnumber);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
    }
}
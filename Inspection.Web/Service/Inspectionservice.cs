using Inspection.Web.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Scripts
{
    public class Inspectionservice
    {
        public List<SelectListItem> GetInspectiontype()
        {
            ITe_INDIAEntities DB = new ITe_INDIAEntities();

            List<SelectListItem> selectListItems = DB.Final_Inspection_Stage_Master.GroupBy(tol => tol.stage_part_status.Trim()).Select(group => group.FirstOrDefault())
                .Select(tol => new SelectListItem
                {
                    Value = tol.stage_part_status.ToString(),
                    Text = tol.stage_part_status
                }).ToList();

            return selectListItems;
        }
        public List<SelectListItem> Getuser()
        {
            ITe_INDIAEntities DB = new ITe_INDIAEntities();

            List<SelectListItem> selectListItems = DB.user_data.GroupBy(tol => tol.name.Trim()).Select(group => group.FirstOrDefault())
                .Select(tol => new SelectListItem
                {
                    Value = tol.name.ToString(),
                    Text = tol.name
                }).ToList();

            return selectListItems;
        }

        public List<SelectListItem> GetRcodes()
        {
            ITe_INDIAEntities DB = new ITe_INDIAEntities();

            List<SelectListItem> selectListItems = DB.Final_Inspection_RCode.GroupBy(tol => tol.RCode.Trim()).Select(group => group.FirstOrDefault()).OrderBy(k=>k.RCode)
                .Select(tol => new SelectListItem
                {
                    Value = tol.ID.ToString(),
                    Text = tol.RCode.ToString()
                }).OrderBy(p=>p.Value).ToList();

            return selectListItems;
        }
        public List<SelectListItem> GetDescription()
        {
            ITe_INDIAEntities DB = new ITe_INDIAEntities();

            List<SelectListItem> selectListItems = DB.Final_Inspection_RCode.GroupBy(tol => tol.RCode.Trim()).Select(group => group.FirstOrDefault())
                .Select(tol => new SelectListItem
                {
                    Value = tol.ID.ToString(),
                    Text = tol.Description.ToString()
                }).ToList();

            return selectListItems;
        }

        public List<SelectListItem> Getdesicion()
        {
            ITe_INDIAEntities DB = new ITe_INDIAEntities();

            List<SelectListItem> selectListItems = DB.Final_Inspection_MRB_Decision.GroupBy(tol => tol.MRBDecision.Trim()).Select(group => group.FirstOrDefault())
                .Select(tol => new SelectListItem
                {
                    Value = tol.ID.ToString(),
                    Text = tol.MRBDecision.ToString()
                }).ToList();

            return selectListItems;
        }

        //public List<SelectListItem> Getstatusdrp()
        //{
        //    ITe_INDIAEntities DB = new ITe_INDIAEntities();

        //    //List<SelectListItem> selectListItems = DB.Final_Inspection_Data.Where(p => p.Final_Inspection ?? false &&  p.Visual_Inspection ?? p.Thread_Inspection ?? p.Humidity).Tolist();
        //    //var selectListItems = DB.Final_Inspection_Data.Where()

        //    return selectListItems;
        //}
    }
}
using Inspection.Web.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Service
{
    public class Maineservice
    {

        public List<SelectListItem> GetInspectiontype()
        {
            ITe_INDIAEntities DB = new ITe_INDIAEntities();

            var dbData = DB.Final_Inspection_Stage_Master.GroupBy(tol => tol.stage_part_status.Trim()).Select(group => group.FirstOrDefault()).ToList();

            List<SelectListItem> selectListItems = dbData.Select(tol => new SelectListItem
            {
                Value = tol.stage_part_status.ToString(),
                //Text = $"{tol.stage_part_status} - {tol.Stage}"
                Text = tol.stage_part_status.ToString(),
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
    }
}
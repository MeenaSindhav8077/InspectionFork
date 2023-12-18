using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Models
{
    public class InwardDataModel
    {
        public int id { get; set; }
        public string name { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        [DisplayFormat(DataFormatString = "{0:dd, MMM yyyy}")]
        public DateTime? InwardDate { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string InwardTime { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string JobNo { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string Partno { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string Stage { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string Status { get; set; }
        public List<string> MStatus { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string ERev { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string ActualRev { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string Qty { get; set; }

        public IEnumerable<SelectListItem> _Stage { get; set; }

        public string currentstage { get; set; }

        public Submodel _submodel { get; set; }
        public List<Submodel> _submodels { get; set; }

        public bool? finalinspection { get; set; }
        public bool? humidity { get; set; }
        public bool? threadinspection { get; set; }
        public bool? visualinspection { get; set; }
        public bool RequideMrb { get; set; }

        

    }
    public class Submodel
    {
        public int id { get; set; }

        public DateTime? inspectiondate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? InspectedQty { get; set; }
        public string InspectionBy { get; set; }
        public IEnumerable<SelectListItem> _User { get; set; }

    }

    public class MainInwardModel
    {
        public InwardDataModel _INWARD { get; set; }
        public List<InwardDataModel> _INWARDList { get; set; }

    }
    public class mAINPROGRESSModel
    {
        public InwardDataModel _INWARD { get; set; }
        public List<Submodel> SUBMODEL { get; set; }

        public Submodel _submodel { get; set; }

    }


}
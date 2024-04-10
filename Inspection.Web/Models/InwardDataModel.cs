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
        [Display(Name = "Inward Date")]
        public DateTime? InwardDate { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "Inward Time")]
        public string InwardTime { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string JobNo { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string Partno { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string ProcessStage { get; set; }
        public string QualityStage { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public bool? Statuschange { get; set; }
        public string InspectionType { get; set; }
        public List<string> MStatus { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string ERev { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string ActualRev { get; set; }

        [Required(ErrorMessage = "The  field is required.")]
        public string Qty { get; set; }

        public IEnumerable<SelectListItem> _Stage { get; set; }

        public string currentstage { get; set; }
        public string typevalue { get; set; }

        public Submodel _submodel { get; set; }
        public List<Submodel> _submodels { get; set; }

        public string finalinspection { get; set; }
        public string humidity { get; set; }
        public string threadinspection { get; set; }
        public string visualinspection { get; set; }
        public bool RequideMrb { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        public string SampleQuantity { get; set; }
        public int? IQTY { get; set; }
        public Dictionary<string, string> KeyValuePairs { get; set; }



    }
    public class Submodel
    {
        public int id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd, MMM yyyy}")]
        public DateTime? inspectiondate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int? InspectedQty { get; set; }
        public string InspectionBy { get; set; }
        public string InspectionTYPE { get; set; }
        public string Stage { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        public string SampleQuantity { get; set; }
        public string MES { get; set; }
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

        public string TOTALfinalQTY { get; set; }
        public string TOTALvisualQTY { get; set; }
        public string TOTALtharedQTY { get; set; }
        public string TOTALhumidityQTY { get; set; }


        public bool? finalstatus { get; set; }
        public bool? visualstatus { get; set; }
        public bool? tharedstatus { get; set; }
        public bool? humiditystatus { get; set; }

        public List<InwardDataModel> _INWARDList { get; set; }

    }


}
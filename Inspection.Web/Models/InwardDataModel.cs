using Org.BouncyCastle.Asn1.X509;
using PagedList;
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
        [DisplayFormat(DataFormatString = "{0:dd, MMM yyyy hh:mm tt}")]
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

        public int finalinspection { get; set; }
        public string humidity { get; set; }
        public string threadinspection { get; set; }
        public string visualinspection { get; set; }
        public bool RequideMrb { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        public string SampleQuantity { get; set; }
        public int? acceptqty { get; set; }
        public int? sortingqty { get; set; }
        public int? reworkqty { get; set; }
        public int? deviationqty { get; set; }
        public string Note { get; set; }
        public int? IQTY { get; set; }
        public Dictionary<string, string> KeyValuePairs { get; set; }
        public Decisionmodel decisionmodel {  get; set; }
        public string Supplier { get; set; }
        public string currentcard { get; set; }
        public bool? checkmrbdone { get; set; }

    }
    public class Submodel
    {
        public int id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd, MMM yyyy  hh:mm tt}")]
        public DateTime? inspectiondate { get; set; }
        [DisplayFormat(DataFormatString = "{hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? _StartTime { get; set; }
        public string StartTime { get; set; }
        public DateTime? EndTime { get; set; }
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
        public List<InwardDataModel> _INWARDListdata { get; set; }
        public List<Submodel> _submodel { get; set; }

    }
    public class mAINPROGRESSModel
    {
        public InwardDataModel _INWARD { get; set; }
        public List<Submodel> SUBMODEL { get; set; }

        public Submodel _submodel { get; set; }
        public int? TOTALreworkQTY { get; set; }
        public int? TOTALfinalQTY { get; set; }
        public string TOTALvisualQTY { get; set; }
        public string TOTALtharedQTY { get; set; }
        public string TOTALhumidityQTY { get; set; }
        public string TOTALAcceptqtyforfinalQTY { get; set; }


        public bool? finalstatus { get; set; }
        public bool? finalruning { get; set; }
        public bool? visualstatus { get; set; }
        public bool? tharedstatus { get; set; }
        public bool? humiditystatus { get; set; }
        public string currerntcard { get; set; }

        public List<InwardDataModel> _INWARDList { get; set; }
    }

    public class Decisionmodel
    {
        public int id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd, MMM yyyy}")]
        [Required(ErrorMessage = "The  field is required.")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        public string StartTime { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        public string OkQty { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        public string RejectQty { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        public string Remark { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        public string InspectionTYPE { get; set; }
        public string TYPE { get; set; }
        public string stage { get; set; }

    }

    public class reworkmodel
    {
        public Submodel _Reworkmodel { get; set; }
        public List<Submodel> _ReworkmodelList { get; set; }
    }


    public class AddDecisionmodel
    {
        public List<InwardDataModel> _INWARDList { get; set; }
        public Decisionmodel _submodeldata { get; set; }
       
        public reworkmodel _Mainreworkmodel { get; set; }
    }

    


    public class InspectionViewModel
    {
        public IEnumerable<InwardDataModel> Items { get; set; }

        public int PageNumber { get; set; } = 1; // Current page number
        public int PageSize { get; set; } = 10; // Items per page
        public int TotalRecords { get; set; } // Total records found
        public string Search { get; set; } // Search term

        // Additional properties if needed, e.g., sorting, filters, etc.
    }
}
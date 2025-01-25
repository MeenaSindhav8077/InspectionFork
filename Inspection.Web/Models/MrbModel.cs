using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspection.Web.Models
{
    public class MrbModel
    {
        public int Id { get; set; }
        public int iId { get; set; }
        public int MId { get; set; }

        public int  Serialno { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public decimal? Qty_qty { get; set; }
        public string Sampleqty { get; set; }
        public int? inspectionqty { get; set; }
        public string jobno { get; set; }
        public string partno { get; set; }
        public string inspectiontype { get; set; }
        public string stage { get; set; }
        public string status { get; set; }
        public List<string> Rcode { get; set; }
        public string Rcodes { get; set; }
        public DateTime? date { get; set; }
        public DateTime? MRbDate { get; set; }
        public string Location { get; set; }
        public string inspectedby { get; set; }
        public string qty { get; set; }
        public string subqty { get; set; }
        public string Qualitystage { get; set; }
        public string note { get; set; }

        public List<MrbDecisionViewModel> DecisionItems { get; set; }
    }

    public class MrbdecisioModel
    {
        public int Id { get; set; }
        public List<string> Rcode { get; set; }
        public List<string> Description { get; set; }
        public List<string> location { get; set; }
        public List<string> Desicion { get; set; }
        public List<string> subqty { get; set; }
        public List<string> inersubqty { get; set; }
        public List<int> ids { get; set; }
        public int Qtys { get; set; }
        public string jobno_j { get; set; }
        public string partno_p { get; set; }

         public List<int?>  Reject  { get; set; }
         public List<int?>  Accept  { get; set; }
         public List<int?>  Rework  { get; set; }
         public List<int?>  Sorting { get; set; }
         public List<int?>  Resorting  { get; set; }
         public List<int?>  Deviation  { get; set; }
         public List<int?>  ReworkMRB{ get; set; }
         public List<int?>  ReMeasured  { get; set; }
         public List<int?>  Split { get; set; }
         public List<int?> Hold { get; set; }
    }
    public class MrbDecisionViewModel
    {
        public string Decisionmrb { get; set; }
        public int SubQtyMrb { get; set; }
    }
    public class mrbmainmodel
    {
        public List<MrbModel> _MrbModellist { get; set; }
        public MrbModel _MrbModel { get; set; }
        public List<MrbdecisioModel> mrbdecisioModel { get; set; }
        public string inspectedby { get; set; }
    }



}
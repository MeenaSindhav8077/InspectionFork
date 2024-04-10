using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspection.Web.Models
{
    public class MrbModel
    {
        public int Id { get; set; }
        public int  Serialno { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public decimal? Qty { get; set; }
        public string jobno { get; set; }
        public string partno { get; set; }
        public string inspectiontype { get; set; }
        public string stage { get; set; }
        public string status { get; set; }

        public List<string> Rcode { get; set; }
        public string Rcodes { get; set; }
        public DateTime? date { get; set; }

        public string Location { get; set; }
        public string inspectedby { get; set; }
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
        public int Qty { get; set; }
        public string jobno { get; set; }
        public string partno { get; set; }
    }

    public class mrbmainmodel
    {
        public List<MrbModel> _MrbModellist { get; set; }
        public MrbModel _MrbModel { get; set; }

        public List<MrbdecisioModel> mrbdecisioModel { get; set; }

        public string[] _Mrbdecisio { get;  set; }

        public string Mrbdecisio { get; set; }
        public string mrbinersubqty { get; set; }

    }
}
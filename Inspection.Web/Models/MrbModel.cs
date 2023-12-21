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
        public int? Qty { get; set; }
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

    public class mrbmainmodel
    {
        public List<MrbModel> _MrbModellist { get; set; }
        public MrbModel _MrbModel { get; set; }

    }
}
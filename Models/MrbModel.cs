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
        public decimal Qty { get; set; }

        public List<string> Rcode { get; set; }
        public string Rcodes { get; set; }

        public string Location { get; set; }
    }
}
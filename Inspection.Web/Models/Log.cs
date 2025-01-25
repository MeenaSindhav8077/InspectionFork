using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspection.Web.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string PageName { get; set; }
        public int LineNumber { get; set; }
    }
}
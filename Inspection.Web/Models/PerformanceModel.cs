using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspection.Web.Models
{
    public class PerformanceModel
    {
        public double HumidityHours { get; set; }
        public double FinalHours { get; set; }
        public double VisualHours { get; set; }
        public double ThreadHours { get; set; }
        public double TotalHours { get; set; }

        public int? Humidityqty { get; set; }
        public int? Finalqty { get; set; }
        public int? Visualqty { get; set; }
        public int? Threadqty { get; set; }
        public int? Totalqty { get; set; }

        public string inspector { get; set; }
        public string DateRange { get; set; }

    }

    public class showdetails
    {
        public int id { get; set; }
        public string partno { get; set; }
        public string jobno { get; set; }
        public string startime { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? endtime { get; set; }
        public string sampleqty { get; set; }
        public string stage { get; set; }
        public string qualitystage { get; set; }
        public int? inspectionqty { get; set; }
        public string inspectiontype { get; set; }
        public string doneby { get; set; }

    }
}
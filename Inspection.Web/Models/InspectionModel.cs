using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspection.Web.Models
{
    public class InspectionModel
    {
        public int Id { get; set; } 


        public string Description { get; set; }

        public string qty { get; set; }
        public string jobno { get; set; }
        public string partno { get; set; }
        public string subqty { get; set; }

    }

    public class Docmodel
    {
        public int Id { get; set; }


        public string Documentname { get; set; }

        public string varificationinstuction { get; set; }
        public string comment { get; set; }
        [Required(ErrorMessage = "The  field is required.")]
        public string passfail { get; set; }
        public string aftercorectionpassfail { get; set; }

    }

    public class Dmainmodel
    {
        public int Id { get; set; }


        public string Description { get; set; }

        public string qty { get; set; }
        public string jobno { get; set; }
        public string partno { get; set; }
        public string subqty { get; set; }


        public List<Docmodel> docmodels { get; set; }
    }
    
}
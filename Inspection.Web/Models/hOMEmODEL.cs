using Inspection.Web.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Web;

namespace Inspection.Web.Models
{
    public class HOMEmODEL
    {

        public int PartsWaitingForFinalCount                               { get; set; }
        public int PartsWaitingForMRBCount                                { get; set; }
        public int PartsWaitingForSortingCount                             { get; set; }
        public int PartsWaitingForReworkCount                                 { get; set; }
        public int PartsinreworkCount                                             { get; set; }
        public int ReworkcompleteandwaitingforinspectionCount                         { get; set; }
        public int Partsindeviationcount                                              { get; set; }
        public int PartdonothaveunitpriceandrevissueCount                                     { get; set; }
        public int PartsInspectioncompletedandwaitingforfilecompleteCount           { get; set; }
        public int PartsReadyForpackingCount                                    { get; set; }
        public int PartsmovedfromqualityCount                                   { get; set; }
        public int PartsWaitingForHumidityCount                                     { get; set; }



    }
    public class MainhOMEInwardModel
    {
        public HOMEmODEL Final { get; set; }
        public HOMEmODEL Visual { get; set; }
        public HOMEmODEL Thread { get; set; }
        public HOMEmODEL Humidity { get; set; }

        public int finalpendinginspection { get; set; }
        public int visualpendinginspection { get; set; }
        public int tharedpendinginspection { get; set; }
        public int humiditypendinginspection { get; set; }

    }
}
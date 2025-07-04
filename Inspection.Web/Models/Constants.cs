namespace Inspection.Web.Models
{
    public static class StageConstants
    {
        public const string PartsWaitingForFinal = "1 - Parts waiting for Final";
        public const string PartsWaitingForVisual = "1 - Parts waiting for Visual";
        public const string PartsWaitingForThread = "1 - Parts waiting for Thread";
        public const string PartsWaitingForHumidity = "1 - Parts waiting for Humidity";
        public const string PartsWaitingForMRB = "2 - Parts waiting for MRB";
        public const string PartsWaitingForSorting = "3 - Parts waiting for Sorting";
        public const string PartsWaitingForRework = "4 - Parts waiting for Rework";
        public const string PartsInRework = "5 - Parts in Rework";
        public const string ReworkCompleteAndWaitingForInspection = "6 - Rework complete and waiting for inspection";
        public const string PartsInDeviation = "7 - Parts in Deviation";
        public const string PartsDontHaveUnitPriceAndRevIssue = "8 - Parts don't have unit price and rev issue"; // Used for Final and Humidity
        public const string PartsInReworkHumidity = "8 - Parts in Rework"; // Used for Humidity
        public const string PartsInspectionCompletedAndWaitingForFileComplete = "9 - Parts inspection completed and waiting for file complete"; // Used for Final and Humidity
        public const string PartsReadyToNextOperation = "9 - Parts Ready To Next Operation"; // Used for Visual and Thread
        public const string PartsReadyForPacking = "10 - Parts Ready For Packing";
        public const string VisualInspectionCompleted = "10 - Visual Inspection Completed";
        public const string ThreadInspectionCompleted = "10 - Thread Inspection Completed";
        public const string PartsMovedFromQuality = "11 - Parts moved from Quality"; // Used for Final
        public const string PartsMovedFromQualityHumidity = "14 - Parts moved from Quality"; // Used for Humidity
        public const string PartsInHold = "12 - Parts in Hold";
    }
}

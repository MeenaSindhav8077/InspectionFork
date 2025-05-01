using Inspection.Web.DataBase;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System.Web.UI.WebControls.WebParts;
using System.Web.Services.Description;
using System.Drawing;

namespace Inspection.Web.Controllers
{
    public class RatingTimeController : Controller
    {
        // GET: RatingTime
        ITEIndiaEntities DB = new ITEIndiaEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExportToExcel(string dateRange)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var dates = dateRange.Split(new[] { " to " }, StringSplitOptions.None);
                DateTime startDate = DateTime.ParseExact(dates[0], "dd MMM, yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dates[1], "dd MMM, yyyy", CultureInfo.InvariantCulture);

                List<Final_Inspection_Data> data = DB.Final_Inspection_Data.Where(p => p.Active == true && p.Delete == false && p.closerequest == true).ToList();
                List<Final_Inspection_Data> total = data.Where(p => p.Note != "Out Source Part").ToList();
                List<Final_Inspection_Data> final = data.Where(p => p.Inspection_Type == "Final").ToList();
                List<Final_Inspection_Data> visual = data.Where(p => p.Inspection_Type == "Visual").ToList();
                List<Final_Inspection_Data> Thared = data.Where(p => p.Inspection_Type == "Thread").ToList();
                List<Final_Inspection_Data> Humidity = data.Where(p => p.Inspection_Type == "Humidity").ToList();
                List<Final_Inspection_Data> incominginspection = data.Where(p => p.Note == "Out Source Part").ToList();


                data = data.Where(x => x.Inward_Date >= startDate && x.CloseRequstDate <= endDate).ToList();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Data Sheet");
                    var worksheet1 = package.Workbook.Worksheets.Add("Total");
                    var worksheet2 = package.Workbook.Worksheets.Add("Final");
                    var worksheet3 = package.Workbook.Worksheets.Add("visual");
                    var worksheet4 = package.Workbook.Worksheets.Add("Thread");
                    var worksheet5 = package.Workbook.Worksheets.Add("Humidity");
                    var worksheet6 = package.Workbook.Worksheets.Add("Incoming Inspection");
                    var worksheet7 = package.Workbook.Worksheets.Add("Sum of Rejection Code");

                    // for Data Sheet
                    try
                    {
                        worksheet.Cells[1, 1].Value = "Sr No";
                        worksheet.Cells[1, 2].Value = "Part No";
                        worksheet.Cells[1, 3].Value = "Rev.No";
                        worksheet.Cells[1, 4].Value = "Job NO";
                        worksheet.Cells[1, 5].Value = "Inward Qty";
                        worksheet.Cells[1, 6].Value = "Inspection type";
                        worksheet.Cells[1, 7].Value = "Quality Stage";
                        worksheet.Cells[1, 8].Value = "Supplier";
                        worksheet.Cells[1, 9].Value = "Inspection Qty";
                        worksheet.Cells[1, 10].Value = "Reject Qty";
                        worksheet.Cells[1, 11].Value = "Reject%";
                        worksheet.Cells[1, 12].Value = "Rework Qty";
                        worksheet.Cells[1, 13].Value = "Rework%";
                        worksheet.Cells[1, 14].Value = "Deviation Qty";
                        worksheet.Cells[1, 15].Value = "Deviation%";
                        worksheet.Cells[1, 16].Value = "Lot Reject";
                        worksheet.Cells[1, 17].Value = "Total time in  Quality";
                        worksheet.Cells[1, 18].Value = "Inspection time(Type) Manpower spend time";
                        worksheet.Cells[1, 19].Value = "Total inspection time- Manpower spend time ";
                        worksheet.Cells[1, 20].Value = "Inspection time(common) -Quality division spend time by part";
                        worksheet.Cells[1, 21].Value = "Total inspection time(Common)-quality division spend time by part";
                        worksheet.Cells[1, 22].Value = "MRB Taken time";
                        worksheet.Cells[1, 23].Value = "Rework time";
                        worksheet.Cells[1, 24].Value = "Sorting time";
                        worksheet.Cells[1, 25].Value = "Deviation waiting time";


                        int row = 2;
                        foreach (var item in data)
                        {
                            worksheet.Cells[row, 1].Value = item.ID;
                            worksheet.Cells[row, 2].Value = item.PartNum;
                            worksheet.Cells[row, 2].Value = item.EpiRev;
                            worksheet.Cells[row, 3].Value = item.JobNum;
                            worksheet.Cells[row, 4].Value = item.Inspection_Qty;
                            worksheet.Cells[row, 5].Value = item.Inspection_Type;
                            worksheet.Cells[row, 6].Value = item.QualityStage;
                            worksheet.Cells[row, 7].Value = item.Suppliername;
                            worksheet.Cells[row, 8].Value = item.Sample_Qty;
                            worksheet.Cells[row, 9].Value = item.Reject_Qty;
                            worksheet.Cells[row, 10].Value = item.rejectpersentage;
                            worksheet.Cells[row, 11].Value = item.Rework_Qty;
                            worksheet.Cells[row, 12].Value = item.Reworkpersentage;
                            worksheet.Cells[row, 13].Value = item.Deviation_Qty;
                            worksheet.Cells[row, 14].Value = item.Deviationpersentage;
                            worksheet.Cells[row, 15].Value = item.lotreject;
                            worksheet.Cells[row, 16].Value = item.TotalTimeinquality;
                            worksheet.Cells[row, 17].Value = item.InspectiontimeManpowerspendtime;
                            worksheet.Cells[row, 18].Value = item.TotalinspectiontimeManpowerspendtime;
                            worksheet.Cells[row, 19].Value = item.InspectiontimeQualitydivisionspendtimebypart;
                            worksheet.Cells[row, 20].Value = item.Totalinspectiontimequalitydivisionspendtimebypart;
                            worksheet.Cells[row, 21].Value = item.MRBTakentime;
                            worksheet.Cells[row, 22].Value = item.Reworktime;
                            worksheet.Cells[row, 23].Value = item.Sortingtime;
                            worksheet.Cells[row, 24].Value = item.Deviationwaitingtime;

                            if (item.Note == "Out source part")
                            {
                                worksheet.Cells[row, 1, row, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[row, 1, row, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                            }

                            row++;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    // for Total Sheet
                    try
                    {
                        worksheet1.Cells[1, 1].Value = "Sr No";
                        worksheet1.Cells[1, 2].Value = "Part No";
                        worksheet1.Cells[1, 3].Value = "Rev.No";
                        worksheet1.Cells[1, 4].Value = "Job NO";
                        worksheet1.Cells[1, 5].Value = "Inward Qty";
                        worksheet1.Cells[1, 6].Value = "Inspection type";
                        worksheet1.Cells[1, 7].Value = "Quality Stage";
                        worksheet1.Cells[1, 8].Value = "Inspection Qty";
                        worksheet1.Cells[1, 9].Value = "Reject Qty";
                        worksheet1.Cells[1, 10].Value = "Reject%";
                        worksheet1.Cells[1, 11].Value = "Rework Qty";
                        worksheet1.Cells[1, 12].Value = "Rework%";
                        worksheet1.Cells[1, 13].Value = "Deviation Qty";
                        worksheet1.Cells[1, 14].Value = "Deviation%";
                        worksheet1.Cells[1, 15].Value = "Lot Reject%";
                        worksheet1.Cells[1, 16].Value = "Total time in Quality%";
                        worksheet1.Cells[1, 17].Value = "Inspection time(Type) Manpower spend time";
                        worksheet1.Cells[1, 18].Value = "Total inspection time- Manpower spend time ";
                        worksheet1.Cells[1, 19].Value = "Inspection time(common) -Quality division spend time by part";
                        worksheet1.Cells[1, 20].Value = "Total inspection time(Common)-quality division spend time by part";
                        worksheet1.Cells[1, 21].Value = "MRB Taken time";
                        worksheet1.Cells[1, 22].Value = "Rework time";
                        worksheet1.Cells[1, 23].Value = "Sorting time";
                        worksheet1.Cells[1, 24].Value = "Deviation waiting time";

                        int row = 2;
                        foreach (var item in total)
                        {
                            worksheet1.Cells[row, 1].Value = item.ID;
                            worksheet1.Cells[row, 2].Value = item.PartNum;
                            worksheet1.Cells[row, 2].Value = item.EpiRev;
                            worksheet1.Cells[row, 3].Value = item.JobNum;
                            worksheet1.Cells[row, 4].Value = item.Inspection_Qty;
                            worksheet1.Cells[row, 5].Value = item.Inspection_Type;
                            worksheet1.Cells[row, 6].Value = item.QualityStage;
                            worksheet1.Cells[row, 7].Value = item.Suppliername;
                            worksheet1.Cells[row, 8].Value = item.Sample_Qty;
                            worksheet1.Cells[row, 9].Value = item.Reject_Qty;
                            worksheet1.Cells[row, 10].Value = item.rejectpersentage;
                            worksheet1.Cells[row, 11].Value = item.Rework_Qty;
                            worksheet1.Cells[row, 12].Value = item.Reworkpersentage;
                            worksheet1.Cells[row, 13].Value = item.Deviation_Qty;
                            worksheet1.Cells[row, 14].Value = item.Deviationpersentage;
                            worksheet1.Cells[row, 15].Value = item.lotreject;
                            worksheet1.Cells[row, 16].Value = item.TotalTimeinquality;
                            worksheet1.Cells[row, 17].Value = item.InspectiontimeManpowerspendtime;
                            worksheet1.Cells[row, 18].Value = item.TotalinspectiontimeManpowerspendtime;
                            worksheet1.Cells[row, 19].Value = item.InspectiontimeQualitydivisionspendtimebypart;
                            worksheet1.Cells[row, 20].Value = item.Totalinspectiontimequalitydivisionspendtimebypart;
                            worksheet1.Cells[row, 21].Value = item.MRBTakentime;
                            worksheet1.Cells[row, 22].Value = item.Reworktime;
                            worksheet1.Cells[row, 23].Value = item.Sortingtime;
                            worksheet1.Cells[row, 24].Value = item.Deviationwaitingtime;
                            if (item.Note == "Out source part")
                            {
                                worksheet.Cells[row, 1, row, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[row, 1, row, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                            }
                            row++;
                        }
                       

                        int colLabel = 28;
                        int colValue = 29;
                        int summaryRow = 2;

                        int inwardLot = incominginspection.Select(x => new { x.JobNum, x.QualityStage }).Distinct().Count();

                        int inwardQty = incominginspection
                            .GroupBy(x => new { x.JobNum, x.QualityStage })
                            .Select(g => g.First())
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int inspectionQty = incominginspection
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int rejectQty = incominginspection
                            .Sum(x => int.TryParse(x.Reject_Qty.ToString(), out int val) ? val : 0);

                        int reworkQty = incominginspection
                            .Sum(x => int.TryParse(x.Rework_Qty.ToString(), out int val) ? val : 0);

                        int deviationQty = incominginspection
                            .Sum(x => int.TryParse(x.Deviation_Qty.ToString(), out int val) ? val : 0);

                        int rejectPercentage = inwardQty > 0
                            ? (int)Math.Round((double)rejectQty / inwardQty * 100)
                            : 0;

                        int reworkPercentage = inwardQty > 0
                            ? (int)Math.Round((double)reworkQty / inwardQty * 100)
                            : 0;

                        int deviationPercentage = inwardQty > 0
                            ? (int)Math.Round((double)deviationQty / inwardQty * 100)
                            : 0;

                        int rejectedLots = incominginspection
                            .Where(x => int.TryParse(x.Reject_Qty.ToString(), out int r) && r > 0)
                            .Select(x => new { x.JobNum, x.QualityStage })
                            .Distinct()
                            .Count();

                        int lotReject = inwardLot > 0
                            ? (int)Math.Round((double)rejectedLots / inwardLot * 100)
                            : 0;

                        worksheet1.Cells[summaryRow++, colLabel].Value = "Filter date";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Inward Lot";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Inward Qty";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Inspection Qty";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Reject Qty";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Reject%";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Rework Qty";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Rework%";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Deviation Qty";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Deviation%";
                        worksheet1.Cells[summaryRow++, colLabel].Value = "Lot Reject%";

                        summaryRow = 2;
                        worksheet1.Cells[summaryRow++, colValue].Value = dateRange;
                        worksheet1.Cells[summaryRow++, colValue].Value = inwardLot;
                        worksheet1.Cells[summaryRow++, colValue].Value = inwardQty;
                        worksheet1.Cells[summaryRow++, colValue].Value = inspectionQty;
                        worksheet1.Cells[summaryRow++, colValue].Value = rejectQty;
                        worksheet1.Cells[summaryRow++, colValue].Value = $"{rejectPercentage}%";
                        worksheet1.Cells[summaryRow++, colValue].Value = reworkQty;
                        worksheet1.Cells[summaryRow++, colValue].Value = $"{reworkPercentage}%";
                        worksheet1.Cells[summaryRow++, colValue].Value = deviationQty;
                        worksheet1.Cells[summaryRow++, colValue].Value = $"{deviationPercentage}%";
                        worksheet1.Cells[summaryRow++, colValue].Value = $"{lotReject}%";

                        worksheet1.Cells[worksheet1.Dimension.Address].AutoFitColumns();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    // for Final Sheet
                    try
                    {
                        worksheet2.Cells[1, 1].Value = "Sr No";
                        worksheet2.Cells[1, 2].Value = "Part No";
                        worksheet2.Cells[1, 3].Value = "Rev.No";
                        worksheet2.Cells[1, 4].Value = "Job NO";
                        worksheet2.Cells[1, 5].Value = "Inward Qty";
                        worksheet2.Cells[1, 6].Value = "Inspection type";
                        worksheet2.Cells[1, 7].Value = "Quality Stage";
                        worksheet2.Cells[1, 8].Value = "Supplier";
                        worksheet2.Cells[1, 9].Value = "Inspection Qty";
                        worksheet2.Cells[1, 10].Value = "Reject Qty";
                        worksheet2.Cells[1, 11].Value = "Reject%";
                        worksheet2.Cells[1, 12].Value = "Rework Qty";
                        worksheet2.Cells[1, 13].Value = "Rework%";
                        worksheet2.Cells[1, 14].Value = "Deviation Qty";
                        worksheet2.Cells[1, 15].Value = "Deviation%";
                        worksheet2.Cells[1, 16].Value = "Lot Reject%";
                        worksheet2.Cells[1, 17].Value = "Total time in Quality%";
                        worksheet2.Cells[1, 18].Value = "Inspection time(Type) Manpower spend time";
                        worksheet2.Cells[1, 19].Value = "Total inspection time- Manpower spend time ";
                        worksheet2.Cells[1, 20].Value = "Inspection time(common) -Quality division spend time by part";
                        worksheet2.Cells[1, 21].Value = "Total inspection time(Common)-quality division spend time by part";
                        worksheet2.Cells[1, 22].Value = "MRB Taken time";
                        worksheet2.Cells[1, 23].Value = "Rework time";
                        worksheet2.Cells[1, 24].Value = "Sorting time";
                        worksheet2.Cells[1, 25].Value = "Deviation waiting time";

                        int row = 2;
                        foreach (var item in final)
                        {
                            worksheet2.Cells[row, 1].Value = item.ID;
                            worksheet2.Cells[row, 2].Value = item.PartNum;
                            worksheet2.Cells[row, 2].Value = item.EpiRev;
                            worksheet2.Cells[row, 3].Value = item.JobNum;
                            worksheet2.Cells[row, 4].Value = item.Inspection_Qty;
                            worksheet2.Cells[row, 5].Value = item.Inspection_Type;
                            worksheet2.Cells[row, 6].Value = item.QualityStage;
                            worksheet2.Cells[row, 7].Value = item.Suppliername;
                            worksheet2.Cells[row, 8].Value = item.Sample_Qty;
                            worksheet2.Cells[row, 9].Value = item.Reject_Qty;
                            worksheet2.Cells[row, 10].Value = item.rejectpersentage;
                            worksheet2.Cells[row, 11].Value = item.Rework_Qty;
                            worksheet2.Cells[row, 12].Value = item.Reworkpersentage;
                            worksheet2.Cells[row, 13].Value = item.Deviation_Qty;
                            worksheet2.Cells[row, 14].Value = item.Deviationpersentage;
                            worksheet2.Cells[row, 15].Value = item.lotreject;
                            worksheet2.Cells[row, 16].Value = item.TotalTimeinquality;
                            worksheet2.Cells[row, 17].Value = item.InspectiontimeManpowerspendtime;
                            worksheet2.Cells[row, 18].Value = item.TotalinspectiontimeManpowerspendtime;
                            worksheet2.Cells[row, 19].Value = item.InspectiontimeQualitydivisionspendtimebypart;
                            worksheet2.Cells[row, 20].Value = item.Totalinspectiontimequalitydivisionspendtimebypart;
                            worksheet2.Cells[row, 21].Value = item.MRBTakentime;
                            worksheet2.Cells[row, 23].Value = item.Sortingtime;
                            worksheet2.Cells[row, 24].Value = item.Deviationwaitingtime;

                            if (item.Note == "Out source part")
                            {
                                worksheet.Cells[row, 1, row, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[row, 1, row, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                            }

                            row++;
                           
                        }
                        int colLabel = 28;
                        int colValue = 29;
                        int summaryRow = 2;

                        int inwardLot = incominginspection.Select(x => new { x.JobNum, x.QualityStage }).Distinct().Count();

                        int inwardQty = incominginspection
                            .GroupBy(x => new { x.JobNum, x.QualityStage })
                            .Select(g => g.First())
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int inspectionQty = incominginspection
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int rejectQty = incominginspection
                            .Sum(x => int.TryParse(x.Reject_Qty.ToString(), out int val) ? val : 0);

                        int reworkQty = incominginspection
                            .Sum(x => int.TryParse(x.Rework_Qty.ToString(), out int val) ? val : 0);

                        int deviationQty = incominginspection
                            .Sum(x => int.TryParse(x.Deviation_Qty.ToString(), out int val) ? val : 0);

                        int rejectPercentage = inwardQty > 0
                            ? (int)Math.Round((double)rejectQty / inwardQty * 100)
                            : 0;

                        int reworkPercentage = inwardQty > 0
                            ? (int)Math.Round((double)reworkQty / inwardQty * 100)
                            : 0;

                        int deviationPercentage = inwardQty > 0
                            ? (int)Math.Round((double)deviationQty / inwardQty * 100)
                            : 0;

                        int rejectedLots = incominginspection
                            .Where(x => int.TryParse(x.Reject_Qty.ToString(), out int r) && r > 0)
                            .Select(x => new { x.JobNum, x.QualityStage })
                            .Distinct()
                            .Count();

                        int lotReject = inwardLot > 0
                            ? (int)Math.Round((double)rejectedLots / inwardLot * 100)
                            : 0;

                        worksheet2.Cells[summaryRow++, colLabel].Value = "Filter date";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Inward Lot";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Inward Qty";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Inspection Qty";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Reject Qty";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Reject%";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Rework Qty";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Rework%";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Deviation Qty";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Deviation%";
                        worksheet2.Cells[summaryRow++, colLabel].Value = "Lot Reject%";

                        summaryRow = 2;
                        worksheet2.Cells[summaryRow++, colValue].Value = "01-Jan-2024 to 31-Jan-2024";
                        worksheet2.Cells[summaryRow++, colValue].Value = inwardLot;
                        worksheet2.Cells[summaryRow++, colValue].Value = inwardQty;
                        worksheet2.Cells[summaryRow++, colValue].Value = inspectionQty;
                        worksheet2.Cells[summaryRow++, colValue].Value = rejectQty;
                        worksheet2.Cells[summaryRow++, colValue].Value = $"{rejectPercentage}%";
                        worksheet2.Cells[summaryRow++, colValue].Value = reworkQty;
                        worksheet2.Cells[summaryRow++, colValue].Value = $"{reworkPercentage}%";
                        worksheet2.Cells[summaryRow++, colValue].Value = deviationQty;
                        worksheet2.Cells[summaryRow++, colValue].Value = $"{deviationPercentage}%";
                        worksheet2.Cells[summaryRow++, colValue].Value = $"{lotReject}%";

                        worksheet2.Cells[worksheet2.Dimension.Address].AutoFitColumns();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    // for Visual Sheet
                    try
                    {
                        worksheet3.Cells[1, 1].Value = "Sr No";
                        worksheet3.Cells[1, 2].Value = "Part No";
                        worksheet3.Cells[1, 3].Value = "Rev.No";
                        worksheet3.Cells[1, 4].Value = "Job NO";
                        worksheet3.Cells[1, 5].Value = "Inward Qty";
                        worksheet3.Cells[1, 6].Value = "Inspection type";
                        worksheet3.Cells[1, 7].Value = "Quality Stage";
                        worksheet3.Cells[1, 8].Value = "Supplier";
                        worksheet3.Cells[1, 9].Value = "Inspection Qty";
                        worksheet3.Cells[1, 10].Value = "Reject Qty";
                        worksheet3.Cells[1, 11].Value = "Reject%";
                        worksheet3.Cells[1, 12].Value = "Rework Qty";
                        worksheet3.Cells[1, 13].Value = "Rework%";
                        worksheet3.Cells[1, 14].Value = "Deviation Qty";
                        worksheet3.Cells[1, 15].Value = "Deviation%";
                        worksheet3.Cells[1, 16].Value = "Lot Reject%";
                        worksheet3.Cells[1, 17].Value = "Total time in Quality%";
                        worksheet3.Cells[1, 18].Value = "Inspection time(Type) Manpower spend time";
                        worksheet3.Cells[1, 19].Value = "Total inspection time- Manpower spend time ";
                        worksheet3.Cells[1, 20].Value = "Inspection time(common) -Quality division spend time by part";
                        worksheet3.Cells[1, 21].Value = "Total inspection time(Common)-quality division spend time by part";
                        worksheet3.Cells[1, 22].Value = "MRB Taken time";
                        worksheet3.Cells[1, 23].Value = "Rework time";
                        worksheet3.Cells[1, 24].Value = "Sorting time";
                        worksheet3.Cells[1, 25].Value = "Deviation waiting time";


                        int row = 2;
                        foreach (var item in visual)
                        {
                            worksheet3.Cells[row, 1].Value = item.ID;
                            worksheet3.Cells[row, 2].Value = item.PartNum;
                            worksheet3.Cells[row, 2].Value = item.EpiRev;
                            worksheet3.Cells[row, 3].Value = item.JobNum;
                            worksheet3.Cells[row, 4].Value = item.Inspection_Qty;
                            worksheet3.Cells[row, 5].Value = item.Inspection_Type;
                            worksheet3.Cells[row, 6].Value = item.QualityStage;
                            worksheet3.Cells[row, 7].Value = item.Suppliername;
                            worksheet3.Cells[row, 8].Value = item.Sample_Qty;
                            worksheet3.Cells[row, 9].Value = item.Reject_Qty;
                            worksheet3.Cells[row, 10].Value = item.rejectpersentage;
                            worksheet3.Cells[row, 11].Value = item.Rework_Qty;
                            worksheet3.Cells[row, 12].Value = item.Reworkpersentage;
                            worksheet3.Cells[row, 13].Value = item.Deviation_Qty;
                            worksheet3.Cells[row, 14].Value = item.Deviationpersentage;
                            worksheet3.Cells[row, 15].Value = item.lotreject;
                            worksheet3.Cells[row, 16].Value = item.TotalTimeinquality;
                            worksheet3.Cells[row, 17].Value = item.InspectiontimeManpowerspendtime;
                            worksheet3.Cells[row, 18].Value = item.TotalinspectiontimeManpowerspendtime;
                            worksheet3.Cells[row, 19].Value = item.InspectiontimeQualitydivisionspendtimebypart;
                            worksheet3.Cells[row, 20].Value = item.Totalinspectiontimequalitydivisionspendtimebypart;
                            worksheet3.Cells[row, 21].Value = item.MRBTakentime;
                            worksheet3.Cells[row, 22].Value = item.Reworktime;
                            worksheet3.Cells[row, 23].Value = item.Sortingtime;
                            worksheet3.Cells[row, 24].Value = item.Deviationwaitingtime;

                            if (item.Note == "Out source part")
                            {
                                worksheet.Cells[row, 1, row, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[row, 1, row, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                            }

                            row++;
                            
                        }
                        int colLabel = 28;
                        int colValue = 29;
                        int summaryRow = 2;

                        int inwardLot = incominginspection.Select(x => new { x.JobNum, x.QualityStage }).Distinct().Count();

                        int inwardQty = incominginspection
                            .GroupBy(x => new { x.JobNum, x.QualityStage })
                            .Select(g => g.First())
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int inspectionQty = incominginspection
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int rejectQty = incominginspection
                            .Sum(x => int.TryParse(x.Reject_Qty.ToString(), out int val) ? val : 0);

                        int reworkQty = incominginspection
                            .Sum(x => int.TryParse(x.Rework_Qty.ToString(), out int val) ? val : 0);

                        int deviationQty = incominginspection
                            .Sum(x => int.TryParse(x.Deviation_Qty.ToString(), out int val) ? val : 0);

                        int rejectPercentage = inwardQty > 0
                            ? (int)Math.Round((double)rejectQty / inwardQty * 100)
                            : 0;

                        int reworkPercentage = inwardQty > 0
                            ? (int)Math.Round((double)reworkQty / inwardQty * 100)
                            : 0;

                        int deviationPercentage = inwardQty > 0
                            ? (int)Math.Round((double)deviationQty / inwardQty * 100)
                            : 0;

                        int rejectedLots = incominginspection
                            .Where(x => int.TryParse(x.Reject_Qty.ToString(), out int r) && r > 0)
                            .Select(x => new { x.JobNum, x.QualityStage })
                            .Distinct()
                            .Count();

                        int lotReject = inwardLot > 0
                            ? (int)Math.Round((double)rejectedLots / inwardLot * 100)
                            : 0;

                        worksheet3.Cells[summaryRow++, colLabel].Value = "Filter date";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Inward Lot";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Inward Qty";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Inspection Qty";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Reject Qty";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Reject%";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Rework Qty";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Rework%";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Deviation Qty";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Deviation%";
                        worksheet3.Cells[summaryRow++, colLabel].Value = "Lot Reject%";

                        summaryRow = 2;
                        worksheet3.Cells[summaryRow++, colValue].Value = dateRange;
                        worksheet3.Cells[summaryRow++, colValue].Value = inwardLot;
                        worksheet3.Cells[summaryRow++, colValue].Value = inwardQty;
                        worksheet3.Cells[summaryRow++, colValue].Value = inspectionQty;
                        worksheet3.Cells[summaryRow++, colValue].Value = rejectQty;
                        worksheet3.Cells[summaryRow++, colValue].Value = $"{rejectPercentage}%";
                        worksheet3.Cells[summaryRow++, colValue].Value = reworkQty;
                        worksheet3.Cells[summaryRow++, colValue].Value = $"{reworkPercentage}%";
                        worksheet3.Cells[summaryRow++, colValue].Value = deviationQty;
                        worksheet3.Cells[summaryRow++, colValue].Value = $"{deviationPercentage}%";
                        worksheet3.Cells[summaryRow++, colValue].Value = $"{lotReject}%";

                        worksheet3.Cells[worksheet3.Dimension.Address].AutoFitColumns();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    // for THared Sheet
                    try
                    {
                        worksheet4.Cells[1, 1].Value = "Sr No";
                        worksheet4.Cells[1, 2].Value = "Part No";
                        worksheet4.Cells[1, 3].Value = "Rev.No";
                        worksheet4.Cells[1, 4].Value = "Job NO";
                        worksheet4.Cells[1, 5].Value = "Inward Qty";
                        worksheet4.Cells[1, 6].Value = "Inspection type";
                        worksheet4.Cells[1, 7].Value = "Quality Stage";
                        worksheet4.Cells[1, 8].Value = "Supplier";
                        worksheet4.Cells[1, 9].Value = "Inspection Qty";
                        worksheet4.Cells[1, 10].Value = "Reject Qty";
                        worksheet4.Cells[1, 11].Value = "Reject%";
                        worksheet4.Cells[1, 12].Value = "Rework Qty";
                        worksheet4.Cells[1, 13].Value = "Rework%";
                        worksheet4.Cells[1, 14].Value = "Deviation Qty";
                        worksheet4.Cells[1, 15].Value = "Deviation%";
                        worksheet4.Cells[1, 16].Value = "Lot Reject%";
                        worksheet4.Cells[1, 17].Value = "Total time in Quality%";
                        worksheet4.Cells[1, 18].Value = "Inspection time(Type) Manpower spend time";
                        worksheet4.Cells[1, 19].Value = "Total inspection time- Manpower spend time ";
                        worksheet4.Cells[1, 20].Value = "Inspection time(common) -Quality division spend time by part";
                        worksheet4.Cells[1, 21].Value = "Total inspection time(Common)-quality division spend time by part";
                        worksheet4.Cells[1, 22].Value = "MRB Taken time";
                        worksheet4.Cells[1, 23].Value = "Rework time";
                        worksheet4.Cells[1, 24].Value = "Sorting time";
                        worksheet4.Cells[1, 25].Value = "Deviation waiting time";

                        int row = 2;
                        foreach (var item in Thared)
                        {
                            worksheet4.Cells[row, 1].Value = item.ID;
                            worksheet4.Cells[row, 2].Value = item.PartNum;
                            worksheet4.Cells[row, 2].Value = item.EpiRev;
                            worksheet4.Cells[row, 3].Value = item.JobNum;
                            worksheet4.Cells[row, 4].Value = item.Inspection_Qty;
                            worksheet4.Cells[row, 5].Value = item.Inspection_Type;
                            worksheet4.Cells[row, 6].Value = item.QualityStage;
                            worksheet4.Cells[row, 7].Value = item.Suppliername;
                            worksheet4.Cells[row, 8].Value = item.Sample_Qty;
                            worksheet4.Cells[row, 9].Value = item.Reject_Qty;
                            worksheet4.Cells[row, 10].Value = item.rejectpersentage;
                            worksheet4.Cells[row, 11].Value = item.Rework_Qty;
                            worksheet4.Cells[row, 12].Value = item.Reworkpersentage;
                            worksheet4.Cells[row, 13].Value = item.Deviation_Qty;
                            worksheet4.Cells[row, 14].Value = item.Deviationpersentage;
                            worksheet4.Cells[row, 15].Value = item.lotreject;
                            worksheet4.Cells[row, 16].Value = item.TotalTimeinquality;
                            worksheet4.Cells[row, 17].Value = item.InspectiontimeManpowerspendtime;
                            worksheet4.Cells[row, 18].Value = item.TotalinspectiontimeManpowerspendtime;
                            worksheet4.Cells[row, 19].Value = item.InspectiontimeQualitydivisionspendtimebypart;
                            worksheet4.Cells[row, 20].Value = item.Totalinspectiontimequalitydivisionspendtimebypart;
                            worksheet4.Cells[row, 21].Value = item.MRBTakentime;
                            worksheet4.Cells[row, 22].Value = item.Reworktime;
                            worksheet4.Cells[row, 23].Value = item.Sortingtime;
                            worksheet4.Cells[row, 24].Value = item.Deviationwaitingtime;

                            if (item.Note == "Out source part")
                            {
                                worksheet.Cells[row, 1, row, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[row, 1, row, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                            }

                            row++;
                           
                        }
                        int colLabel = 28;
                        int colValue = 29;
                        int summaryRow = 2;

                        int inwardLot = incominginspection.Select(x => new { x.JobNum, x.QualityStage }).Distinct().Count();

                        int inwardQty = incominginspection
                            .GroupBy(x => new { x.JobNum, x.QualityStage })
                            .Select(g => g.First())
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int inspectionQty = incominginspection
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int rejectQty = incominginspection
                            .Sum(x => int.TryParse(x.Reject_Qty.ToString(), out int val) ? val : 0);

                        int reworkQty = incominginspection
                            .Sum(x => int.TryParse(x.Rework_Qty.ToString(), out int val) ? val : 0);

                        int deviationQty = incominginspection
                            .Sum(x => int.TryParse(x.Deviation_Qty.ToString(), out int val) ? val : 0);

                        int rejectPercentage = inwardQty > 0
                            ? (int)Math.Round((double)rejectQty / inwardQty * 100)
                            : 0;

                        int reworkPercentage = inwardQty > 0
                            ? (int)Math.Round((double)reworkQty / inwardQty * 100)
                            : 0;

                        int deviationPercentage = inwardQty > 0
                            ? (int)Math.Round((double)deviationQty / inwardQty * 100)
                            : 0;

                        int rejectedLots = incominginspection
                            .Where(x => int.TryParse(x.Reject_Qty.ToString(), out int r) && r > 0)
                            .Select(x => new { x.JobNum, x.QualityStage })
                            .Distinct()
                            .Count();

                        int lotReject = inwardLot > 0
                            ? (int)Math.Round((double)rejectedLots / inwardLot * 100)
                            : 0;

                        worksheet4.Cells[summaryRow++, colLabel].Value = "Filter date";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Inward Lot";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Inward Qty";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Inspection Qty";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Reject Qty";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Reject%";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Rework Qty";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Rework%";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Deviation Qty";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Deviation%";
                        worksheet4.Cells[summaryRow++, colLabel].Value = "Lot Reject%";

                        summaryRow = 2;
                        worksheet4.Cells[summaryRow++, colValue].Value = dateRange;
                        worksheet4.Cells[summaryRow++, colValue].Value = inwardLot;
                        worksheet4.Cells[summaryRow++, colValue].Value = inwardQty;
                        worksheet4.Cells[summaryRow++, colValue].Value = inspectionQty;
                        worksheet4.Cells[summaryRow++, colValue].Value = rejectQty;
                        worksheet4.Cells[summaryRow++, colValue].Value = $"{rejectPercentage}%";
                        worksheet4.Cells[summaryRow++, colValue].Value = reworkQty;
                        worksheet4.Cells[summaryRow++, colValue].Value = $"{reworkPercentage}%";
                        worksheet4.Cells[summaryRow++, colValue].Value = deviationQty;
                        worksheet4.Cells[summaryRow++, colValue].Value = $"{deviationPercentage}%";
                        worksheet4.Cells[summaryRow++, colValue].Value = $"{lotReject}%";

                        worksheet4.Cells[worksheet4.Dimension.Address].AutoFitColumns();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    // for Humidity Sheet
                    try
                    {
                        worksheet5.Cells[1, 1].Value = "Sr No";
                        worksheet5.Cells[1, 2].Value = "Part No";
                        worksheet5.Cells[1, 3].Value = "Rev.No";
                        worksheet5.Cells[1, 4].Value = "Job NO";
                        worksheet5.Cells[1, 5].Value = "Inward Qty";
                        worksheet5.Cells[1, 6].Value = "Inspection type";
                        worksheet5.Cells[1, 7].Value = "Quality Stage";
                        worksheet5.Cells[1, 8].Value = "Supplier";
                        worksheet5.Cells[1, 9].Value = "Inspection Qty";
                        worksheet5.Cells[1, 10].Value = "Reject Qty";
                        worksheet5.Cells[1, 11].Value = "Reject%";
                        worksheet5.Cells[1, 12].Value = "Rework Qty";
                        worksheet5.Cells[1, 13].Value = "Rework%";
                        worksheet5.Cells[1, 14].Value = "Deviation Qty";
                        worksheet5.Cells[1, 15].Value = "Deviation%";
                        worksheet5.Cells[1, 16].Value = "Lot Reject%";
                        worksheet5.Cells[1, 17].Value = "Total time in Quality%";
                        worksheet5.Cells[1, 18].Value = "Inspection time(Type) Manpower spend time";
                        worksheet5.Cells[1, 19].Value = "Total inspection time- Manpower spend time ";
                        worksheet5.Cells[1, 20].Value = "Inspection time(common) -Quality division spend time by part";
                        worksheet5.Cells[1, 21].Value = "Total inspection time(Common)-quality division spend time by part";
                        worksheet5.Cells[1, 22].Value = "MRB Taken time";
                        worksheet5.Cells[1, 23].Value = "Rework time";
                        worksheet5.Cells[1, 24].Value = "Sorting time";
                        worksheet5.Cells[1, 25].Value = "Deviation waiting time";

                        int row = 2;
                        foreach (var item in Humidity)
                        {
                            worksheet5.Cells[row, 1].Value = item.ID;
                            worksheet5.Cells[row, 2].Value = item.PartNum;
                            worksheet5.Cells[row, 2].Value = item.EpiRev;
                            worksheet5.Cells[row, 3].Value = item.JobNum;
                            worksheet5.Cells[row, 4].Value = item.Inspection_Qty;
                            worksheet5.Cells[row, 5].Value = item.Inspection_Type;
                            worksheet5.Cells[row, 6].Value = item.QualityStage;
                            worksheet5.Cells[row, 7].Value = item.Suppliername;
                            worksheet5.Cells[row, 8].Value = item.Sample_Qty;
                            worksheet5.Cells[row, 9].Value = item.Reject_Qty;
                            worksheet5.Cells[row, 10].Value = item.rejectpersentage;
                            worksheet5.Cells[row, 11].Value = item.Rework_Qty;
                            worksheet5.Cells[row, 12].Value = item.Reworkpersentage;
                            worksheet5.Cells[row, 13].Value = item.Deviation_Qty;
                            worksheet5.Cells[row, 14].Value = item.Deviationpersentage;
                            worksheet5.Cells[row, 15].Value = item.lotreject;
                            worksheet5.Cells[row, 16].Value = item.TotalTimeinquality;
                            worksheet5.Cells[row, 17].Value = item.InspectiontimeManpowerspendtime;
                            worksheet5.Cells[row, 18].Value = item.TotalinspectiontimeManpowerspendtime;
                            worksheet5.Cells[row, 19].Value = item.InspectiontimeQualitydivisionspendtimebypart;
                            worksheet5.Cells[row, 20].Value = item.Totalinspectiontimequalitydivisionspendtimebypart;
                            worksheet5.Cells[row, 21].Value = item.MRBTakentime;
                            worksheet5.Cells[row, 22].Value = item.Reworktime;
                            worksheet5.Cells[row, 23].Value = item.Sortingtime;
                            worksheet5.Cells[row, 24].Value = item.Deviationwaitingtime;

                            if (item.Note == "Out source part")
                            {
                                worksheet.Cells[row, 1, row, 25].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[row, 1, row, 25].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                            }

                            row++;
                            
                        }
                        int colLabel = 28;
                        int colValue = 29;
                        int summaryRow = 2;

                        int inwardLot = incominginspection.Select(x => new { x.JobNum, x.QualityStage }).Distinct().Count();

                        int inwardQty = incominginspection
                            .GroupBy(x => new { x.JobNum, x.QualityStage })
                            .Select(g => g.First())
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int inspectionQty = incominginspection
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int rejectQty = incominginspection
                            .Sum(x => int.TryParse(x.Reject_Qty.ToString(), out int val) ? val : 0);

                        int reworkQty = incominginspection
                            .Sum(x => int.TryParse(x.Rework_Qty.ToString(), out int val) ? val : 0);

                        int deviationQty = incominginspection
                            .Sum(x => int.TryParse(x.Deviation_Qty.ToString(), out int val) ? val : 0);

                        int rejectPercentage = inwardQty > 0
                            ? (int)Math.Round((double)rejectQty / inwardQty * 100)
                            : 0;

                        int reworkPercentage = inwardQty > 0
                            ? (int)Math.Round((double)reworkQty / inwardQty * 100)
                            : 0;

                        int deviationPercentage = inwardQty > 0
                            ? (int)Math.Round((double)deviationQty / inwardQty * 100)
                            : 0;

                        int rejectedLots = incominginspection
                            .Where(x => int.TryParse(x.Reject_Qty.ToString(), out int r) && r > 0)
                            .Select(x => new { x.JobNum, x.QualityStage })
                            .Distinct()
                            .Count();

                        int lotReject = inwardLot > 0
                            ? (int)Math.Round((double)rejectedLots / inwardLot * 100)
                            : 0;

                        worksheet5.Cells[summaryRow++, colLabel].Value = "Filter date";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Inward Lot";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Inward Qty";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Inspection Qty";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Reject Qty";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Reject%";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Rework Qty";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Rework%";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Deviation Qty";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Deviation%";
                        worksheet5.Cells[summaryRow++, colLabel].Value = "Lot Reject%";

                        summaryRow = 2;
                        worksheet5.Cells[summaryRow++, colValue].Value = dateRange;
                        worksheet5.Cells[summaryRow++, colValue].Value = inwardLot;
                        worksheet5.Cells[summaryRow++, colValue].Value = inwardQty;
                        worksheet5.Cells[summaryRow++, colValue].Value = inspectionQty;
                        worksheet5.Cells[summaryRow++, colValue].Value = rejectQty;
                        worksheet5.Cells[summaryRow++, colValue].Value = $"{rejectPercentage}%";
                        worksheet5.Cells[summaryRow++, colValue].Value = reworkQty;
                        worksheet5.Cells[summaryRow++, colValue].Value = $"{reworkPercentage}%";
                        worksheet5.Cells[summaryRow++, colValue].Value = deviationQty;
                        worksheet5.Cells[summaryRow++, colValue].Value = $"{deviationPercentage}%";
                        worksheet5.Cells[summaryRow++, colValue].Value = $"{lotReject}%";

                        worksheet5.Cells[worksheet5.Dimension.Address].AutoFitColumns();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    // for Incoming Inspection Sheet
                    try
                    {
                        worksheet6.Cells[1, 1].Value = "Sr No";
                        worksheet6.Cells[1, 2].Value = "Part No";
                        worksheet6.Cells[1, 3].Value = "Rev.No";
                        worksheet6.Cells[1, 4].Value = "Job NO";
                        worksheet6.Cells[1, 5].Value = "Inward Qty";
                        worksheet6.Cells[1, 6].Value = "Inspection type";
                        worksheet6.Cells[1, 7].Value = "Quality Stage";
                        worksheet6.Cells[1, 8].Value = "Supplier";
                        worksheet6.Cells[1, 9].Value = "Inspection Qty";
                        worksheet6.Cells[1, 10].Value = "Reject Qty";
                        worksheet6.Cells[1, 11].Value = "Reject%";
                        worksheet6.Cells[1, 12].Value = "Rework Qty";
                        worksheet6.Cells[1, 13].Value = "Rework%";
                        worksheet6.Cells[1, 14].Value = "Deviation Qty";
                        worksheet6.Cells[1, 15].Value = "Deviation%";
                        worksheet6.Cells[1, 16].Value = "Lot Reject%";
                        worksheet6.Cells[1, 17].Value = "Total time in Quality%";
                        worksheet6.Cells[1, 18].Value = "Inspection time(Type) Manpower spend time";
                        worksheet6.Cells[1, 19].Value = "Total inspection time- Manpower spend time ";
                        worksheet6.Cells[1, 20].Value = "Inspection time(common) -Quality division spend time by part";
                        worksheet6.Cells[1, 21].Value = "Total inspection time(Common)-quality division spend time by part";
                        worksheet6.Cells[1, 22].Value = "MRB Taken time";
                        worksheet6.Cells[1, 23].Value = "Rework time";
                        worksheet6.Cells[1, 24].Value = "Sorting time";
                        worksheet6.Cells[1, 25].Value = "Deviation waiting time";

                        int row = 2;
                        foreach (var item in incominginspection)
                        {
                            worksheet6.Cells[row, 1].Value = item.ID;
                            worksheet6.Cells[row, 2].Value = item.PartNum;
                            worksheet6.Cells[row, 3].Value = item.EpiRev;
                            worksheet6.Cells[row, 4].Value = item.JobNum;
                            worksheet6.Cells[row, 5].Value = item.Inspection_Qty;
                            worksheet6.Cells[row, 6].Value = item.Inspection_Type;
                            worksheet6.Cells[row, 7].Value = item.QualityStage;
                            worksheet6.Cells[row, 8].Value = item.Suppliername;
                            worksheet6.Cells[row, 9].Value = item.Sample_Qty;
                            worksheet6.Cells[row, 10].Value = item.Reject_Qty;
                            worksheet6.Cells[row, 11].Value = item.rejectpersentage;
                            worksheet6.Cells[row, 12].Value = item.Rework_Qty;
                            worksheet6.Cells[row, 13].Value = item.Reworkpersentage;
                            worksheet6.Cells[row, 14].Value = item.Deviation_Qty;
                            worksheet6.Cells[row, 15].Value = item.Deviationpersentage;
                            worksheet6.Cells[row, 16].Value = item.lotreject;
                            worksheet6.Cells[row, 17].Value = item.TotalTimeinquality;
                            worksheet6.Cells[row, 18].Value = item.InspectiontimeManpowerspendtime;
                            worksheet6.Cells[row, 19].Value = item.TotalinspectiontimeManpowerspendtime;
                            worksheet6.Cells[row, 20].Value = item.InspectiontimeQualitydivisionspendtimebypart;
                            worksheet6.Cells[row, 21].Value = item.Totalinspectiontimequalitydivisionspendtimebypart;
                            worksheet6.Cells[row, 22].Value = item.MRBTakentime;
                            worksheet6.Cells[row, 23].Value = item.Reworktime;
                            worksheet6.Cells[row, 24].Value = item.Sortingtime;
                            worksheet6.Cells[row, 25].Value = item.Deviationwaitingtime; 
                            row++;
                        }

                        int colLabel = 28; 
                        int colValue = 29; 
                        int summaryRow = 2;

                        int inwardLot = incominginspection.Select(x => new { x.JobNum, x.QualityStage }).Distinct().Count();

                        int inwardQty = incominginspection
                            .GroupBy(x => new { x.JobNum, x.QualityStage })
                            .Select(g => g.First())
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int inspectionQty = incominginspection
                            .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);

                        int rejectQty = incominginspection
                            .Sum(x => int.TryParse(x.Reject_Qty.ToString(), out int val) ? val : 0);

                        int reworkQty = incominginspection
                            .Sum(x => int.TryParse(x.Rework_Qty.ToString(), out int val) ? val : 0);

                        int deviationQty = incominginspection
                            .Sum(x => int.TryParse(x.Deviation_Qty.ToString(), out int val) ? val : 0);

                        int rejectPercentage = inwardQty > 0
                            ? (int)Math.Round((double)rejectQty / inwardQty * 100)
                            : 0;

                        int reworkPercentage = inwardQty > 0
                            ? (int)Math.Round((double)reworkQty / inwardQty * 100)
                            : 0;

                        int deviationPercentage = inwardQty > 0
                            ? (int)Math.Round((double)deviationQty / inwardQty * 100)
                            : 0;

                        int rejectedLots = incominginspection
                            .Where(x => int.TryParse(x.Reject_Qty.ToString(), out int r) && r > 0)
                            .Select(x => new { x.JobNum, x.QualityStage })
                            .Distinct()
                            .Count();

                        int lotReject = inwardLot > 0
                            ? (int)Math.Round((double)rejectedLots / inwardLot * 100)
                            : 0;

                        worksheet6.Cells[summaryRow++, colLabel].Value = "Filter date";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Inward Lot";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Inward Qty";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Inspection Qty";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Reject Qty";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Reject%";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Rework Qty";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Rework%";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Deviation Qty";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Deviation%";
                        worksheet6.Cells[summaryRow++, colLabel].Value = "Lot Reject%";

                        summaryRow = 2; 
                        worksheet6.Cells[summaryRow++, colValue].Value = "01-Jan-2024 to 31-Jan-2024";
                        worksheet6.Cells[summaryRow++, colValue].Value = inwardLot;
                        worksheet6.Cells[summaryRow++, colValue].Value = inwardQty;
                        worksheet6.Cells[summaryRow++, colValue].Value = inspectionQty;
                        worksheet6.Cells[summaryRow++, colValue].Value = rejectQty;
                        worksheet6.Cells[summaryRow++, colValue].Value = $"{rejectPercentage}%";
                        worksheet6.Cells[summaryRow++, colValue].Value = reworkQty;
                        worksheet6.Cells[summaryRow++, colValue].Value = $"{reworkPercentage}%";
                        worksheet6.Cells[summaryRow++, colValue].Value = deviationQty;
                        worksheet6.Cells[summaryRow++, colValue].Value = $"{deviationPercentage}%";
                        worksheet6.Cells[summaryRow++, colValue].Value = $"{lotReject}%";

                        int supplierRow = summaryRow + 2;
                        int supplierCol = 28;

                        worksheet6.Cells[supplierRow, supplierCol].Value = "Supplier Name";
                        worksheet6.Cells[supplierRow, supplierCol + 1].Value = "Inward Lot";
                        worksheet6.Cells[supplierRow, supplierCol + 2].Value = "Inward Qty";
                        worksheet6.Cells[supplierRow, supplierCol + 3].Value = "Reject Qty";
                        worksheet6.Cells[supplierRow, supplierCol + 4].Value = "Reject%";
                        worksheet6.Cells[supplierRow, supplierCol + 5].Value = "Rework Qty";
                        worksheet6.Cells[supplierRow, supplierCol + 6].Value = "Rework%";
                        worksheet6.Cells[supplierRow, supplierCol + 7].Value = "Deviation Qty";
                        worksheet6.Cells[supplierRow, supplierCol + 8].Value = "Deviation%";
                        worksheet6.Cells[supplierRow, supplierCol + 9].Value = "Lot Reject%";

                        supplierRow++;

                        var groupedSuppliers = incominginspection.GroupBy(x => x.Suppliername);

                        foreach (var group in groupedSuppliers)
                        {
                            string supplier = group.Key;
                            int lotCount = group.Select(x => new { x.JobNum, x.QualityStage }).Distinct().Count();
                            int totalInward = group
                                .GroupBy(x => new { x.JobNum, x.QualityStage })
                                .Select(g => g.First())
                                .Sum(x => int.TryParse(x.Inspection_Qty, out int val) ? val : 0);
                            int totalReject = group.Sum(x => int.TryParse(x.Reject_Qty.ToString(), out int val) ? val : 0);
                            int totalRework = group.Sum(x => int.TryParse(x.Rework_Qty.ToString(), out int val) ? val : 0);
                            int totalDeviation = group.Sum(x => int.TryParse(x.Deviation_Qty.ToString(), out int val) ? val : 0);
                            int rejectedLotsPerSupplier = group
                                .Where(x => int.TryParse(x.Reject_Qty.ToString(), out int r) && r > 0)
                                .Select(x => new { x.JobNum, x.QualityStage })
                                .Distinct()
                                .Count();

                            double rejectPer = totalInward > 0 ? (double)totalReject * 100 / totalInward : 0;
                            double reworkPer = totalInward > 0 ? (double)totalRework * 100 / totalInward : 0;
                            double devPer = totalInward > 0 ? (double)totalDeviation * 100 / totalInward : 0;
                            double lotRejPer = lotCount > 0 ? (double)rejectedLotsPerSupplier * 100 / lotCount : 0;

                            worksheet6.Cells[supplierRow, supplierCol].Value = supplier;
                            worksheet6.Cells[supplierRow, supplierCol + 1].Value = lotCount;
                            worksheet6.Cells[supplierRow, supplierCol + 2].Value = totalInward;
                            worksheet6.Cells[supplierRow, supplierCol + 3].Value = totalReject;
                            worksheet6.Cells[supplierRow, supplierCol + 4].Value = $"{rejectPer:0.00}%";
                            worksheet6.Cells[supplierRow, supplierCol + 5].Value = totalRework;
                            worksheet6.Cells[supplierRow, supplierCol + 6].Value = $"{reworkPer:0.00}%";
                            worksheet6.Cells[supplierRow, supplierCol + 7].Value = totalDeviation;
                            worksheet6.Cells[supplierRow, supplierCol + 8].Value = $"{devPer:0.00}%";
                            worksheet6.Cells[supplierRow, supplierCol + 9].Value = $"{lotRejPer:0.00}%";

                            supplierRow++;
                        }

                        worksheet6.Cells[worksheet6.Dimension.Address].AutoFitColumns();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error generating Excel report: " + ex.Message);
                    }
                    // for Sum of Rejection Code Sheet
                    try
                    {
                        worksheet7.Cells["A1:D1"].Merge = true;
                        worksheet7.Cells["A1"].Value = dateRange;
                        worksheet7.Cells["A1"].Style.Font.Bold = true;
                        worksheet7.Cells["A1"].Style.Font.Size = 14;
                        worksheet7.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet7.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet7.Row(1).Height = 25;
                        worksheet7.Cells["A1:D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet7.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                        worksheet7.Cells[2, 1].Value = "Defect code";
                        worksheet7.Cells[2, 2].Value = "Description";
                        worksheet7.Cells[2, 3].Value = "Rejection Qty.";
                        worksheet7.Cells[2, 4].Value = "Rework Qty.";
                        worksheet7.Cells["A2:D2"].Style.Font.Bold = true;
                        worksheet7.Cells["A2:D2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet7.Cells["A2:D2"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                        var headerRange = worksheet7.Cells["A2:D2"];
                        headerRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        headerRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        headerRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        string[] codes = {
                            "R1", "R2", "R3", "R4", "R5", "R6", "R7", "R8", "R9", "R10",
                            "R11", "R12", "R13", "R14", "R15", "R16", "R17", "R18", "R19",
                            "R20", "R21", "R22", "R23", "R24", "R25", "R26", "R27", "R28", "R29", "R30"
                        };

                        string[] descriptions = {
                            "Dimensional rejection in outer diameter",
                            "Dimensional rejection in inner diameter",
                            "Dimensional rejection in distance or center distance at outer side",
                            "Dimensional rejection in distance or center distance at inner side",
                            "Dimensional rejection in Angle",
                            "Dimension rejection in total length",
                            "Dimensional rejection in GD & T",
                            "Dimensional rejection in threading",
                            "Dimensional rejection in radius form",
                            "Dimensional rejection in Hex/Square/Flat/Slot sizes",
                            "Rejection due to Burr/surface finish/Step",
                            "Rejection due to Dent/Scratches/Steel mark at machining stage",
                            "Rejection due to Overbuff at machining stage",
                            "Dimensional rejection due to Special processes",
                            "Rejection due to Rusting",
                            "Rejection due to Dent/Scratches/Steel mark at special processes stage",
                            "Rejection due to Overbuff at special processes stage",
                            "Rejection due to special processes",
                            "Rejection due to functionality",
                            "Rejection due to welding defects",
                            "Rejection in setting",
                            "Rejection reused for setting",
                            "Rejection due to power-cut",
                            "Rejection due to cut pc/HT checking",
                            "Rejection due to wrong gauge/instrument",
                            "Rejection due to raw material",
                            "Rejection due to outside processes",
                            "Rejection due to miss reading of drawing or print drawing",
                            "Rejection due to counterfeit part",
                            "Rejection of development"
                        };

                        var defectSummary = DB.Final_Inspection_Mrb_Rcode
                            .Where(x => x.Active == true && x.Deleted == false)
                            .GroupBy(x => x.Rcode)
                            .ToDictionary(
                                g => g.Key,
                                g => new
                                {
                                    RejectionQty = g.Sum(x => x.Reject ?? 0),
                                    ReworkQty = g.Sum(x => x.Rework ?? 0)
                                });

                        for (int i = 0; i < codes.Length; i++)
                        {
                            int row = i + 3;
                            string code = codes[i];
                            worksheet7.Cells[row, 1].Value = code;
                            worksheet7.Cells[row, 2].Value = descriptions[i];

                            if (defectSummary.ContainsKey(code))
                            {
                                worksheet7.Cells[row, 3].Value = defectSummary[code].RejectionQty;
                                worksheet7.Cells[row, 4].Value = defectSummary[code].ReworkQty;
                            }

                            var fillColor = (i >= 13 && i <= 16) ? Color.MediumPurple : Color.LightGreen;
                            var cellRange = worksheet7.Cells[row, 1, row, 4];
                            cellRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cellRange.Style.Fill.BackgroundColor.SetColor(fillColor);

                            cellRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            cellRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            cellRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            cellRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        worksheet7.Cells[worksheet7.Dimension.Address].AutoFitColumns();

                        worksheet7.View.FreezePanes(3, 1);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    // Save to MemoryStream
                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = "Quality rating and performance.xlsx";
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return File(stream, contentType, fileName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
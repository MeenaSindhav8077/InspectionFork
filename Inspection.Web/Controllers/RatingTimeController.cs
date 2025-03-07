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

namespace Inspection.Web.Controllers
{
    public class RatingTimeController : Controller
    {
        // GET: RatingTime
        ITe_INDIAEntities1 DB = new ITe_INDIAEntities1();
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
                List<Final_Inspection_Data> final = data.Where(p =>  p.Inspection_Type == "Final").ToList();
                List<Final_Inspection_Data> visual = data.Where(p =>  p.Inspection_Type == "Visual").ToList();
                List<Final_Inspection_Data> Thared = data.Where(p =>  p.Inspection_Type == "Thread").ToList();
                List<Final_Inspection_Data> Humidity = data.Where(p =>  p.Inspection_Type == "Humidity").ToList();
                List<Final_Inspection_Data> incominginspection = data.Where(p => p.Note == "Out Source Part").ToList();


                data = data.Where(x=>x.Inward_Date >= startDate &&  x.CloseRequstDate <= endDate).ToList();

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
                        worksheet.Cells[1, 17].Value = "Total time in Quality";
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

                        //int row = 2;
                        //foreach (var item in data)
                        //{
                        //    worksheet.Cells[row, 1].Value = item.Name;
                        //    worksheet.Cells[row, 2].Value = item.Age;
                        //    worksheet.Cells[row, 3].Value = item.Country;
                        //    row++;
                        //}
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    // for Sum of Rejection Code Sheet
                    try
                    {
                        worksheet7.Cells[1, 1].Value = "Sr No";
                        worksheet7.Cells[1, 2].Value = "Defect code";
                        worksheet7.Cells[1, 3].Value = "Description";
                        worksheet7.Cells[1, 4].Value = "Rejection Qty.";
                        worksheet7.Cells[1, 5].Value = "Rework Qty.";

                        worksheet7.Cells[3, 1].Value = "R1";
                        worksheet7.Cells[4, 1].Value = "R2";
                        worksheet7.Cells[5, 1].Value = "R3";
                        worksheet7.Cells[6, 1].Value = "R4";
                        worksheet7.Cells[7, 1].Value = "R5";
                        worksheet7.Cells[8, 1].Value = "R6";
                        worksheet7.Cells[9, 1].Value = "R7";
                        worksheet7.Cells[10, 1].Value = "R8";
                        worksheet7.Cells[11, 1].Value = "R9 ";
                        worksheet7.Cells[12, 1].Value = "R10";
                        worksheet7.Cells[13, 1].Value = "R11";
                        worksheet7.Cells[14, 1].Value = "R12";
                        worksheet7.Cells[15, 1].Value = "R13";
                        worksheet7.Cells[16, 1].Value = "R14";
                        worksheet7.Cells[17, 1].Value = "R15";
                        worksheet7.Cells[18, 1].Value = "R16";
                        worksheet7.Cells[19, 1].Value = "R17";
                        worksheet7.Cells[20, 1].Value = "R18";
                        worksheet7.Cells[21, 1].Value = "R19";
                        worksheet7.Cells[22, 1].Value = "R20";
                        worksheet7.Cells[23, 1].Value = "R21";
                        worksheet7.Cells[24, 1].Value = "R22";
                        worksheet7.Cells[25, 1].Value = "R23";
                        worksheet7.Cells[26, 1].Value = "R24";
                        worksheet7.Cells[27, 1].Value = "R25";
                        worksheet7.Cells[28, 1].Value = "R26";
                        worksheet7.Cells[29, 1].Value = "R27";
                        worksheet7.Cells[30, 1].Value = "R28";
                        worksheet7.Cells[31, 1].Value = "R29";
                        worksheet7.Cells[32, 1].Value = "R30";


                        worksheet7.Cells[3, 2].Value = "Dimensional rejection in outer diameter";
                        worksheet7.Cells[4, 2].Value = "Dimensional rejection in inner diameter";
                        worksheet7.Cells[5, 2].Value = "Dimensional rejection in distance or center distance at outer side";
                        worksheet7.Cells[6, 2].Value = "Dimensional rejection in distance or center distance at inner side";
                        worksheet7.Cells[7, 2].Value = "Dimensional rejection in Angle";
                        worksheet7.Cells[8, 2].Value = "Dimension rejection in total length";
                        worksheet7.Cells[9, 2].Value = "Dimensional rejection in GD & T";
                        worksheet7.Cells[10, 2].Value = "Dimensional rejection in threading";
                        worksheet7.Cells[11, 2].Value = "Dimensional rejection in radius form ";
                        worksheet7.Cells[12, 2].Value = "Dimensional rejection in Hex/Square/Flat/Slot sizes";
                        worksheet7.Cells[13, 2].Value = "Rejection due to Burr/surface finish/Step";
                        worksheet7.Cells[14, 2].Value = "Rejection due to Dent/Scratches/Steel mark at machining stage";
                        worksheet7.Cells[15, 2].Value = "Rejection due to Overbuff at machining stage";
                        worksheet7.Cells[16, 2].Value = "Dimensional rejection due to Special processes";
                        worksheet7.Cells[17, 2].Value = "Rejection due to Rusting";
                        worksheet7.Cells[18, 2].Value = "Rejection due to Dent/Scratches/Steel mark at special processes stage";
                        worksheet7.Cells[19, 2].Value = "Rejection due to Overbuff at special processes stage";
                        worksheet7.Cells[20, 2].Value = "Rejection due to special processes";
                        worksheet7.Cells[21, 2].Value = "Rejection due to functionality";
                        worksheet7.Cells[22, 2].Value = "Rejection due to welding defects";
                        worksheet7.Cells[23, 2].Value = "Rejection in setting";
                        worksheet7.Cells[24, 2].Value = "Rejection reused for setting";
                        worksheet7.Cells[25, 2].Value = "Rejection due to power-cut";
                        worksheet7.Cells[26, 2].Value = "Rejection due to cut pc/HT checking";
                        worksheet7.Cells[27, 2].Value = "Rejection due to wrong gauge/instrument";
                        worksheet7.Cells[28, 2].Value = "Rejection due to raw material";
                        worksheet7.Cells[29, 2].Value = "Rejection due to outside processes";
                        worksheet7.Cells[30, 2].Value = "Rejection due to miss reading of drawing or print drawing";
                        worksheet7.Cells[31, 2].Value = "Rejection due to counterfeit part";
                        worksheet7.Cells[32, 2].Value = "Rejection of development";

                        //int row = 2;
                        //foreach (var item in data)
                        //{
                        //    worksheet.Cells[row, 1].Value = item.Name;
                        //    worksheet.Cells[row, 2].Value = item.Age;
                        //    worksheet.Cells[row, 3].Value = item.Country;
                        //    row++;
                        //}
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
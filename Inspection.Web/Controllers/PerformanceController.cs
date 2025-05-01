using Inspection.Web.DataBase;
using Inspection.Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Inspection.Web.Controllers
{
    public class PerformanceController : Controller
    {
        ITEIndiaEntities DB = new ITEIndiaEntities();
        [Authorize]
        public ActionResult Index(PerformanceModel _model)
        {
            PerformanceModel model = new PerformanceModel();
            try
            {
                if (!string.IsNullOrEmpty(_model.DateRange))
                {
                    if (_model.inspector == null)
                    {
                        _model.inspector = "All";
                    }
                    var dates = _model.DateRange.Split(new[] { "to" }, StringSplitOptions.None);
                    if (dates.Length == 2)
                    {
                        string startDateString = dates[0].Trim();
                        string endDateString = dates[1].Trim();
                        IFormatProvider culture = new CultureInfo("en-US", true);
                        DateTime _sdate = DateTime.ParseExact(startDateString, "dd MMM, yyyy", culture);
                        DateTime _edate = DateTime.ParseExact(endDateString, "dd MMM, yyyy", culture);

                        if (_model.inspector == "All")
                        {
                            List<Final_Inspection_Process> _data = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= _sdate && x.Inspection_date <= _edate && x.Inspection_Type == "Humidity" && (x.Active == null || x.Active == true)).ToList();
                            if (_data != null)
                            {
                                double totalMinutes = _data.Sum(p =>
                                {
                                    if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                        return 0;

                                    try
                                    {
                                        DateTime? start = p.Inspection_date;
                                        DateTime? end = p.endtime;

                                        TimeSpan diff = end.Value - start.Value;
                                        return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                                    }
                                    catch
                                    {
                                        return 0; // Handle parsing errors
                                    }
                                });
                                model.Humidityqty = _data.Sum(p => p.Inspection_Qty);
                                model.HumidityHours = Math.Ceiling(totalMinutes / 60.1);
                            }
                            List<Final_Inspection_Process> _Fdata = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= _sdate && x.Inspection_date <= _edate && x.Inspection_Type == "Final" && (x.Active == null || x.Active == true)).ToList();
                            
                            if (_Fdata != null)
                            {
                                double totalMinutes = _Fdata.Sum(p =>
                                {
                                    if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                        return 0;

                                    try
                                    {
                                        DateTime? start = p.Inspection_date;
                                        DateTime? end = p.endtime;

                                        TimeSpan diff = end.Value - start.Value;
                                        return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                                    }
                                    catch
                                    {
                                        return 0; // Handle parsing errors
                                    }
                                });

                                model.Finalqty = _Fdata.Sum(p => p.Inspection_Qty);
                                model.FinalHours = Math.Ceiling(totalMinutes / 60.0); // Convert minutes to hours and round up
                            }
                            List<Final_Inspection_Process> _Vdata = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= _sdate && x.Inspection_date <= _edate && x.Inspection_Type == "Visual" && (x.Active == null || x.Active == true)).ToList();
                            if (_Vdata != null)
                            {
                                double totalMinutes = _Vdata.Sum(p =>
                                {
                                    if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                        return 0;

                                    try
                                    {
                                        DateTime? start = p.Inspection_date;
                                        DateTime? end = p.endtime;

                                        TimeSpan diff = end.Value - start.Value;
                                        return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                                    }
                                    catch
                                    {
                                        return 0; // Handle parsing errors
                                    }
                                });

                                model.Visualqty = _Vdata.Sum(p => p.Inspection_Qty);
                                model.VisualHours = Math.Ceiling(totalMinutes / 60.1);
                            }
                            List<Final_Inspection_Process> _Tdata = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= _sdate && x.Inspection_date <= _edate && x.Inspection_Type == "Thread" && (x.Active == null || x.Active == true)).ToList();
                            if (_Tdata != null)
                            {
                                double totalMinutes = _Tdata.Sum(p =>
                                {
                                    if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                        return 0;

                                    try
                                    {
                                        DateTime? start = p.Inspection_date;
                                        DateTime? end = p.endtime;

                                        TimeSpan diff = end.Value - start.Value;
                                        return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                                    }
                                    catch
                                    {
                                        return 0; // Handle parsing errors
                                    }
                                });
                                model.Threadqty = _Tdata.Sum(p => p.Inspection_Qty);
                                model.ThreadHours = Math.Ceiling(totalMinutes / 60.1);
                            }
                        }
                        else
                        {
                            List<Final_Inspection_Process> _data = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= _sdate && x.Inspection_date <= _edate && x.Inspection_Type == "Humidity" && x.done_by == _model.inspector && (x.Active == null || x.Active == true)).ToList();
                            if (_data != null)
                            {
                                double totalMinutes = _data.Sum(p =>
                                {
                                    if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                        return 0;

                                    try
                                    {
                                        DateTime? start = p.Inspection_date;
                                        DateTime? end = p.endtime;

                                        TimeSpan diff = end.Value - start.Value;
                                        return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                                    }
                                    catch
                                    {
                                        return 0; // Handle parsing errors
                                    }
                                });
                                model.Humidityqty = _data.Sum(p => p.Inspection_Qty);
                                model.HumidityHours = Math.Ceiling(totalMinutes / 60.0);
                            }
                            List<Final_Inspection_Process> _Fdata = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= _sdate && x.Inspection_date <= _edate && x.Inspection_Type == "Final" && x.done_by == _model.inspector && (x.Active == null || x.Active == true)).ToList();
                            //if (_Fdata != null)
                            //{
                            //    double totalMinutes = _Fdata.Sum(p =>
                            //    {
                            //        if (p.endtime == null)
                            //            return 0;

                            //        TimeSpan start = TimeSpan.Parse(p.starttime);
                            //        TimeSpan end = TimeSpan.Parse(p.endtime);

                            //        TimeSpan diff = end >= start ? end - start : (end + TimeSpan.FromHours(24)) - start;

                            //        return diff.TotalMinutes;
                            //    });
                            //    model.Finalqty = _Fdata.Sum(p => p.Inspection_Qty);
                            //    model.FinalHours = Math.Ceiling(totalMinutes / 60.0);
                            //}
                            if (_Fdata != null)
                            {
                                double totalMinutes = _Fdata.Sum(p =>
                                {
                                    if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                        return 0;

                                    try
                                    {
                                        DateTime? start = p.Inspection_date;
                                        DateTime? end = p.endtime;

                                        TimeSpan diff = end.Value - start.Value;
                                        return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                                    }
                                    catch
                                    {
                                        return 0; // Handle parsing errors
                                    }
                                });

                                model.Finalqty = _Fdata.Sum(p => p.Inspection_Qty);
                                model.FinalHours = Math.Ceiling(totalMinutes / 60.0); // Convert minutes to hours and round up
                            }
                            List<Final_Inspection_Process> _Vdata = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= _sdate && x.Inspection_date <= _edate && x.Inspection_Type == "Visual" && x.done_by == _model.inspector && (x.Active == null || x.Active == true)).ToList();
                            if (_Vdata != null)
                            {
                                double totalMinutes = _Vdata.Sum(p =>
                                {
                                    if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                        return 0;

                                    try
                                    {
                                        DateTime? start = p.Inspection_date;
                                        DateTime? end = p.endtime;

                                        TimeSpan diff = end.Value - start.Value;
                                        return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                                    }
                                    catch
                                    {
                                        return 0; // Handle parsing errors
                                    }
                                });
                                model.Visualqty = _Vdata.Sum(p => p.Inspection_Qty);
                                model.VisualHours = Math.Ceiling(totalMinutes / 60.0);

                            }
                            List<Final_Inspection_Process> _Tdata = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= _sdate && x.Inspection_date <= _edate && x.Inspection_Type == "Thread" && x.done_by == _model.inspector && (x.Active == null || x.Active == true)).ToList();
                            if (_Tdata != null)
                            {
                                double totalMinutes = _Tdata.Sum(p =>
                                {
                                    if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                        return 0;

                                    try
                                    {
                                        DateTime? start = p.Inspection_date;
                                        DateTime? end = p.endtime;

                                        TimeSpan diff = end.Value - start.Value;
                                        return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                                    }
                                    catch
                                    {
                                        return 0; // Handle parsing errors
                                    }
                                });

                                model.Threadqty = _Tdata.Sum(p => p.Inspection_Qty);
                                model.ThreadHours = Math.Ceiling(totalMinutes / 60.0);
                            }
                        }
                    }
                }
                else
                {
                    DateTime sdate = DateTime.Parse("2024-01-01");
                    DateTime edate = DateTime.Parse("2024-01-31");
                    _model.DateRange = "01 Jan 2022 to 31 Jan 2022";
                    List<Final_Inspection_Process> _data = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= sdate && x.Inspection_date <= edate && x.Inspection_Type == "Humidity" && (x.Active == null || x.Active == true)).ToList();
                    if (_data != null)
                    {
                        double totalMinutes = _data.Sum(p =>
                        {
                            if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                return 0;

                            try
                            {
                                DateTime? start = p.Inspection_date;
                                DateTime? end = p.endtime;

                                TimeSpan diff = end.Value - start.Value;
                                return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                            }
                            catch
                            {
                                return 0; // Handle parsing errors
                            }
                        });
                        model.Humidityqty = _data.Sum(p => p.Inspection_Qty);
                        model.HumidityHours = Math.Ceiling(totalMinutes / 60.0); ;
                    }
                    List<Final_Inspection_Process> _Fdata = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= sdate && x.Inspection_date <= edate && x.Inspection_Type == "Final" && (x.Active == null || x.Active == true)).ToList();
                    if (_Fdata != null)
                    {
                        double totalMinutes = _Fdata.Sum(p =>
                        {
                            if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                return 0;

                            try
                            {
                                DateTime? start = p.Inspection_date;
                                DateTime? end = p.endtime;

                                TimeSpan diff = end.Value - start.Value;
                                return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                            }
                            catch
                            {
                                return 0; // Handle parsing errors
                            }
                        });
                        model.Finalqty = _Fdata.Sum(p => p.Inspection_Qty);
                        model.FinalHours = Math.Ceiling(totalMinutes / 60.0);
                    }
                    List<Final_Inspection_Process> _Vdata = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= sdate && x.Inspection_date <= edate && x.Inspection_Type == "Visual" && (x.Active == null || x.Active == true)).ToList();
                    if (_Vdata != null)
                    {
                        double totalMinutes = _Vdata.Sum(p =>
                        {
                            if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                return 0;

                            try
                            {
                                DateTime? start = p.Inspection_date;
                                DateTime? end = p.endtime;

                                TimeSpan diff = end.Value - start.Value;
                                return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                            }
                            catch
                            {
                                return 0; // Handle parsing errors
                            }
                        });
                        model.Visualqty = _Vdata.Sum(p => p.Inspection_Qty);
                        model.VisualHours = Math.Ceiling(totalMinutes / 60.0);
                    }
                    List<Final_Inspection_Process> _Tdata = DB.Final_Inspection_Process.Where(x => x.Inspection_date >= sdate && x.Inspection_date <= edate && x.Inspection_Type == "Thread" && (x.Active == null || x.Active == true)).ToList();
                    if (_Tdata != null)
                    {
                        double totalMinutes = _Tdata.Sum(p =>
                        {
                            if (string.IsNullOrEmpty(p.starttime) || p.endtime != null)
                                return 0;

                            try
                            {
                                DateTime? start = p.Inspection_date;
                                DateTime? end = p.endtime;

                                TimeSpan diff = end.Value - start.Value;
                                return diff.TotalMinutes > 0 ? diff.TotalMinutes : 0;
                            }
                            catch
                            {
                                return 0; // Handle parsing errors
                            }
                        });

                        model.Threadqty = _Tdata.Sum(p => p.Inspection_Qty);
                        model.HumidityHours = Math.Ceiling(totalMinutes / 60.0);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            model.Totalqty = model.Threadqty + model.Humidityqty + model.Finalqty + model.Visualqty;
            model.TotalHours = model.ThreadHours + model.HumidityHours + model.FinalHours + model.VisualHours;

            model.DateRange = _model.DateRange;
            return View(model);
        }

        public ActionResult ShowDetails(string type , string inspector)
        {

            List<showdetails> _model = new List<showdetails>();
            if (!string.IsNullOrEmpty(inspector))
            {
                _model = (from model in DB.Final_Inspection_Process.Where(p => p.Active == true && p.Deleted == false && p.Inspection_Type == type && p.done_by == inspector)
                          select new showdetails
                          {
                              id = model.ID,
                              jobno = model.JobNum,
                              partno = model.PartNum,
                              stage = model.Stage,
                              qualitystage = model.Qualitystage,
                              sampleqty = model.sampleqty,
                              endtime = model.endtime,
                              startime = model.starttime,
                              inspectionqty = model.Inspection_Qty,
                          }).ToList();
                
            }
            else
            {
                _model = (from model in DB.Final_Inspection_Process.Where(p => p.Active == true && p.Deleted == false && p.Inspection_Type == type)
                          select new showdetails
                          {
                              id = model.ID,
                              jobno = model.JobNum,
                              partno = model.PartNum,
                              stage = model.Stage,
                              qualitystage = model.Qualitystage,
                              sampleqty = model.sampleqty,
                              endtime = model.endtime,
                              startime = model.starttime,
                              inspectionqty = model.Inspection_Qty,
                              startdate = model.Inspection_date,
                          }).ToList();
            }
            
            return View("Showallperformancedetails", _model);
        }

        private double CalculateTotalHours(List<Final_Inspection_Process> data)
        {
            if (data == null) return 0;
            if (data == null) return 0;

            double totalMinutes = data.Sum(p =>
            {
                if (p.endtime == null)
                    return 0;

                DateTime? start = (p.Inspection_date);
                DateTime? end =(p.endtime);

                TimeSpan diff = end >= start ? end.Value - start.Value : (end.Value.AddDays(1) - start.Value);
                return diff.TotalMinutes;
            });

            return totalMinutes / 60; 
        }
    }
}
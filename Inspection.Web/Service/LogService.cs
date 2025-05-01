using Inspection.Web.DataBase;
using Inspection.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inspection.Web.Service
{

    public class LogService
    {
        ITEIndiaEntities DB = new ITEIndiaEntities();
        public void AddLog(Exception ex,string methodname, string page)
        {
            int LNO = GetLineNumber(ex);


                Final_Inspection_LogTable _Inspection_LogTable = new Final_Inspection_LogTable();

                    _Inspection_LogTable.Message = ex.ToString();
                     _Inspection_LogTable.LineNumber = LNO;
                    _Inspection_LogTable.CurrentDateTime = DateTime.Now;
                     _Inspection_LogTable.PageName = page;
                     _Inspection_LogTable.MethodName = methodname;
                    _Inspection_LogTable.Active = true;
                    _Inspection_LogTable.Deleted = false;


                DB.Final_Inspection_LogTable.Add(_Inspection_LogTable);
                DB.SaveChanges();
          
        }


        private int GetLineNumber(Exception exception)
        {
            if (exception.StackTrace != null)
            {
                var stackTrace = new System.Diagnostics.StackTrace(exception, true);
                foreach (var frame in stackTrace.GetFrames())
                {
                    int lineNumber = frame.GetFileLineNumber();
                    if (lineNumber != 0)
                    {
                        return lineNumber;
                    }
                }
            }
            return 0;
        }
    }
}
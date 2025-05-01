using Inspection.Web.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Service
{

    public class Maineservice
    {
        LogService logService = new LogService();
        public List<SelectListItem> GetInspectiontype()
        {
            ITEIndiaEntities DB = new ITEIndiaEntities();

            var dbData = DB.Final_Inspection_Stage_Master.GroupBy(tol => tol.stage_part_status.Trim()).Select(group => group.FirstOrDefault()).OrderBy(p=>p.Stage).ToList();

            List<SelectListItem> selectListItems = dbData.Select(tol => new SelectListItem
            {
                Value = tol.stage_part_status.ToString(),
                //Text = $"{tol.stage_part_status} - {tol.Stage}"
                Text = tol.stage_part_status.ToString(),
            }).ToList();

            return selectListItems;
        }
        public List<SelectListItem> Getuser()
        {
            ITEIndiaEntities DB = new ITEIndiaEntities();

            List<SelectListItem> selectListItems = DB.user_data.GroupBy(tol => tol.name.Trim()).Select(group => group.FirstOrDefault())
                .Select(tol => new SelectListItem
                {
                    Value = tol.name.ToString(),
                    Text = tol.name
                }).ToList();

            return selectListItems;
        }

        public void SendEmail(List<string> toEmail, int id, string jobno, string body)
        {
            try
            {
                if (toEmail != null && id != null && jobno != null)
                {

                    //if (id == 2)
                    //{
                    //    body = $"The drawing provided does not meet the required standards For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 4)
                    //{
                    //    body = $"The Job Traveller provided does not meet the required standards For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 5)
                    //{
                    //    body = $"The Job Pick List provided does not meet the required standards For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 6)
                    //{
                    //    body = $"The PO  does not meet the required standards For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 7)
                    //{
                    //    body = $"The MAFIA  does not meet the required standards For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 8)
                    //{
                    //    body = $"The PIR If Applicable  does not meet the required standards For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 10)
                    //{
                    //    body = $"The Purchase orders must have part number, revision number, SQRM code and unit price For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 11)
                    //{
                    //    body = $"The Check part number, job number, revision number match with purchase order in Drawing, Job traveller, Job pick list, MAFIA or PIR, HT Certificate does not meet the required standards For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 12)
                    //{
                    //    body = $"The Job traveller processes match with drawing requirement For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 13)
                    //{
                    //    body = $"The Quantity mismatch each operation For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 14)
                    //{
                    //    body = $"The Any box shoud not be blanked For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 15)
                    //{
                    //    body = $"The used raw material is match with the drawing requirement For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 16)
                    //{
                    //    body = $" The issue lot number against the Part Job number. For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 17)
                    //{
                    //    body = $"If SQRM Code is A22 then check that the used material is must be a homogeouse lot(One heat number). For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 18)
                    //{
                    //    body = $"Verify standard as per drawing requirement in material certificate. For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 19)
                    //{
                    //    body = $"All performed dimensions to be filled in MAFIA. For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 20)
                    //{
                    //    body = $" All performed dimensions to be filled in PIR. For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 21)
                    //{
                    //    body = $" The filled dimension as per the sample size. For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 22)
                    //{
                    //    body = $"The Pass, failed qty, date, and do sign off. For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    //else if (id == 23)
                    //{
                    //    body = $"The drawing provided does not meet the required standards For this Jobno {jobno}. Please review and submit a corrected version..";
                    //}
                    if (!string.IsNullOrEmpty(body))
                    {
                        foreach (var item in toEmail)
                        {
                            using (var client = new SmtpClient())
                            {
                                var emailMessage = new MailMessage();
                                emailMessage.From = new MailAddress("ssw-ai@sswhite.net");
                                emailMessage.To.Add("atul@sswhite.net");
                                emailMessage.Subject = "Document Verification Failed";
                                emailMessage.Body = body;

                                client.Host = "smtp.gmail.com";
                                client.Port = 587;
                                client.UseDefaultCredentials = true;
                                client.Credentials = new NetworkCredential("ssw-ai@sswhite.net", "ahbarp#6008");
                                client.EnableSsl = true;
                                client.Send(emailMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logService.AddLog(ex, "SendEmail", "mainservice");
            }
        }

        public void SendEmailMovePart(int qty, string jobno, string partno, string stage , string Type)
        {
            try
            {
                if (!string.IsNullOrEmpty(jobno))
                {
                    using (var client = new SmtpClient())
                    {
                        var emailMessage = new MailMessage();
                        emailMessage.From = new MailAddress("ssw-ai@sswhite.net");
                        emailMessage.To.Add("atul@sswhite.net");
                        emailMessage.Subject = "Part Move in rework Stage";
                        emailMessage.Body = $"Part Move in Rework Stage. Qty: {qty}, Jobno: {jobno}, Partnum: {partno}, Stage: {stage}, Type: {Type}";
                        client.Host = "smtp.gmail.com";
                        client.Port = 587;
                        client.UseDefaultCredentials = true;
                        client.Credentials = new NetworkCredential("ssw-ai@sswhite.net", "ahbarp#6008");
                        client.EnableSsl = true;
                        client.Send(emailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                logService.AddLog(ex, "SendEmailMovePart", "mainservice");
            }
        }
    }
}
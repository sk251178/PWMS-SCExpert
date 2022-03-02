using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace WMS.SchedulerTasks
{
    public class ExpiredLoadsReport
    {
        public ExpiredLoadsReport()
        {

        }

        public void Send()
        {
            try
            {
                // Create a MailMessage object
                MailMessage mm = new MailMessage();

                // Define the sender and recipient
                mm.From = new MailAddress("tevaexpiredloadreport@teva.co.il", "TEVAExpiredLoadReportSender");
                mm.To.Add(new MailAddress("smadarts@matrix.co.il", "Mu ha ha"));

                // Define the subject and body
                mm.Subject = "TEVAExpiredLoadReportSender";
                mm.Body = "TEVAExpiredLoadReportSender";
                mm.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.teva.co.il");
                client.Send(mm);

            }
            catch (Exception ex)
            {
                Made4Net.Shared.Logging.LogFile _qhlogger = new Made4Net.Shared.Logging.LogFile("F:\\SCExpert\\Logs\\Scheduler\\", "EL_" + DateTime.Now.ToString("_ddMMyyyy_hhmmss") + new Random().Next().ToString() + ".txt", false);
                _qhlogger.WriteLine("Ex : " + ex.Message, true);                

            }

        }
    }
}

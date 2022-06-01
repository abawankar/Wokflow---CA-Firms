using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace UserInterface.Controllers
{
    public static class EmailSetting
    {
        #region -- Mail setting --

        public static void SendEmail(MailMessage m)
        {
            m.From = new MailAddress("nsb@nimbusconsulting.in", "Nimbus Consulting");
            SendEmail(m, true);
        }

        public static void SendEmail(MailMessage m, Boolean Async)
        {
            SmtpClient smtpClient = null;
            smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("nsb@nimbusconsulting.in", "e83yJ<@5v");
            smtpClient.Port = 25;
            smtpClient.Host = "mail.nimbusconsulting.in";
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;
            if (Async)
            {
                SendEmailDelegate sd = new SendEmailDelegate(smtpClient.Send);
                AsyncCallback cb = new AsyncCallback(SendEmailResponse);
                sd.BeginInvoke(m, cb, sd);
            }
            else
            {
                smtpClient.Send(m);
            }
        }

        private delegate void SendEmailDelegate(MailMessage m);

        private static void SendEmailResponse(IAsyncResult ar)
        {
            SendEmailDelegate sd = (SendEmailDelegate)(ar.AsyncState);
            sd.EndInvoke(ar);
        }

        #endregion

        public static void SMSSendToLenders(long MobileNo, string Collection)
        {
            WebClient client = new WebClient();

            string user = "IKPL";
            string pwd = "1754444876 ";

            string msg = "" + Collection + ".";

            string baseurl = "http://bulksms.mysmsmantra.com:8080/WebSMS/SMSAPI.jsp?username=" + user + "&password=" + pwd + "&sendername=NETSMS&mobileno=" + MobileNo + "&message=" + msg + "";
            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            Console.WriteLine(s + " to " + MobileNo);
            data.Close();
            reader.Close();
        }
    }
}
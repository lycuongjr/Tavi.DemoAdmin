using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
namespace Tavi.Core.UI
{
  public static  class EmailHelper
    {
        public static bool SendEmail(string toAddress, string subject, string body, string displayName)//, string smtpServer, int smtpPort, string userName, string pass, bool enableSsl
        {
            if (String.IsNullOrEmpty(toAddress))
                return false;
            bool checkOk = true;
            string IsSendMail = System.Configuration.ConfigurationManager.AppSettings["IsSendMail"].ToString();
            if (Convert.ToBoolean(IsSendMail))
            {
                //Read from Web.config.xml
                string smtpServer = System.Configuration.ConfigurationManager.AppSettings["smtpServer"].ToString();
                int smtpPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["smtpPort"].ToString());
                string userName = System.Configuration.ConfigurationManager.AppSettings["email"].ToString();
                string email = System.Configuration.ConfigurationManager.AppSettings["email"].ToString();
                string pass = System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString();
                bool enableSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["enableSsl"].ToString());
                string fromAddress = email;
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(fromAddress, displayName);
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = body;

                //message.Attachments.Add(new Attachment());
                foreach (string s in toAddress.Split(';'))
                    message.To.Add(new MailAddress(s));

                message.BodyEncoding = Encoding.UTF8;


                SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(userName, pass);
                smtp.EnableSsl = enableSsl;
                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    checkOk = false;
                }
            }
            else
            {
                checkOk = false;
            }
            return checkOk;
        }

    }
}

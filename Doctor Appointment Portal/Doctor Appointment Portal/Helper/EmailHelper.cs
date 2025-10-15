using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Doctor_Appointment_Portal.Helper
{
    public class EmailHelper
    {
        public static void SendEmail(string toEmail, string subject,string body)
        {
            var mail = new MailMessage();
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress("asmataliravtar@gmail.com");

            using (var smtp = new SmtpClient())
            {
                smtp.Send(mail);
            }
        }
    }
}
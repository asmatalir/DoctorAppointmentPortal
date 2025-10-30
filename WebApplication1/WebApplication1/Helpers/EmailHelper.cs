using System.Net;
using System.Net.Mail;


public static class EmailHelper
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

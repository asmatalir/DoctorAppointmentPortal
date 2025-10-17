using System.Net;
using System.Net.Mail;


public static class EmailHelper
{
    public static void SendSecondaryPassword(string toEmail, string otp)
    {
        var mail = new MailMessage();
        mail.To.Add(toEmail);
        mail.Subject = "OTP for Purchase Order";
        mail.Body = $"Your one-time password is: {otp}";
        mail.From = new MailAddress("asmataliravtar@gmail.com");

        using (var smtp = new SmtpClient()) 
        {
            smtp.Send(mail);
        }
    }
}

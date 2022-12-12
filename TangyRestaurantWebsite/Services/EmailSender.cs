using System;
using System.Net;
using System.Net.Mail;

namespace TangyRestaurantWebsite.Services
{
	public class EmailSender
	{
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("ben.spark92@gmail.com", "Temp1234*"),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("ben.spark92@gmail.com")
            };
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            client.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}


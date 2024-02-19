using System.Net;
using System.Net.Mail;

namespace personal_project.Utils
{
    public class MailUtils
    {
        public static void SendGmail(string to)
        {
            MailMessage message = new MailMessage();

            message.From = new MailAddress("nguyenminhduc12cltt19@gmail.com");
            message.To.Add(new MailAddress(to));
            message.Subject = "Test gmail";
            message.IsBodyHtml = true;
            message.Body = "<h1>hahaha<h1>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(message.From.ToString(), "rweg yhvy iewl xits")
            };
            smtp.Send(message);
        }
    }
}

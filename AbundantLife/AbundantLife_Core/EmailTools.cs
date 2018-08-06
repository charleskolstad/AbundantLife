using AbundantLife_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AbundantLife_Core
{
    internal class EmailTools : IEmailTools
    {
        public bool SendEmail(string to, string body)
        {
            try
            {
                MailAddress mailFrom = new MailAddress("charlespkolstad@gmail.com", "202MobileService");
                MailAddress mailTo = new MailAddress(to, to);

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(mailFrom.Address, "hide");

                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = "Message from Abundant Life!";
                message.Body = body;
                message.IsBodyHtml = true;
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                DBCommands.RecordError(ex);
                return false;
            }
        }
    }

    internal class FakeEmailTools : IEmailTools
    {
        public bool SendEmail(string to, string body)
        {
            return true;
        }
    }

    internal interface IEmailTools
    {
        bool SendEmail(string to, string body);
    }
}

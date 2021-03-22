using Api.Services.IServices;
using Core.CustomEntities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly SmptSettings options;
        private readonly IWebHostEnvironment environment;

        public EmailSender(IOptions<SmptSettings> options, IWebHostEnvironment environment)
        {
            this.options = options.Value;
            this.environment = environment;
        }

        public async Task SenderEmail(string email, string subject, string body)
        {
            using (var smtp = new SmtpClient())
            {
                var Mail = new MailMessage(this.options.SenderEmail, email);

                smtp.Host = this.options.Server;
                smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;

                smtp.Port = this.options.Port;
                smtp.Credentials = new System.Net.NetworkCredential(this.options.UserName, this.options.Password);

                Mail.Subject = subject;
                Mail.IsBodyHtml = true;
                Mail.BodyEncoding = UTF8Encoding.UTF8;
                Mail.Body = $"<h1>{body}</h1>";
                Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                await smtp.SendMailAsync(Mail);
            }
        }
    }
}

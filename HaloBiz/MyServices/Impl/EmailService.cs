using System;
using System.Threading;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HaloBiz.DTOs;
using MimeKit.Text;
using Microsoft.Extensions.Options;

namespace HaloBiz.MyServices.Impl
{
    public class EmailService:IEmailService
    {
        private readonly MailSettings _appSettings;

        public EmailService(IOptions<MailSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public  void Send(string from, string to, string subject, string html)
        {
            // create message
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_appSettings.Mail,_appSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
         

        }
    }
}
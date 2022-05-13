using System;
using System.Collections.Generic;
using System.Threading;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloBiz.DTOs;
using HaloBiz.DTOs.TransferDTOs;
using Microsoft.AspNetCore.Http;
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

        public  void Send(MailRequest mailRequest)
        {
            // create message
            try
            {
                // var email = new MimeMessage();
                // email.From.Add(MailboxAddress.Parse(from),MailboxAddress.Parse(" "));
                
                var email = new MimeMessage();  
                email.From.Add(new MailboxAddress  
                (mailRequest.EmailSenderName + $"({mailRequest.EmailSender})",   
                    mailRequest.EmailSender 
                ));  

               
                foreach (var recipient in mailRequest.EmailReceivers)
                {
                    email.To.Add(MailboxAddress.Parse(recipient));
                }

                // foreach (var VARIABLE in COLLECTION)
                // {
                //     
                // }
                email.Subject = mailRequest.EmailSubject;
                email.Body = new TextPart(TextFormat.Html) { Text = mailRequest.EmailBody };
                var builder = new BodyBuilder();
                if (mailRequest.Attachments.Any())
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes,ContentType.Parse(file.ContentType));
                        }
                    }
                }

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect(_appSettings.Host, _appSettings.Port, SecureSocketOptions.SslOnConnect);
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
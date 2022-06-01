using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
      
namespace HaloBiz.DTOs.TransferDTOs
{
    public class MailRequest
    {
        public string EmailSender { get; set; }
        public List<string> EmailReceivers { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string EmailSenderName { get; set; }
        public long DestinationId { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
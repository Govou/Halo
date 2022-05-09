namespace HaloBiz.DTOs.TransferDTOs
{
    public class MailRequest
    {
        public string EmailSender { get; set; }
        public string EmailReceiver { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}
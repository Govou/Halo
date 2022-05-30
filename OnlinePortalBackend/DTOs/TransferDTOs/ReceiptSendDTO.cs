namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class ReceiptSendDTO
    {
        public string CustomerName { get; set; }
        public string ReceiptSummary { get; set; }
        public string ReceiptItems { get; set; }
        public string AmountInWords { get; set; }
        public string[] Email { get; set; }
        public string Subject { get; set; }
        public string ReceiptFooter { get; set; }
    }
}
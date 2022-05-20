namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class SMSInvoiceDTO
    {
        public string InvoiceNumber { get; set; }
        public double InvoiceValue { get; set; }
        public long InvoiceId { get; set; }
        public double Tax { get; set; }
    }
}

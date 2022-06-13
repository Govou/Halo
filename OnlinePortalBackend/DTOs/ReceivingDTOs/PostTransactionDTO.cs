namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class PostTransactionDTO
    {
        public int ProfileId { get; set; }
        public string PaymentGateway { get; set; }
        public string PaymentReferenceInternal { get; set; }
        public string PaymentReferenceGateway { get; set; }
        public long ContractId { get; set; }
        public string PaymentGatewayResponseCode { get; set; }
        public string PaymentGatewayResponseDescription { get; set; }
        public decimal Value { get; set; }
        public decimal VAT { get; set; }
        public string TransactionType { get; set; }
        public string TransactionSource { get; set; }
        public bool PaymentConfirmed { get; set; }
    }
}

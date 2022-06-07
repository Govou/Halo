namespace HaloBiz.DTOs.TransferDTOs
{
  
    public class AdvancePaymentTransferDTO
    {
        public long Id { get; set; }

        public long? CustomerDivisionId { get; set; }

        public CustomerDivisionWithoutObjectsTransferDTO CustomerDivision { get; set; }

        public double Amount { get; set; }

        public string EvidenceOfPaymentUrl { get; set; }
    }
}

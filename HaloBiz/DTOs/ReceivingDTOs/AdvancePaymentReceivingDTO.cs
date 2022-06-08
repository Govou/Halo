namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class AdvancePaymentReceivingDTO
    {

        public string Description { get; set; }

        public long CustomerDivisionId { get; set; }

        public double Amount { get; set; }

        public string EvidenceOfPaymentUrl { get; set; }

        public long AccountId { get; set; }

    }
}

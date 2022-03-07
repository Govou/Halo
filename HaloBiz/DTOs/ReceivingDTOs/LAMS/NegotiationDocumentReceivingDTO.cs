using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class NegotiationDocumentReceivingDTO : DocumentReceivingDTO
    {
        [Required]
        public long QuoteServiceId { get; set; }
    }
}
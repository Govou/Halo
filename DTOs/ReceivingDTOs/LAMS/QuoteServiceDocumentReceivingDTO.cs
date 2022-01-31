using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class QuoteServiceDocumentReceivingDTO : DocumentReceivingDTO
    {
        public long QuoteServiceId { get; set; }
        public string Type { get; set; }
        public bool IsGroupUpload { get; set; } = false;
        public long QuoteId { get; set; }
    }
}
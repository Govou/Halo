using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ClosureDocumentReceivingDTO : DocumentReceivingDTO
    {
        [Required]
        public long ContractServiceId { get; set; }
    }
}
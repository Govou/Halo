using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs.LAMS
{
    public class DropLeadReceivingDTO
    {
        [Required]
        public long DropReasonId { get; set; }
        [Required, StringLength(2500)]
        public string DropLearning { get; set; }
    }
}
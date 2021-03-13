using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class StandardSLAForOperatingEntitiesReceivingDTO : BaseSetupDTO
    {
        [Required, StringLength(2500)]
        public string DocumentUrl { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
    }
}
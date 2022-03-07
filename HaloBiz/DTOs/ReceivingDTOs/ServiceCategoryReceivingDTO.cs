using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ServiceCategoryReceivingDTO
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public long ServiceGroupId { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
        [Required]
        public long DivisionId { get; set; }
        public string ImageUrl { get; set; }
    }
}
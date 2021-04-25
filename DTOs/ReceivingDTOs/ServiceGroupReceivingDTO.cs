using System.ComponentModel.DataAnnotations;
using HalobizMigrations.Models;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ServiceGroupReceivingDTO
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
        [Required]
        public long DivisionId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ZoneReceivingDTO
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        public long HeadId { get; set; }
        public long StateId { get; set; }
        public long LGAId { get; set; }
        public long RegionId { get; set; }
    }
}
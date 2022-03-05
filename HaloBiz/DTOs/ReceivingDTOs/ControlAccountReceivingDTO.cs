using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ControlAccountReceivingDTO
    {
        [StringLength(100)]
        public string Caption { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public string Alias { get; set; }
        public long AccountClassId { get; set; }
    }
}
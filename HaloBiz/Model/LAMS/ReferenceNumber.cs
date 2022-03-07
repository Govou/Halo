using System.ComponentModel.DataAnnotations;

namespace HaloBiz.Model.LAMS
{
    public class ReferenceNumber
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long ReferenceNo { get; set; }
        
    }
}
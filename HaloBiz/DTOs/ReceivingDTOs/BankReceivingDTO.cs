using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class BankReceivingDTO
    {
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Slogan { get; set; }
        [StringLength(200)]
        public string Alias { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
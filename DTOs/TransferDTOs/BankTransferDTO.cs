using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class BankTransferDTO
    {
        [Key]
        public long Id { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Slogan { get; set; }
        [StringLength(200)]
        public string Alias { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
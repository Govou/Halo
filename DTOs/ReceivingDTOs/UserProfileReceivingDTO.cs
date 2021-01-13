using System;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTO
{
    public class UserProfileReceivingDTO
    {
    
        [Required, RegularExpression("[\\w\\s\\W]{2,20}", ErrorMessage="Firstname can only be alphabets between 3 to 20 characters long")]
        public string FirstName { get; set; }
        [Required, RegularExpression("[\\w\\s\\W]{2,20}", ErrorMessage="Lastname can only be alphabets between 3 to 20 characters long")]
        public string LastName { get; set; }
        [StringLength(20, ErrorMessage="Othername can only be alphabets between 3 to 20 characters long")]
        public string OtherName { get; set; }
        [StringLength(50, ErrorMessage="Codename can only be alphabets between 3 to 50 characters long")]
        public string CodeName { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required, RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage="Invalid Email Address")]
        public string Email { get; set; }
        [RegularExpression("\\d{10,15}", ErrorMessage="Mobile number can only be digits between 10 to 15 characters long")]
        public string MobileNumber { get; set; }
        [Required, MaxLength(255)]
        public string ImageUrl { get; set; }
        public string AltEmail { get; set; }
        [RegularExpression("\\d{10,15}", ErrorMessage="Mobile number can only be digits between 10 to 15 characters long")]
        public string AltMobileNumber { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [MaxLength(255)]
        public string LinkedInHandle { get; set; }
        [MaxLength(255)]
        public string FacebookHandle  { get; set; }
        [MaxLength(255)]
        public string TwitterHandle { get; set; }
        [MaxLength(255)]
        public string InstagramHandle { get; set; }
        public long StaffId { get; set; }
    }
}
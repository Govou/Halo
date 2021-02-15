using System;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs.LAMS
{
    public class ContactRecievingDTO
    {
        public string Title { get; set; }
        public string Gender { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, RegularExpression("\\d{10,15}")]
        public string MobileNumber { get; set; }
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage="Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string Designation { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
       
    }
}
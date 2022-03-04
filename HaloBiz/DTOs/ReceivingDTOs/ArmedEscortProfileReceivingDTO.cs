using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ArmedEscortProfileReceivingDTO
    {
       
        public string Gender { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public long ArmedEscortTypeId { get; set; }
      
        [Required]
        public long? RankId { get; set; }
        public string Alias { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class PilotProfileReceivingDTO
    {
        [Required]
        public long? MeansOfIdentificationId { get; set; }
        [Required]
        public string IDNumber { get; set; }
        public string Gender { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public long? PilotTypeId { get; set; }
        public string Address { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public long? RankId { get; set; }
    }
}

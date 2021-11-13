using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class PilotProfileTransferDTO
    {
        public long CreatedById { get; set; }
        public long? MeansOfIdentificationId { get; set; }
        public string IDNumber { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? PilotTypeId { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public DateTime DOB { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        
        public long Id { get; set; }
        public long RankId { get; set; }
        public bool IsDeleted { get; set; }
    }
}

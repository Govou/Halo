using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ArmedEscortProfileTransferDTO
    {
       

        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public long ArmedEscortTypeId { get; set; }
        public long? SupplierServiceId { get; set; }
        public long? ServiceAssignmentId { get; set; }
        public long? RankId { get; set; }
        public string Alias { get; set; }
        public string Mobile { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public long Id { get; set; }
      
    }
}

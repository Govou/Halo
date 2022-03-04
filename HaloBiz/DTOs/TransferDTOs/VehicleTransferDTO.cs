using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class VehicleTransferDTO
    {
        
        public long Id { get; set; }
        public long? SupplierServiceId { get; set; }
      
        public long? AttachedBranchId { get; set; }
       
        public long? AttachedOfficeId { get; set; }
      
        public long? VehicleTypeId { get; set; }
    
        public long CreatedById { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }


        public SupplierService SupplierService { get; set; }
        public Branch AttachedBranch { get; set; }
       
        public Office AttachedOffice { get; set; }
      
        public VehicleType VehicleType { get; set; }
      
        public UserProfile CreatedBy { get; set; }
    }

    public class VehicleTypeTransferDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public string TypeDesc { get; set; }
        public long? ServiceRegistrationId { get; set; }
        public long CreatedById { get; set; }
        //public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    

}

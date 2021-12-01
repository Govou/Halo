using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ResourceRequiredTransferDTO
    {
    }

    public class CommanderResourceRequiredTransferDTO
    {
      
        public long Id { get; set; }
        public long? ServiceRegistrationId { get; set; }
        public ServiceRegistration ServiceRegistration { get; set; }
        public long? CommanderTypeId { get; set; }
        public CommanderType CommanderType { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ArmedEscortResourceRequiredTransferDTO
    {

        
        public long Id { get; set; }
        public long? ServiceRegistrationId { get; set; }
        public ServiceRegistration ServiceRegistration { get; set; }
        public long? ArmedEscortTypeId { get; set; }
        public ArmedEscortType ArmedEscortType { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PilotResourceRequiredTransferDTO
    {
        public long Id { get; set; }
        public long? ServiceRegistrationId { get; set; }
        public ServiceRegistration ServiceRegistration { get; set; }
        public long? PilotTypeId { get; set; }
        public PilotType PilotType { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class VehicleResourceRequiredTransferDTO
    {
        
        public long Id { get; set; }
        public long? ServiceRegistrationId { get; set; }
        public ServiceRegistration ServiceRegistration { get; set; }
        public long? VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

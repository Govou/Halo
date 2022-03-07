using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class DTSDetailGenericDaysTransferDTO
    {
    }

    public class ArmedEscortDTSDetailGenericDaysTransferDTO
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool Sunday { get; set; }
        public bool Saturday { get; set; }
        public bool Friday { get; set; }
        public bool Thursday { get; set; }
        public bool Wednesday { get; set; }
        public bool Tuesday { get; set; }
        public bool Monday { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public ArmedEscortDTSMaster DTSMaster { get; set; }
        public long? DTSMasterId { get; set; }
       
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class CommanderDTSDetailGenericDaysTransferDTO
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool Sunday { get; set; }
        public bool Saturday { get; set; }
        public bool Friday { get; set; }
        public bool Thursday { get; set; }
        public bool Wednesday { get; set; }
        public bool Tuesday { get; set; }
        public bool Monday { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public CommanderDTSMaster DTSMaster { get; set; }
        public long? DTSMasterId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class PilotDTSDetailGenericDaysTransferDTO
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool Sunday { get; set; }
        public bool Saturday { get; set; }
        public bool Friday { get; set; }
        public bool Thursday { get; set; }
        public bool Wednesday { get; set; }
        public bool Tuesday { get; set; }
        public bool Monday { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public PilotDTSMaster DTSMaster { get; set; }
        public long? DTSMasterId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class VehicleDTSDetailGenericDaysTransferDTO
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool Sunday { get; set; }
        public bool Saturday { get; set; }
        public bool Friday { get; set; }
        public bool Thursday { get; set; }
        public bool Wednesday { get; set; }
        public bool Tuesday { get; set; }
        public bool Monday { get; set; }
        public DateTime ClosingTime { get; set; }
        public DateTime OpeningTime { get; set; }
        public VehicleDTSMaster DTSMaster { get; set; }
        public long? DTSMasterId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

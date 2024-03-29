﻿using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class DTSMastersTransferDTO
    {
    }
    public class ArmedEscortDTSMastersTransferDTO
    {
        
        public long Id { get; set; }
        public string Caption { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
        public long? ArmedEscortResourceId { get; set; }
        public ArmedEscortProfile ArmedEscortResource { get; set; }
        public List<ArmedEscortDTSDetailGenericDay> GenericDays { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
       
    }
    public class CommanderDTSMastersTransferDTO
    {
        
        public long Id { get; set; }
        public string Caption { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
        public List<CommanderDTSDetailGenericDay> GenericDays { get; set; }
        public long? CommanderResourceId { get; set; }
        public CommanderProfile CommanderResource { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class PilotDTSMastersTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
        public List<PilotDTSDetailGenericDay> GenericDays { get; set; }
        public long? PilotResourceId { get; set; }
        public PilotProfile PilotResource { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class VehicleDTSMastersTransferDTO
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailablilityEnd { get; set; }
        public List<VehicleDTSDetailGenericDay> GenericDays { get; set; }
        public long? VehicleResourceId { get; set; }
        public Vehicle VehicleResource { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

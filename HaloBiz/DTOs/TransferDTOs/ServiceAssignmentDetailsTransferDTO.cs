using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServiceAssignmentDetailsTransferDTO
    {
    }
    public class ArmedEscortServiceAssignmentDetailsTransferDTO
    {
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public ReleaseType ActionReleaseType { get; set; }
        public long? ActionReleaseTypeId { get; set; }
        public ReleaseType TempReleaseType { get; set; }
        public long? TempReleaseTypeId { get; set; }
        public DateTime? DateActionReleased { get; set; }
        public DateTime DateHeldForAction { get; set; }
        public bool IsHeldForAction { get; set; }
        public DateTime? DateTemporarilyReleased { get; set; }
        
        public DateTime DateTemporarilyHeld { get; set; }
        public bool IsTemporarilyHeld { get; set; }
        public int RequiredCount { get; set; }
        public ArmedEscortProfile ArmedEscortResource { get; set; }
        public long? ArmedEscortResourceId { get; set; }
        public MasterServiceAssignment ServiceAssignment { get; set; }
        public long? ServiceAssignmentId { get; set; }
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class CommanderServiceAssignmentDetailsTransferDTO
    {
        public long Id { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public ReleaseType ActionReleaseType { get; set; }
        public long? ActionReleaseTypeId { get; set; }
        public ReleaseType TempReleaseType { get; set; }
        public long? TempReleaseTypeId { get; set; }
        public DateTime? DateActionReleased { get; set; }
        public DateTime DateHeldForAction { get; set; }
        public bool IsHeldForAction { get; set; }
        public DateTime? DateTemporarilyReleased { get; set; }
        public DateTime DateTemporarilyHeld { get; set; }
        public bool IsTemporarilyHeld { get; set; }
        public int RequiredCount { get; set; }
        public Vehicle TiedVehicleResource { get; set; }
        public long? TiedVehicleResourceId { get; set; }
        public CommanderProfile CommanderResource { get; set; }
        public long? CommanderResourceId { get; set; }
        public MasterServiceAssignment ServiceAssignment { get; set; }
        public long? ServiceAssignmentId { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class PilotServiceAssignmentDetailsTransferDTO
    {
        public long Id { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public ReleaseType ActionReleaseType { get; set; }
        public long? ActionReleaseTypeId { get; set; }
        public ReleaseType TempReleaseType { get; set; }
        public long? TempReleaseTypeId { get; set; }
        public DateTime? DateActionReleased { get; set; }
        public DateTime DateHeldForAction { get; set; }
        public bool IsHeldForAction { get; set; }
        public DateTime? DateTemporarilyReleased { get; set; }
        public DateTime DateTemporarilyHeld { get; set; }
        public bool IsTemporarilyHeld { get; set; }
        public int RequiredCount { get; set; }
        public Vehicle TiedVehicleResource { get; set; }
        public long? TiedVehicleResourceId { get; set; }
        public PilotProfile PilotResource { get; set; }
        public long? PilotResourceId { get; set; }
        public MasterServiceAssignment ServiceAssignment { get; set; }
        public long? ServiceAssignmentId { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class VehicleServiceAssignmentDetailsTransferDTO
    {
        public long Id { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public ReleaseType ActionReleaseType { get; set; }
        public long? ActionReleaseTypeId { get; set; }
        public ReleaseType TempReleaseType { get; set; }
        public long? TempReleaseTypeId { get; set; }
        public DateTime? DateActionReleased { get; set; }
        public DateTime DateHeldForAction { get; set; }
        public bool IsHeldForAction { get; set; }
        public DateTime? DateTemporarilyReleased { get; set; }
        public DateTime DateTemporarilyHeld { get; set; }
        public bool IsTemporarilyHeld { get; set; }
        public int RequiredCount { get; set; }
        public Vehicle VehicleResource { get; set; }
        public long? VehicleResourceId { get; set; }
        public MasterServiceAssignment ServiceAssignment { get; set; }
        public long? ServiceAssignmentId { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PassengerTransferDTO
    {
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public PassengerType PassengerType { get; set; }
        public long? PassengerTypeId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedAt { get; set; }
        public string InstagramHandle { get; set; }
        public string TwitterHandle { get; set; }
        public string Othername { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
       
        public MasterServiceAssignment ServiceAssignment { get; set; }
        public long? ServiceAssignmentId { get; set; }
        public long Id { get; set; }
        public string FacebooHandle { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

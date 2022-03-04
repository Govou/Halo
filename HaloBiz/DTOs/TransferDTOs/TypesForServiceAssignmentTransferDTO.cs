using HalobizMigrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class TypesForServiceAssignmentTransferDTO
    {
    }
    public class PassengerTypesForServiceAssignmentTransferDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class TripTypesForServiceAssignmentTransferDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class SourceTypesForServiceAssignmentTransferDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ReleaseTypesForServiceAssignmentTransferDTO
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

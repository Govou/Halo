﻿using HalobizMigrations.Models;
using HalobizMigrations.Models.Armada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class MasterServiceAssignmentTransferDTO
    {
        public long Id { get; set; }
      
        public bool IsReturnJourney { get; set; }
        public long? PrimaryTripAssignmentId { get; set; }
        public TripType TripType { get; set; }
        public long? TripTypeId { get; set; }
        public SMORoute SMORoute { get; set; }
        public long? SMORouteId { get; set; }
        public SMORegion SMORegion { get; set; }
        public long? SMORegionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ReadinessTime { get; set; }
        public string DropoffLocation { get; set; }
        public string PickoffLocation { get; set; }
        public DateTime DropoffDate { get; set; }
        public DateTime PickupDate { get; set; }
        public ContractService ContractService { get; set; }
        public int SAExecutionStatus { get; set; }
        public long? ContractServiceId { get; set; }
        public double PickupLocationLongitude { get; set; }
        public double PickupLocationLatitude { get; set; }
        public double DropoffLocationLongitude { get; set; }
        public double DropoffLocationLatitude { get; set; }
        public double DistanceInKM { get; set; }
        public ServiceRegistration ServiceRegistration { get; set; }
        public long? ServiceRegistrationId { get; set; }
        public CustomerDivision CustomerDivision { get; set; }
        public long? CustomerDivisionId { get; set; }
        public SourceType SourceType { get; set; }
        public long? SourceTypeId { get; set; }
        public UserProfile CreatedBy { get; set; }
        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public string AssignmentStatus { get; set; }
        public bool ReadyStatus { get; set; }
        public DateTime PickoffTime { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SecondaryServiceAssignmentTransferDTO
    {
        public long Id { get; set; }
        public long? ServiceAssignmentId { get; set; }
        public MasterServiceAssignment ServiceAssignment { get; set; }
        public long? SecondaryServiceRegistrationId { get; set; }
        public ServiceRegistration SecondaryServiceRegistration { get; set; }
        public long? SecondaryContractServiceId { get; set; }
        public ContractService SecondaryContractService { get; set; }
        public bool IsDeleted { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
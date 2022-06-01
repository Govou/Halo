using System;
using System.Collections.Generic;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Shared;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServiceTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string ServiceCode { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double UnitPrice { get; set; }
        public long ServiceCategoryId { get; set; }
        public ServiceCategory ServiceCategory { get; set; }
        public long ServiceGroupId { get; set; }
        public ServiceGroup ServiceGroup { get; set; }
        public long OperatingEntityId { get; set; }
        public OperatingEntity OperatingEntity { get; set; }
        public long DivisionId { get; set; }
        public Division Division { get; set; }
        public long CreatedById { get; set; }
        public UserProfile CreatedBy { get; set; }
        public bool IsPublished { get; set; }
        public bool IsRequestedForPublish { get; set; }
        public bool PublishedApprovedStatus { get; set; }
        public TargetTransferDTO Target { get; set; }
        public ControlAccountTransferDTO ControlAccount { get; set; }
        public long ControlAccountId { get; set; }
        public ServiceTypeTransferDTO ServiceType { get; set; }
        public bool IsVatable { get; set; }
        public bool? CanBeSoldOnline { get; set; }
        public virtual ServiceRelationship DirectRelationship { get; set; }
        public virtual ServiceRelationship AdminRelationship { get; set; }

        public DateTime CreatedAt { get; set; }
        public IList<RequiredServiceDocumentTransferDTO> RequiredServiceDocument { get; set; }
        public IList<RequredServiceQualificationElementTransferDTO> RequiredServiceFields { get; set; }
        public ServiceRelationshipEnum ServiceRelationshipEnum { get; set; }

    }

    public class ServiceCategoryForRetailTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        //public ServiceCategory ServiceCategory { get; set; }
        public serviceCategoryTransferDTO ServiceCategory { get; set; }
    }

    public class serviceCategoryTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
}
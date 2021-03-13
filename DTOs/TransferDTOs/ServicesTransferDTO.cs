using System;
using System.Collections.Generic;
using HaloBiz.Model;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.ManyToManyRelationship;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServicesTransferDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
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
        public AccountTransferDTO Account { get; set; }
        public long AccountId { get; set; }
        public ServiceTypeTransferDTO ServiceType { get; set; }
        public DateTime CreatedAt { get; set; }
        public IList<RequiredServiceDocumentTransferDTO> RequiredServiceDocument { get; set; }
        public IList<RequredServiceQualificationElementTransferDTO> RequiredServiceFields { get; set; }
       
    }
}
using System;
using System.Collections.Generic;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Shared;

namespace HaloBiz.DTOs.TransferDTOs
{
    public class ServicesLeanformatDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ServiceCode { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double UnitPrice { get; set; }
        public long ServiceCategoryId { get; set; }
        public long ServiceGroupId { get; set; }
        public long OperatingEntityId { get; set; }
        public long DivisionId { get; set; }
        public bool IsPublished { get; set; }
        public bool IsRequestedForPublish { get; set; }
        public bool PublishedApprovedStatus { get; set; }
    
        public long ControlAccountId { get; set; }
        public bool IsVatable { get; set; }
        public bool? CanBeSoldOnline { get; set; }
        public ServiceRelationshipEnum ServiceRelationshipEnum { get; set; }
        public virtual ServiceRelationship DirectRelationship { get; set; }
        public virtual ServiceRelationship AdminRelationship { get; set; }


    }
}
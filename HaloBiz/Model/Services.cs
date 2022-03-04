using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.AccountsModel;
using HaloBiz.Model.LAMS;
using HaloBiz.Model.ManyToManyRelationship;

namespace HaloBiz.Model
{
    public class Services
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(50)]
        public string ServiceCode { get; set; }
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [MaxLength(5000)]
        public string ImageUrl { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        public bool IsPublished { get; set; }
        public bool IsRequestedForPublish { get; set; }
        public bool PublishedApprovedStatus { get; set; }
        public long? TargetId { get; set; }
        public virtual Target Target { get; set; }
        public long? ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }
        [Required]
        public long ServiceCategoryId { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
        [Required]
        public long ServiceGroupId { get; set; }
        public virtual ServiceGroup ServiceGroup { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
        public virtual OperatingEntity OperatingEntity { get; set; }
        [Required]
        public long DivisionId { get; set; }
        public virtual Division Division { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public long? AccountId { get; set; }
        public Account Account { get; set; }
        public IList<ServiceRequiredServiceDocument> RequiredServiceDocument { get; set; }
        public IList<ServiceRequredServiceQualificationElement> RequredServiceQualificationElement { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
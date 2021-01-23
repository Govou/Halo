using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [MaxLength(500)]
        public string ImageUrl { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        public bool IsPublished { get; set; }
        public bool IsRequestedForPublish { get; set; }
        public bool PublishedApprovedStatus { get; set; }
        public long? TargetId { get; set; }
        public virtual Target Target { get; set; }
        [Required]
        public long ServiceCategoryId { get; set; }
        [Required]
        public long ServiceGroupId { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
        [Required]
        public long DivisionId { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
        public IList<ServiceRequiredServiceDocument> RequiredServiceDocument { get; set; }
        public IEnumerable<QuoteService> QuoteServices { get; set; }
        public IEnumerable<ContractService> ContractServices { get; set; }
        public bool IsDeleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ServicesReceivingDTO
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [MaxLength(500)]
        public string ImageUrl { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public bool IsRequestedForPublish { get; set; }
        [Required]
        public long TargetId { get; set; }
        [Required]
        public long ServiceCategoryId { get; set; }
        [Required]
        public long ServiceTypeId { get; set; }
        [Required]
        public long ServiceGroupId { get; set; }
        [Required]
        public long AccountId { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
        [Required]
        public long DivisionId { get; set; }
        [Required]
        public IList<Int64> RequiredDocumentsId { get; set; }
        public IList<Int64> RequiredServiceFieldsId { get; set; }
    }
}
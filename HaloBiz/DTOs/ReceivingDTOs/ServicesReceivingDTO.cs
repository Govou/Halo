using HalobizMigrations.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ServiceReceivingDTO
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(3), MaxLength(255)]
        public string Description { get; set; }
        [MaxLength(5000)]
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
        public long ControlAccountId { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
        [Required]
        public long DivisionId { get; set; }
        [Required]
        public bool IsVatable { get; set; }
        public bool? CanBeSoldOnline { get; set; }

        public ServiceRelationshipEnum ServiceRelationshipEnum { get; set; }

        [Required]
        public IList<Int64> RequiredDocumentsId { get; set; }
        public IList<Int64> RequiredServiceFieldsId { get; set; }

        [NotMapped]
        public long? DirectServiceId { get; set; }
        public string Alias { get; set; }
        public long? ServiceAccount { get; set; }

    }
}
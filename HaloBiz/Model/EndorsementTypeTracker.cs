using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Model.LAMS;

namespace HaloBiz.Model
{
    public class EndorsementTypeTracker
    {
        [Key]
        public long Id { get; set; }
        public long PreviousContractServiceId { get; set; }
        public long NewContractServiceId { get; set; }
        public string DescriptionOfChange { get; set; }
        public long ApprovedById { get; set; }
        public UserProfile ApprovedBy { get; set; }
        public long EndorsementTypeId { get; set; }
        public EndorsementType EndorsementType { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}


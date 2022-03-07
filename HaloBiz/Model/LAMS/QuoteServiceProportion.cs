using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaloBiz.Helpers;

namespace HaloBiz.Model.LAMS
{
    public class QuoteServiceProportion
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public double Proportion { get; set; }
        [Required]
        public ProportionStatusType Status { get; set; }
        [Required]
        public long StrategicBusinessUnitId { get; set; }
        public StrategicBusinessUnit StrategicBusinessUnit { get; set; }
        public UserProfile UserInvolved { get; set; }        
        public long UserInvolvedId { get; set; }        
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}


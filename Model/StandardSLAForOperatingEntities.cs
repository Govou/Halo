using System.ComponentModel.DataAnnotations;

namespace HaloBiz.Model
{
    public class StandardSLAForOperatingEntities : SetupBaseModel
    {
        [Required, StringLength(2500)]
        public string DocumentUrl { get; set; }
        [Required]
        public long OperatingEntityId { get; set; }
        public virtual OperatingEntity OperatingEntity { get; set; }
    }
}
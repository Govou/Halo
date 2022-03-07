using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model.ManyToManyRelationship
{
    public class ServiceRequredServiceQualificationElement
    {
        public long ServicesId { get; set; }
        public Services Services { get; set; }
        public long RequredServiceQualificationElementId { get; set; }
        public RequredServiceQualificationElement RequredServiceQualificationElement { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
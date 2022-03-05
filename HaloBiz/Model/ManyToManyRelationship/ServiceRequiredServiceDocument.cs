using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.Model.ManyToManyRelationship
{
    public class ServiceRequiredServiceDocument
    {
        public long ServicesId { get; set; }
        public Services Services { get; set; }
        public long RequiredServiceDocumentId { get; set; }
        public RequiredServiceDocument RequiredServiceDocument { get; set; }
        public bool IsDeleted { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
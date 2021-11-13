using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ArmedEscortReceivingDTO
    {
    }
    public class ArmedEscortTypeReceivingDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Caption { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        //public long CreatedById { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
       
       // public virtual UserProfile CreatedBy { get; set; }
    }
    public class ArmedEscortRankReceivingDTO
    {
        [Required]
        public string RankName { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        //public long Sequence { get; set; }
        //public long CreatedById { get; set; }
        public long ArmedEscortTypeId { get; set; }
       
        
    }
}

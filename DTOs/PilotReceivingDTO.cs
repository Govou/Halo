using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.DTOs
{
    public class PilotReceivingDTO
    {
    }

    public class PilotTypeReceivingDTO
    {
        //public long Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        [Required]
        public string TypeDesc { get; set; }
       
    }
    public class PilotRankReceivingDTO
    {
        //public long Id { get; set; }
        [Required]
        public string RankName { get; set; }
        [Required]
        public string Alias { get; set; }
        public string Description { get; set; }
        [Required]
        public long PilotTypeId { get; set; }
        //public string PilotType { get; set; }

    }


}

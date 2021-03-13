using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class SBUProportionReceivingDTO
    {
        public long OperatingEntityId { get; set; }
        public int LeadClosureProportion { get; set; }
        public int LeadGenerationProportion { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class ApproverLevelReceivingDTO : BaseSetupDTO
    {
        public string Alias { get; set; }
    }

    public class ApprovingLevelOfficeReceivingDTO
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public List<int> OfficersIds { get; set; }
    }
}
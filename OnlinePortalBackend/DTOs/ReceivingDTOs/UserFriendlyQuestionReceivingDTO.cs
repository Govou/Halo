using System;
using System.ComponentModel.DataAnnotations;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class UserFriendlyQuestionReceivingDTO
    {
        public long ServiceGroupId { get; set; }
        [Required]
        public string Question { get; set; }
    }
}
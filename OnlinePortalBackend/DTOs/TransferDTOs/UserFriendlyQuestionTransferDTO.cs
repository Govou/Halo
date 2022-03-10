using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class UserFriendlyQuestionTransferDTO
    {
        public long Id { get; set; }
        public long ServiceGroupId { get; set; }
        public string Question { get; set; }
    }
}
using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class WelcomeNoteTransferDTO
    {
        public long Id { get; set; }
        public string BotInformation { get; set; }
        public string WelcomeText { get; set; }
    }
}
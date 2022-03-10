using System;
using System.ComponentModel.DataAnnotations;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class WelcomeNoteReceivingDTO
    {
        public string BotInformation { get; set; }
        public string WelcomeText { get; set; }
    }
}
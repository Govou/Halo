using HalobizMigrations.Models;
using System.Collections.Generic;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class NegotiationDocumentTransferDTO : DocumentSetupTransferDTO
    {
        public long QuoteServiceId { get; set; }
        public QuoteService QuoteService { get; set; }
    }
}
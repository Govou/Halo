using System.Collections.Generic;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class NegotiationDocumentTransferDTO : DocumentSetupTransferDTO
    {
        public long QuoteServiceId { get; set; }
        public QuoteService QuoteService { get; set; }
    }
}
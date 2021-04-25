using System.Collections.Generic;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class QuoteServiceDocumentTransferDTO : DocumentSetupTransferDTO
    {
        public string Type { get; set; }
        public long QuoteServiceId { get; set; }
    }
}
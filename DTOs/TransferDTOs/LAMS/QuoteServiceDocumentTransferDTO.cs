using System.Collections.Generic;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class QuoteServiceDocumentTransferDTO : DocumentSetupTransferDTO
    {
        public long QuoteServiceId { get; set; }
    }
}
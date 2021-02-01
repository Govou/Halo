using System.Collections.Generic;
using HaloBiz.Helpers;
using HaloBiz.Model;
using HaloBiz.Model.LAMS;

namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class QuoteTransferDTO
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; }
        public long LeadDivisionId { get; set; }
        public LeadDivision LeadDivision { get; set; }
        public bool IsConvertedToContract { get; set; }
        public IEnumerable<QuoteServiceTransferDTO> QuoteServices { get; set; }
        public VersionType Version { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
    }
}
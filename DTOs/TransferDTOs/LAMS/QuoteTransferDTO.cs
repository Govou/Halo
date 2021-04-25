using System.Collections.Generic;
using HaloBiz.Helpers;
using HalobizMigrations.Models;


namespace HaloBiz.DTOs.TransferDTOs.LAMS
{
    public class QuoteTransferDTO
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; }
        public long LeadDivisionId { get; set; }
        public LeadDivision LeadDivision { get; set; }
        public bool IsConvertedToContract { get; set; }
        public IEnumerable<QuoteServiceTransferDTO> QuoteService { get; set; }
        public VersionType Version { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
    }
    public class QuoteWithoutLeadDivisionTransferDTO
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; }
        public long LeadDivisionId { get; set; }
        public bool IsConvertedToContract { get; set; }
        public IEnumerable<QuoteServiceTransferDTO> QuoteService { get; set; }
        public VersionType Version { get; set; }
        public long CreatedById { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
    }
}
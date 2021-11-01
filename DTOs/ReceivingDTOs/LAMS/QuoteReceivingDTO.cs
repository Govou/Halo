using HaloBiz.Helpers;
using HalobizMigrations.Models;
using HalobizMigrations.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaloBiz.DTOs.ReceivingDTOs
{
    public class QuoteReceivingDTO
    {
        public long Id { get; set; }
        public string GroupInvoiceNumber { get; set; }
        public GroupQuoteCategory GroupQuoteCategory { get; set; }
        public long LeadDivisionId { get; set; }
        public bool IsConvertedToContract { get; set; } = true;
        public VersionType Version { get; set; } = VersionType.Latest;
        public IEnumerable<QuoteServiceReceivingDTO> QuoteServices { get; set; }
    }
}
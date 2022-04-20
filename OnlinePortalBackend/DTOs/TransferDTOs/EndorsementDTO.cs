using HalobizMigrations.Models;
using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class EndorsementDTO
    {
        public int Quantity { get; set; }
        public DateTime DateOfEffect { get; set; }
        public string Description { get; set; }
        public string DocumentUrl { get; set; }
        public int EndorsementType { get; set; }
        public int ContractServiceId { get; set; }
        public long Id { get; set; }
    }
}

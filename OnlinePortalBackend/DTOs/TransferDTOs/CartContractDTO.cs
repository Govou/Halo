using HalobizMigrations.Models.Shared;
using System;
using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class CartContractDTO
    {
        public long Id { get; set; }
        public long? CustomerDivisionId { get; set; }
        public GroupContractCategory GroupContractCategory { get; set; }
        public string GroupInvoiceNumber { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }

        public List<CartContractDetailDTO> CartContractServices { get; set; }

    }
}

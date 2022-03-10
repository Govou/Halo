using HalobizMigrations.Models;
using System;

namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class ServiceWishlistTransferDTO
    {
        public long Id { get; set; }
        public long ProspectId { get; set; }
        public Prospect Prospect { get; set; }
        public long ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
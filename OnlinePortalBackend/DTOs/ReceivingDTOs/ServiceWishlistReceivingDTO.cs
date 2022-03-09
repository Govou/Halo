using System;
using System.ComponentModel.DataAnnotations;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class ServiceWishlistReceivingDTO
    {
        public long ProspectId { get; set; }
        public long ServiceId { get; set; }
    }
}
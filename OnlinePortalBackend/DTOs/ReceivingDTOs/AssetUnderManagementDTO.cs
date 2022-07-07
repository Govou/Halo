using System.Collections.Generic;

namespace OnlinePortalBackend.DTOs.ReceivingDTOs
{
    public class AssetUnderManagementDTO
    {
        public IEnumerable<Asset> PendingReview { get; set; }
        public IEnumerable<Asset> CompletedReview { get; set; }
    }

    public class Asset
    {
        public string ServiceName { get; set; }
        public string DateAdded { get; set; }
        public string PlateNumber { get; set; }
        public int PercentageCompleted { get; set; }
        public int DaysLeft { get; set; }
        public string ImageUrl { get; set; }
    }
}

﻿namespace OnlinePortalBackend.DTOs.TransferDTOs
{
    public class SupplierDashboardDetails
    {
        public string TotalAssetUnderManagement { get; set; }
        public string DistinctTypes { get; set; }
        public string AssetAwaitingInspection { get; set; }
        public string SupplierName { get; set; }
        public string TotalAssetsDueForReAccreditation { get; set; }
        public string AssetAddedInCurrentMonth { get; set; }
        public string PreferredServiceCentre { get; set; }
    }
}

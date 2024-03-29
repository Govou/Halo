﻿using Halobiz.Common.DTOs.ApiDTOs;
using OnlinePortalBackend.DTOs.AdapterDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public interface ISupplierService
    {
        Task<ApiCommonResponse> GetServiceCenters(string state);
        Task<ApiCommonResponse> NewAssetAddition(AssetAdditionDTO request);
        Task<ApiCommonResponse> GetVehicleMakes();
        Task<ApiCommonResponse> GetVehicleModels(int makeId);
        Task<ApiCommonResponse> GetSupplierCategories();
        Task<ApiCommonResponse> GetDashboardDetails(int profileId);
        Task<ApiCommonResponse> AssetsUnderManagement(int profileId);
        Task<ApiCommonResponse> PostInspectionDetails(InspectionDetailDTO request);
        Task<ApiCommonResponse> CheckIdentificatioNumberExists(string idNumber);
    }
}

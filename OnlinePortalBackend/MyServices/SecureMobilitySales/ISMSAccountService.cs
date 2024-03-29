﻿using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.ReceivingDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices.SecureMobilitySales
{
    public interface ISMSAccountService
    {
        Task<ApiCommonResponse> CreateIndividualAccount(SMSIndividualAccountDTO request);
        Task<ApiCommonResponse> CreateBusinessAccount(SMSBusinessAccountDTO request);
        Task<ApiCommonResponse> GetCustomerProfile(int profileId);
        Task<ApiCommonResponse> UpdateCustomerProfile(ProfileUpdateDTO profileId);
        Task<ApiCommonResponse> CreateSupplierIndividualAccount(SMSSupplierIndividualAccountDTO request);
        Task<ApiCommonResponse> CreateSupplierBusinessAccount(SMSSupplierBusinessAccountDTO request);
    }
}

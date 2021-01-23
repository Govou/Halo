﻿using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IAccountMasterService
    {
        Task<ApiResponse> AddAccountMaster(HttpContext context, AccountMasterReceivingDTO accountMasterReceivingDTO);
        Task<ApiResponse> GetAccountMasterById(long id);
        Task<ApiResponse> GetAccountMasterByAlias(long alias);
        Task<ApiResponse> GetAccountMasterByName(string name);
        Task<ApiResponse> GetAllAccountMasters();
        Task<ApiResponse> UpdateAccountMaster(long id, AccountMasterReceivingDTO accountMasterReceivingDTO);
        Task<ApiResponse> DeleteAccountMaster(long id);
    }
}
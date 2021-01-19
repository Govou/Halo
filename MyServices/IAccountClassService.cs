﻿using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IAccountClassService
    {
        Task<ApiResponse> AddAccountClass(HttpContext context, AccountClassReceivingDTO accountClassReceivingDTO);
        Task<ApiResponse> GetAccountClassById(long id);
        Task<ApiResponse> GetAccountClassByCaption(string caption);
        Task<ApiResponse> GetAllAccountClasses();
        Task<ApiResponse> UpdateAccountClass(long id, AccountClassReceivingDTO accountClassReceivingDTO);
        Task<ApiResponse> DeleteAccountClass(long id);
    }
}

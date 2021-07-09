using HaloBiz.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface ICronJobService
    {
        Task<ApiResponse> MigrateNewCustomersToDTRACK(HttpContext context);
        Task<ApiResponse> PostNewAccountingRecordsToDTRACK(HttpContext context);
    }
}

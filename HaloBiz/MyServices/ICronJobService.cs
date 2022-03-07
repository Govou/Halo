using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface ICronJobService
    {
        Task<ApiCommonResponse> MigrateNewCustomersToDTRACK(HttpContext context);
        Task<ApiCommonResponse> PostNewAccountingRecordsToDTRACK(HttpContext context);
    }
}

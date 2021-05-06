using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IComplaintHandlingService
    {
        Task<ApiResponse> GetComplaintHandlingStats(HttpContext context, long userProfileID);
        Task<ApiResponse> GetComplaintsHandling(HttpContext context, long userProfileID);
    }
}

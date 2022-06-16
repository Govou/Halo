using System.Collections.Generic;
using System.Threading.Tasks;
using HaloBiz.DTOs;
using HaloBiz.DTOs.ApiDTOs;
using HalobizMigrations.Models;
using Microsoft.AspNetCore.Http;
using ApiCommonResponse = Halobiz.Common.DTOs.ApiDTOs.ApiCommonResponse;

namespace HaloBiz.Helpers
{
    public interface IProjectResolver
    {
        Task<ApiCommonResponse> ResolveService(long requestId, HttpContext httpContext);
        // Task<ApiCommonResponse> ResolveLead(long requestId, HttpContext httpContext);

        Task<ApiCommonResponse> ResolveEndorsement(long requestId, HttpContext httpContext);

        // Task<ApiCommonResponse> ResolveContract(long requestId, HttpContext httpContext);
    }
}
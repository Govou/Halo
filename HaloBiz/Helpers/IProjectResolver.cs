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
        Task<Halobiz.Common.DTOs.ApiDTOs.ApiCommonResponse> CreateEndorseMentProject(HttpContext httpContext,ContractServiceForEndorsement endorsement);
        Task<List<ContractFulfillMentStructure>> StructureServices(List<QuoteService> quoteServices);

        Task<ApiCommonResponse> CreateProjectForFulfilmentProject(HttpContext httpContext, LeadDivision leadDivision,
            List<ContractFulfillMentStructure> contractFulfillMentStructures);

        Task<ApiCommonResponse> CreateServiceProject(HttpContext httpContext,Service service);
    }
}
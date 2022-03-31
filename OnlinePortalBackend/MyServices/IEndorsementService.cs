using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IEndorsementService
    {
        Task<ApiCommonResponse> FetchEndorsements(HttpContext context, int limit = 10);
        Task<ApiCommonResponse> TrackEndorsement(HttpContext context, long endorsementId);
        Task<ApiCommonResponse> EndorsementTopUp(HttpContext context, List<ContractServiceForEndorsementReceivingDto> endorsement);
        Task<ApiCommonResponse> EndorsementReduction(HttpContext context, List<ContractServiceForEndorsementReceivingDto> endorsement);
        
    }
}

using Halobiz.Common.DTOs.ApiDTOs;
using Halobiz.Common.DTOs.TransferDTOs;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.TransferDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IEndorsementService
    {
        Task<ApiCommonResponse> FetchEndorsements(int userId);
        Task<ApiCommonResponse> TrackEndorsement(long endorsementId);
        Task<ApiCommonResponse> EndorsementTopUp(int userId, List<EndorsementDTO> endorsement);
        Task<ApiCommonResponse> EndorsementReduction(int userId, List<EndorsementDTO> endorsement);
        Task<ApiCommonResponse> GetContractService(int id);
        Task<ApiCommonResponse> GetContractServices(int userId);
        Task<ApiCommonResponse> GetPossibleDatesOfEffectForEndorsement(int contractServiceId);
    }
}

using Halobiz.Common.DTOs.ApiDTOs;
using Microsoft.AspNetCore.Http;
using OnlinePortalBackend.DTOs.ApiDTOs;
using System.Threading.Tasks;

namespace OnlinePortalBackend.MyServices
{
    public interface IEndorsementService
    {
        Task<ApiCommonResponse> FetchEndorsements(HttpContext context, int limit = 10);
        Task<ApiCommonResponse> TrackEndorsement(HttpContext context, long endorsementId);
    }
}

using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;

namespace HaloBiz.MyServices
{
    public interface IServiceRelationshipService
    {
        Task<ApiCommonResponse> FindServiceRelationshipByAdminId(long Id);
        Task<ApiCommonResponse> FindServiceRelationshipByDirectId(long Id); 
        Task<ApiCommonResponse> FindAllUnmappedDirects();
        Task<ApiCommonResponse> FindAllRelationships();
        Task<ApiCommonResponse> FindAllDirectServices();
    }
}
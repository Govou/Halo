using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;

namespace HaloBiz.MyServices
{
    public interface IServiceRelationshipService
    {
        Task<ApiResponse> FindServiceRelationshipByAdminId(long Id);
        Task<ApiResponse> FindServiceRelationshipByDirectId(long Id); 
        Task<ApiResponse> FindAllUnmappedDirects();
        Task<ApiResponse> FindAllRelationships();
    }
}
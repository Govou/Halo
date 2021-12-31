using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IRelationshipService
    {
        Task<ApiResponse> AddRelationship(HttpContext context, RelationshipReceivingDTO relationshipReceivingDTO);
        Task<ApiResponse> GetAllRelationship();
        Task<ApiResponse> GetRelationshipById(long id);
        Task<ApiResponse> GetRelationshipByName(string name);
        Task<ApiResponse> UpdateRelationship(HttpContext context, long id, RelationshipReceivingDTO relationshipReceivingDTO);
        Task<ApiResponse> DeleteRelationship(long id);
    }
}
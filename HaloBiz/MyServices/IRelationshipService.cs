using System.Threading.Tasks;
using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IRelationshipService
    {
        Task<ApiCommonResponse> AddRelationship(HttpContext context, RelationshipReceivingDTO relationshipReceivingDTO);
        Task<ApiCommonResponse> GetAllRelationship();
        Task<ApiCommonResponse> GetRelationshipById(long id);
        Task<ApiCommonResponse> GetRelationshipByName(string name);
        Task<ApiCommonResponse> UpdateRelationship(HttpContext context, long id, RelationshipReceivingDTO relationshipReceivingDTO);
        Task<ApiCommonResponse> DeleteRelationship(long id);
    }
}
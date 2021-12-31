using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IGroupTypeService
    {
        Task<ApiResponse> AddGroupType(HttpContext context, GroupTypeReceivingDTO groupTypeReceivingDTO);
        Task<ApiResponse> GetAllGroupType();
        Task<ApiResponse> GetGroupTypeById(long id);
        Task<ApiResponse> GetGroupTypeByName(string name);
        Task<ApiResponse> UpdateGroupType(HttpContext context, long id, GroupTypeReceivingDTO groupTypeReceivingDTO);
        Task<ApiResponse> DeleteGroupType(long id);
    }
}
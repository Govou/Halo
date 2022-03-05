using System.Threading.Tasks;
using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;

namespace HaloBiz.MyServices
{
    public interface IGroupTypeService
    {
        Task<ApiCommonResponse> AddGroupType(HttpContext context, GroupTypeReceivingDTO groupTypeReceivingDTO);
        Task<ApiCommonResponse> GetAllGroupType();
        Task<ApiCommonResponse> GetGroupTypeById(long id);
        Task<ApiCommonResponse> GetGroupTypeByName(string name);
        Task<ApiCommonResponse> UpdateGroupType(HttpContext context, long id, GroupTypeReceivingDTO groupTypeReceivingDTO);
        Task<ApiCommonResponse> DeleteGroupType(long id);
    }
}
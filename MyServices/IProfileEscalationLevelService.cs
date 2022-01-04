using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IProfileEscalationLevelService
    {
        Task<ApiCommonResponse> AddProfileEscalationLevel(HttpContext context, ProfileEscalationLevelReceivingDTO profileEscalationLevelReceivingDTO);
        Task<ApiCommonResponse> GetAllProfileEscalationLevel();
        Task<ApiCommonResponse> GetAllHandlerProfileEscalationLevel();
        Task<ApiCommonResponse> GetProfileEscalationLevelById(long id);
        //Task<ApiCommonResponse> GetProfileEscalationLevelByName(string name);
        Task<ApiCommonResponse> UpdateProfileEscalationLevel(HttpContext context, long id, ProfileEscalationLevelReceivingDTO profileEscalationLevelReceivingDTO);
        Task<ApiCommonResponse> DeleteProfileEscalationLevel(long id);
    }
}

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
        Task<ApiResponse> AddProfileEscalationLevel(HttpContext context, ProfileEscalationLevelReceivingDTO profileEscalationLevelReceivingDTO);
        Task<ApiResponse> GetAllProfileEscalationLevel();
        Task<ApiResponse> GetProfileEscalationLevelById(long id);
        //Task<ApiResponse> GetProfileEscalationLevelByName(string name);
        Task<ApiResponse> UpdateProfileEscalationLevel(HttpContext context, long id, ProfileEscalationLevelReceivingDTO profileEscalationLevelReceivingDTO);
        Task<ApiResponse> DeleteProfileEscalationLevel(long id);
    }
}

using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IEscalationLevelService
    {
        Task<ApiResponse> AddEscalationLevel(HttpContext context, EscalationLevelReceivingDTO escalationLevelReceivingDTO);
        Task<ApiResponse> GetAllEscalationLevel();
        Task<ApiResponse> GetEscalationLevelById(long id);
        Task<ApiResponse> GetEscalationLevelByName(string name);
        Task<ApiResponse> UpdateEscalationLevel(HttpContext context, long id, EscalationLevelReceivingDTO escalationLevelReceivingDTO);
        Task<ApiResponse> DeleteEscalationLevel(long id);
    }
}

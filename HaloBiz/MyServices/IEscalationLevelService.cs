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
        Task<ApiCommonResponse> AddEscalationLevel(HttpContext context, EscalationLevelReceivingDTO escalationLevelReceivingDTO);
        Task<ApiCommonResponse> GetAllEscalationLevel();
        Task<ApiCommonResponse> GetEscalationLevelById(long id);
        Task<ApiCommonResponse> GetEscalationLevelByName(string name);
        Task<ApiCommonResponse> UpdateEscalationLevel(HttpContext context, long id, EscalationLevelReceivingDTO escalationLevelReceivingDTO);
        Task<ApiCommonResponse> DeleteEscalationLevel(long id);
    }
}

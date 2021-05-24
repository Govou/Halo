using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface ISuspectService
    {
        Task<ApiResponse> AddSuspect(HttpContext context, SuspectReceivingDTO suspectReceivingDTO);
        Task<ApiResponse> GetAllSuspect();
        Task<ApiResponse> GetUserSuspects(HttpContext context);
        Task<ApiResponse> GetSuspectById(long id);
        //Task<ApiResponse> GetSuspectByName(string name);
        Task<ApiResponse> UpdateSuspect(HttpContext context, long id, SuspectReceivingDTO suspectReceivingDTO);
        Task<ApiResponse> DeleteSuspect(long id);
        Task<ApiResponse> ConvertSuspect(HttpContext context, long suspectId);
    }
}

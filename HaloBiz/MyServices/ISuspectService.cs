using Halobiz.Common.DTOs.ApiDTOs;
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
        Task<ApiCommonResponse> AddSuspect(HttpContext context, SuspectReceivingDTO suspectReceivingDTO);
        Task<ApiCommonResponse> GetAllSuspect();
        Task<ApiCommonResponse> GetUserSuspects(HttpContext context);
        Task<ApiCommonResponse> GetSuspectById(long id);
        //Task<ApiCommonResponse> GetSuspectByName(string name);
        Task<ApiCommonResponse> UpdateSuspect(HttpContext context, long id, SuspectReceivingDTO suspectReceivingDTO);
        Task<ApiCommonResponse> DeleteSuspect(long id);
        Task<ApiCommonResponse> ConvertSuspect(HttpContext context, long suspectId);

        Task<ApiCommonResponse> GetSuspectByNames(string firstname, string lastname);
        Task<ApiCommonResponse> GetSuspectByBusinessName(string businessname);
        Task<ApiCommonResponse> GetSuspectByRC(string rc);
        Task<ApiCommonResponse> GetSuspectByEmail(string email);
    }
}

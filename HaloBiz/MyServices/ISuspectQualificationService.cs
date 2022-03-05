using HaloBiz.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface ISuspectQualificationService
    {
        Task<ApiCommonResponse> AddSuspectQualification(HttpContext context, SuspectQualificationReceivingDTO suspectQualificationReceivingDTO);
        Task<ApiCommonResponse> GetAllSuspectQualification();
        Task<ApiCommonResponse> GetUserSuspectQualification(HttpContext context);
        Task<ApiCommonResponse> GetSuspectQualificationById(long id);
        //Task<ApiCommonResponse> GetSuspectQualificationByName(string name);
        Task<ApiCommonResponse> UpdateSuspectQualification(HttpContext context, long id, SuspectQualificationReceivingDTO suspectQualificationReceivingDTO);
        Task<ApiCommonResponse> DeleteSuspectQualification(long id);
    }
}

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
        Task<ApiResponse> AddSuspectQualification(HttpContext context, SuspectQualificationReceivingDTO suspectQualificationReceivingDTO);
        Task<ApiResponse> GetAllSuspectQualification();
        Task<ApiResponse> GetSuspectQualificationById(long id);
        //Task<ApiResponse> GetSuspectQualificationByName(string name);
        Task<ApiResponse> UpdateSuspectQualification(HttpContext context, long id, SuspectQualificationReceivingDTO suspectQualificationReceivingDTO);
        Task<ApiResponse> DeleteSuspectQualification(long id);
    }
}

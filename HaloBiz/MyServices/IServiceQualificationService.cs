using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IServiceQualificationService
    {
        Task<ApiCommonResponse> AddServiceQualification(HttpContext context, ServiceQualificationReceivingDTO serviceQualificationReceivingDTO);
        Task<ApiCommonResponse> GetAllServiceQualification();
        Task<ApiCommonResponse> GetServiceQualificationById(long id);
        //Task<ApiCommonResponse> GetServiceQualificationByName(string name);
        Task<ApiCommonResponse> UpdateServiceQualification(HttpContext context, long id, ServiceQualificationReceivingDTO serviceQualificationReceivingDTO);
        Task<ApiCommonResponse> DeleteServiceQualification(long id);
    }
}

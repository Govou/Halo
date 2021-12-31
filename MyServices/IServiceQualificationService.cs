using HaloBiz.DTOs.ApiDTOs;
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
        Task<ApiResponse> AddServiceQualification(HttpContext context, ServiceQualificationReceivingDTO serviceQualificationReceivingDTO);
        Task<ApiResponse> GetAllServiceQualification();
        Task<ApiResponse> GetServiceQualificationById(long id);
        //Task<ApiResponse> GetServiceQualificationByName(string name);
        Task<ApiResponse> UpdateServiceQualification(HttpContext context, long id, ServiceQualificationReceivingDTO serviceQualificationReceivingDTO);
        Task<ApiResponse> DeleteServiceQualification(long id);
    }
}

using Halobiz.Common.DTOs.ApiDTOs;
using HaloBiz.DTOs.ReceivingDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.MyServices
{
    public interface IRequredServiceQualificationElementService
    {
        Task<ApiCommonResponse> AddRequredServiceQualificationElement(HttpContext context, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceivingDTO);
        Task<ApiCommonResponse> GetAllRequredServiceQualificationElements();
        Task<ApiCommonResponse> GetAllRequredServiceQualificationElementsByServiceCategory(long serviceCategoryId);
        Task<ApiCommonResponse> GetRequredServiceQualificationElementById(long id);
        Task<ApiCommonResponse> GetRequredServiceQualificationElementByName(string name);
        Task<ApiCommonResponse> UpdateRequredServiceQualificationElement(HttpContext context, long id, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceivingDTO);
        Task<ApiCommonResponse> DeleteRequredServiceQualificationElement(long id);
    }
}

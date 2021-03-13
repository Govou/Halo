using HaloBiz.DTOs.ApiDTOs;
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
        Task<ApiResponse> AddRequredServiceQualificationElement(HttpContext context, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceivingDTO);
        Task<ApiResponse> GetAllRequredServiceQualificationElements();
        Task<ApiResponse> GetRequredServiceQualificationElementById(long id);
        Task<ApiResponse> GetRequredServiceQualificationElementByName(string name);
        Task<ApiResponse> UpdateRequredServiceQualificationElement(HttpContext context, long id, RequredServiceQualificationElementReceivingDTO RequredServiceQualificationElementReceivingDTO);
        Task<ApiResponse> DeleteRequredServiceQualificationElement(long id);
    }
}
